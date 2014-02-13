using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamClient
{
    [Table("DasUser")]
    public class DasUser
    {
        public int Id { get; set; }
        public int TwitterId { get; set; }
        public string Name {get; set;}
        public string Status {get; set;}
        public string Message {get; set;}
        public DateTime? Date { get; set; }
    }
}
