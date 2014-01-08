using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TweetSharp;

namespace TwitterStreamClient
{
    public class TwitterStreamClient
    {
        TwitterService twitterService;
        XDocument doc;

        public TwitterStreamClient(string token, string secret, string consumerKey, string consumerSecret)
        {
            twitterService = new TwitterService(consumerKey, consumerSecret);
            twitterService.AuthenticateWith(token, secret);

Console.WriteLine("init");
            var xmlText = File.ReadAllText("Status.xml");
Console.WriteLine("ReadAll");
            doc = XDocument.Parse(xmlText);
        }

        public void Start()
        {
Console.WriteLine("Start");
            twitterService.StreamUser((streamEvent, response) =>
            {
                Console.WriteLine("Stream");
                if (response.StatusCode == 0)
                {
                    var message = streamEvent as TwitterUserStreamDirectMessage;
                    if (message != null)
                    {
Console.WriteLine(message.DirectMessage.Text);
                        var riders = doc.Descendants("Riders").FirstOrDefault();
                        if(riders != null)
                        {
                            var rider =  riders.Descendants("Rider").SingleOrDefault(r => r.Attribute("Id").Value == message.DirectMessage.SenderId.ToString());
                            if (rider != null)
                            {
                                rider.SetAttributeValue("Name", message.DirectMessage.Sender.Name);
                                rider.SetAttributeValue("Message", message.DirectMessage.Text);
                                rider.SetAttributeValue("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else
                            {
                                riders.Add(new XElement("Rider",
                                    new XAttribute("Id", message.DirectMessage.SenderId),
                                    new XAttribute("Name", message.DirectMessage.Sender.Name),
                                    new XAttribute("Message", message.DirectMessage.Text),
                                    new XAttribute("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                            }
                        }
                        doc.Save("Status.xml");
                    }
                }
            });
        }
    }
}
