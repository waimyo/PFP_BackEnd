using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class GroupSearchViewModel
    {
        //public int Id { get; set; }
        //public int group_no { get; set; }
        //public string group_name { get; set; }
        //public string sender_group { get; set; }
        //public int? sender_group_id { get; set; }
        //public int group_member { get; set; }
        //public string group_date { get; set; }
        public int? ministry_id { get; set; }
        public string ministry_name { get; set; }
        //public string fromdate { get; set; }
        //public string todate { get; set; }

        public int Id { get; set; }
        public string group_name { get; set; }
        public string username { get; set; }
        public int group_member { get; set; }
        public string group_date { get; set; }
        public int user_id { get; set; }
        public int parent_id { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
    }
}
