using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class UserFilterViewModel
    {
        public int ministry_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public int role_id { get; set; }
        public string search { get; set; }
    }
}
