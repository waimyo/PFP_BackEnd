using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class UserEntryViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string new_password { get; set; }
        public int role_id { get; set; }
        public int ministry_id { get; set; }
        public int parent_minid { get; set; }
        public int parent_id { get; set; }
        //public int parent_cpu { get; set; }
        public string profile_img { get; set; }
        public bool status { get; set; }        
        public int location_state { get; set; }
        public int location_district { get; set; }
        public int location_township { get; set; }

        // for header
        public string minlogo { get; set; }
        public string minname { get; set; }

        public string Token { get; set; }
        public bool isFirstApiCall { get; set; }
        public DateTime apiCallJwtExpire { get; set; }
    }
}
