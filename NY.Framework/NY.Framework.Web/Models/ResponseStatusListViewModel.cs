using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class ResponseStatusListViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string role_name { get; set; }
        public int sms_send_count { get; set; }
        public int sms_receive_count { get; set; }
        public string response_rate { get; set; }
    }
}
