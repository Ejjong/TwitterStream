using Mindscape.Raygun4Net;
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
        RaygunClient _raygunClient = new RaygunClient("xmDvb3dv/nA8cfGeaqLV8Q==");
        TwitterService twitterService;
        IDasUserRepository dasRepo;
        readonly string token;
        readonly string secret;
        readonly string consumerKey;
        readonly string consumerSecret;

        public TwitterStreamClient(string token, string secret, string consumerKey, string consumerSecret, IDasUserRepository dasRepo)
        {
            this.token = token;
            this.secret = secret;
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.dasRepo = dasRepo;
        }

        void CreateService()
        {
            twitterService = new TwitterService(consumerKey, consumerSecret);
            twitterService.AuthenticateWith(token, secret);
        }

        public void Start()
        {
            try
            {
                CreateService();
                OnStart();
            }
            catch (Exception e)
            {
                twitterService.CancelStreaming();
                twitterService = null;

                Console.WriteLine("Start Exception!!!");
                _raygunClient.Send(e);
                CreateService();
                OnStart();
            }
        }

        internal void OnStart()
        {
            var result = twitterService.StreamUser((streamEvent, response) =>
            {
                if (response.StatusCode == 0)
                {
                    var message = streamEvent as TwitterUserStreamDirectMessage;
                    if (message != null)
                    {
                        var strings = message.Text.Split(',');
                        if (strings.GetStatus() == "" && message.DirectMessage.SenderId != 229481394) return;

                        try
                        {
                            var twitterId = message.DirectMessage.SenderId;
                            var user = new DasUser
                            {
                                TwitterId = (int)message.DirectMessage.SenderId,
                                Name = message.DirectMessage.Sender.Name,
                                Status = strings.GetStatus(),
                                Message = strings.GetMessage()
                            };
                            dasRepo.InsertOrUpate(user);
                            twitterService.SendDirectMessage(new SendDirectMessageOptions() { UserId = message.DirectMessage.SenderId, Text = strings.GetStatus() });
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("StreamUser Exception!!!");
                            _raygunClient.Send(e);
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            });
        }
    }
}
