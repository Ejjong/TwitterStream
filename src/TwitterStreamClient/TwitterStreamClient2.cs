using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetinCore.Interfaces.TwitterToken;
using TwitterToken;
using Tweetinvi;
using Streaminvi;
using System.Xml.Linq;
using System.IO;

namespace TwitterStreamClient
{
    public class TwitterStreamClient2
    {
        IToken _token;
        XDocument doc;

        public TwitterStreamClient2(string token, string secret, string consumerKey, string consumerSecret)
        {
            _token = new Token(token, secret, consumerKey, consumerSecret);
            var xmlText = File.ReadAllText("Status.xml");
            doc = XDocument.Parse(xmlText);
        }

        public void Start()
        {
            var stream = new UserStream(_token);
            stream.MessageReceivedFromX += (sender, args) =>
            {
                if (args == null) return;

                var riders = doc.Descendants("Riders").FirstOrDefault();
                if (riders != null)
                {
                    var rider = riders.Descendants("Rider").SingleOrDefault(r => r.Attribute("Id").Value == args.Value3.Id.ToString());
                    if (rider != null)
                    {
                        rider.SetAttributeValue("Name", args.Value3.Name);
                        rider.SetAttributeValue("Message", args.Value.Text);
                        rider.SetAttributeValue("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        riders.Add(new XElement("Rider",
                            new XAttribute("Id", args.Value3.Id),
                            new XAttribute("Name", args.Value3.Name),
                            new XAttribute("Message", args.Value.Text),
                            new XAttribute("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))));
                    }
                }
                doc.Save("Status.xml");
            };
            stream.StartStream();
        }
    }
}
