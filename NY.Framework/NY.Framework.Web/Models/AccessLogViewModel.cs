using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class AccessLogViewModel
    {
        public int id { get; set; }

        public string access_time { get; set; }
        
        public string username { get; set; }
        
        public string role { get; set; }
        
        public string page_accessed { get; set; }

        public string action { get; set; }

        public string ip_address { get; set; }

        public string fromdate { get; set; }
        public string todate { get; set; }
    }
}
