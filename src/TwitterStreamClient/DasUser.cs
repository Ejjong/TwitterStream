
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamClient
{
    [CompositeIndex("id", "twitterid", Unique = true)]
    [Alias("dasuser")]
    public class DasUser
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Alias("twitterid")]
        public int TwitterId { get; set; }
        public string Name {get; set;}
        public string Status {get; set;}
        public string Message {get; set;}
        public DateTime? Date { get; set; }
    }
}
