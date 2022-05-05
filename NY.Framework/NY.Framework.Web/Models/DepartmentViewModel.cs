using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class DepartmentViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int ministry_id { get; set; }
        public string ministry_name { get; set; }
    }
}
