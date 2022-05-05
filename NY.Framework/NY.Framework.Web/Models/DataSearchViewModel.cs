using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class DataSearchViewModel
    {
        public int ministry_id { get; set; }
        public int user_id { get; set; }
        public string gender { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string name { get; set; }
        public string service { get; set; }
    }
}
