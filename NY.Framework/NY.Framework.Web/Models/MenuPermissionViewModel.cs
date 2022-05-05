using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class MenuPermissionViewModel
    {
        public string program_name { get; set; }
        public int parent { get; set; }
        public string name { get; set; }
        public int code { get; set; }
        public string href { get; set; }
        public string icon { get; set; }
        public bool? status { get; set; }
        public int index_no { get; set; }

        public string text { get; set; }

        public int view { get; set; }
        public int saveorUpdate { get; set; }
        public int delete { get; set; }
        public int print { get; set; }

        public virtual List<SubMenuViewModel> children { get; set; }
    }
    public class SubMenuViewModel
    {
        public string program_name { get; set; }
        public int parent { get; set; }
        public string name { get; set; }
        public int code { get; set; }
        public string href { get; set; }
        public string icon { get; set; }
        public bool? status { get; set; }
        public int index_no { get; set; }

        public string text { get; set; }

        public int view { get; set; }
        public int saveorUpdate { get; set; }
        public int delete { get; set; }
        public int print { get; set; }
    }
}
