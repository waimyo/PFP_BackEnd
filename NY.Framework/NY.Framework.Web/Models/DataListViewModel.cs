using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class DataListViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string gender { get; set; }
        public string ministry { get; set; }
        public string department { get; set; }
        public string service { get; set; }
        public string statedivision { get; set; }
        public string district { get; set; }
        public string township { get; set; }
        public string date_of_application { get; set; }
        public string date_of_completion { get; set; }
        public string created_by { get; set; }
        public string created_date { get; set; }

        //wmo
        public int department_id { get; set; }
        public int service_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public int uploadedby { get; set; }
        public string uploadedbyname { get; set; }
    }
}
