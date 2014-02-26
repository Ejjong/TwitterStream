
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamClient
{
    [Alias("dasuser")]
    public class DasUser
    {
        [PrimaryKey]
        public int TwitterId { get; set; }
        [StringLength(40)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        [StringLength(280)]
        public string Message { get; set; }
        public DateTime? Date { get; set; }
    }
}
