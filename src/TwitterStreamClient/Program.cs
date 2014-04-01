using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterStreamClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            IConfig config = new Config();
            string token = config.Get("token");
            string secret = config.Get("secret");
            string consumerKey = config.Get("consumerKey");
            string consumerSecret = config.Get("consumerSecret");

            var tsClient = new TwitterStreamClient(
               token,
               secret,
               consumerKey,
               consumerSecret,
               new DasUserRepository());

            var block = new AutoResetEvent(false);

            Console.WriteLine("tsClient Start");
            tsClient.Start();
            Console.WriteLine("block waitOne");
            block.WaitOne();
            Console.WriteLine("block end");
        }
    }
}
