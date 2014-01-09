using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetinCore.Interfaces.TwitterToken;
using TwitterToken;
using Tweetinvi;
using Streaminvi;

namespace TwitterStreamClient
{
    public class TwitterStreamClient2
    {
        IToken _token;
        public TwitterStreamClient2(string token, string secret, string consumerKey, string consumerSecret)
        {
            _token = new Token(token, secret, consumerKey, consumerSecret);
        }

        public void Start()
        {
            var stream = new UserStream(_token);
            stream.MessageReceivedFromX += (sender, args) =>
            {
                Console.WriteLine("Message '{0}' received from {1}!", args.Value.Text, args.Value3.Id);
            };
            stream.StartStream();
        }
    }
}
