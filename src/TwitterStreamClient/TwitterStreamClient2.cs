using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TweetinCore.Interfaces;
using TweetinCore.Interfaces.TwitterToken;
using Tweetinvi;
using TwitterToken;
using Streaminvi;
using System.Xml.Linq;
using System.IO;
using System.Data.Common;
using System.Collections.Generic;
using Mindscape.Raygun4Net;
using System.Data;
using ServiceStack.OrmLite;

namespace TwitterStreamClient
{
    //public class TwitterStreamClient2
    //{    
    //    long masterId = 229481394;

    //    readonly string token;
    //    readonly string secret;
    //    readonly string consumerKey;
    //    readonly string consumerSecret;
    //    readonly IDasUserRepository dasRepo;

    //    public TwitterStreamClient2(string token, string secret, string consumerKey, string consumerSecret, IDasUserRepository dasRepo)
    //    {
    //        this.token = token;
    //        this.secret = secret;
    //        this.consumerKey = consumerKey;
    //        this.consumerSecret = consumerSecret;
    //        this.dasRepo = dasRepo;
    //        Console.WriteLine("ctor");
    //    }

    //    IToken CreateToken(string token, string secret, string consumerKey, string consumerSecret)
    //    {
    //        return new Token(token, secret, consumerKey, consumerSecret);
    //        Console.WriteLine("create token");
    //    }

    //    public void Start()
    //    {
    //        var token = CreateToken(this.token, this.secret, this.consumerKey, this.consumerSecret);
    //        OnStart(token);
    //    }

    //    internal void OnStart(IToken _token)
    //    {
    //        try
    //        {
    //            var stream = new UserStream(_token);
    //            Console.WriteLine("create user stream");
    //            stream.MessageReceivedFromX += (sender, args) =>
    //            {
    //                if (args == null || args.Value3.Id == null) return;

    //                var strings = args.Value.Text.Split(',');
    //                if (strings.GetStatus() == "" && args.Value3.Id != masterId) return;

    //                try
    //                {
    //                    var twitterId = args.Value3.Id;
    //                    var user = new DasUser
    //                    {
    //                        TwitterId = (int)args.Value3.Id,
    //                        Name = args.Value3.Name,
    //                        Status = strings.GetStatus(),
    //                        Message = strings.GetMessage()
    //                    };
    //                    dasRepo.InsertOrUpate(user);
    //                    Console.WriteLine("insert or update user");
    //                    SendMessage(args.Value3.Id, strings.GetStatus(), _token);
    //                    Console.WriteLine("send message");
    //                }
    //                catch (Exception e)
    //                {
    //                    Console.WriteLine("inner exception");
    //                    Console.WriteLine(e.Message);
    //                    Start();
    //                }
    //            };
    //            stream.StartStream();
    //            Console.WriteLine("start stream");
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("root exception");
    //            Console.WriteLine(ex.Message);
    //            Start();
    //        }
    //    }

    //    private static IMessage CreateNewMessage(long? receiverId, string message)
    //    {
    //        IUser receiver = new User(receiverId);
    //        IMessage msg = new Message(message, receiver);

    //        return msg;
    //    }

    //    private static void SendMessage(long? receiverId, string message, IToken token)
    //    {
    //        IMessage msg = CreateNewMessage(receiverId, message);
    //        msg.Publish(token);
    //    }
    //}

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
