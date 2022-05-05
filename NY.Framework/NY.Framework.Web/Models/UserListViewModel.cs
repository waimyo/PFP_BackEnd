using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class UserListViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
        public string parent_ministry { get; set; }
        public string parent_cpu { get; set; }
        public string created_date { get; set; }
    }
}
