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
            string token = "2276962602-nAw6FGdvi0znOodwi3bKk6UA7KVMN25Sz4hLhbo";
            string secret = "OTkcTIO1ndAIWvt2s6zPbVHrOviSSbUN9p3v6xeT2LSyu";
            string consumerKey = "gjj9HQM8FfHKadq97K3g";
            string consumerSecret = "9rD3gfrCT6AzOrSa0ras9dlzlbgLH6RWTGq3ry9uPc0";
            var tsClient = new TwitterStreamClient(token, secret, consumerKey, consumerSecret);

            var block = new AutoResetEvent(false);

            tsClient.Start();

            block.WaitOne();
        }
    }
}
