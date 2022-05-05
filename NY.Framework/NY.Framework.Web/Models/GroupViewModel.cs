using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class GroupListViewModel
    {
        public string name { get; set; }
        //public virtual List<GroupViewModel> GList { get; set; }

        public int? id { get; set; }
        public string department { get; set; }
        public int? department_id { get; set; }
        public string service { get; set; }
        public int? service_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public int? uploadedby { get; set; }
        public string uploadedbyname { get; set; }

        ////wmo
        //public int department_id { get; set; }
        //public int service_id { get; set; }
        //public string fromdate { get; set; }
        //public string todate { get; set; }
        //public int uploadedby { get; set; }
        //public string uploadedbyname { get; set; }
    }
    public class GroupViewModel
    {
        public int id { get; set; }
        //public int name { get; set; }
        //public int department_id { get; set; }
        //public int service_id { get; set; }
        //public string fromdate { get; set; }
        //public string todate { get; set; }
        //public int uploadedby { get; set; }
    }

}
