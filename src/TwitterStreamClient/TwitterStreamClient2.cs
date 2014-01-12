using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TweetinCore.Interfaces;
using TweetinCore.Interfaces.TwitterToken;
using Tweetinvi;
using TwitterToken;
using Streaminvi;
using System.Xml.Linq;
using System.IO;

namespace TwitterStreamClient
{
    public class TwitterStreamClient2
    {
        readonly IToken _token;

        public TwitterStreamClient2(string token, string secret, string consumerKey, string consumerSecret)
        {
            Console.WriteLine("ctor");
            _token = new Token(token, secret, consumerKey, consumerSecret);
            Console.WriteLine("create token");
        }

        public void Start()
        {
            try
            {
                Console.WriteLine("start");
                var stream = new UserStream(_token);
                var curPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                stream.MessageReceivedFromX += (sender, args) =>
                {
                    if (args == null || args.Value3.Id == null) return;

                    var strings = args.Value.Text.Split(',');
                    if (strings.GetStatus() == "" && args.Value3.Id != 229481394) return;

                    var xmlText = string.Empty;
                    Console.WriteLine("before read");
                    try
                    {
                        xmlText = File.ReadAllText(Path.Combine(curPath, "Status.xml"));

                        Console.WriteLine("after read");
                        var doc = XDocument.Parse(xmlText);

                        var riders = doc.Descendants("Riders").FirstOrDefault();
                        if (riders != null)
                        {
                            var rider =
                                riders.Descendants("Rider")
                                    .SingleOrDefault(r => r.Attribute("Id").Value == args.Value3.Id.ToString());
                            if (rider != null)
                            {
                                switch (strings.GetStatus())
                                {
                                    case "Online":
                                    case "Riding":
                                        rider.SetAttributeValue("Name", args.Value3.Name);
                                        rider.SetAttributeValue("Status", strings.GetStatus());
                                        rider.SetAttributeValue("Message", strings.GetMessage());
                                        rider.SetAttributeValue("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        SendMessage(args.Value3.Id, strings.GetStatus(), _token);
                                        break;
                                    case "Offline":
                                        rider.Remove();
                                        SendMessage(args.Value3.Id, strings.GetStatus(), _token);
                                        break;
                                    default:
                                        if (args.Value3.Id == 229481394)
                                        {
                                            SendMessage(args.Value3.Id, "pingtest", _token);
                                            return;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                if (strings.GetStatus() == "Offline")
                                {
                                    SendMessage(args.Value3.Id, "already Offline", _token);
                                    return;
                                }
                                if (strings.GetStatus() == "") return;

                                riders.Add(new XElement("Rider",
                                    new XAttribute("Id", args.Value3.Id),
                                    new XAttribute("Name", args.Value3.Name),
                                    new XAttribute("Status", strings.GetStatus()),
                                    new XAttribute("Message", strings.GetMessage()),
                                    new XAttribute("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                                SendMessage(args.Value3.Id, strings.GetStatus(), _token);
                            }

                            var tempRiders = riders.Descendants("Rider").ToList();
                            var ords = tempRiders.OrderByDescending(el => el.Attribute("Date").Value).ToList();
                            riders.RemoveAll();
                            foreach (XElement tab in ords)
                                riders.Add(tab);
                        }

                        Console.WriteLine("before save");
                        doc.SaveToJson(Path.Combine(curPath, "Status.json"));
                        doc.Save(Path.Combine(curPath, "Status.xml"));
                        Console.WriteLine("after save");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                };
                stream.StartStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static IMessage createNewMessage(long? receiverId, string message)
        {
            IUser receiver = new User(receiverId);
            IMessage msg = new Message(message, receiver);

            return msg;
        }

        private static void SendMessage(long? receiverId, string message, IToken token)
        {
            Console.WriteLine("before send message");
            IMessage msg = createNewMessage(receiverId, message);
            msg.Publish(token);
            Console.WriteLine("after send message");
        }
    }

    internal static class XDocumentExtensions
    {
        public static void SaveToJson(this XDocument doc, string fileName)
        {
            string jsonText = Regex.Replace(JsonConvert.SerializeXNode(doc.FirstNode), "(?<=\")(@)(?!.*\":\\s )",
                string.Empty, RegexOptions.IgnoreCase);
            File.WriteAllText(fileName, jsonText);
        }
    }

    internal static class StringArrayExtensions
    {
        public static string GetStatus(this string[] strings)
        {
            switch (strings.FirstOrDefault())
            {
                case "1":
                    return "Online";
                case "2":
                    return "Riding";
                case "0":
                    return "Offline";
                default:
                    return string.Empty;
            }
        }

        public static string GetMessage(this string[] strings)
        {
            if (strings == null || !strings.Any() || strings.Length == 1 || (strings.Length > 1 && strings[1] == null)) return string.Empty;

            return strings[1];
        }
    }
}
