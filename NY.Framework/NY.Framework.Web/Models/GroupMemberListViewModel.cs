using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class GroupMemberListViewModel
    {
        public int Id { get; set; }
        public int group_id { get; set; }
        public int datainfo_id { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public int? sender_id { get; set; }
        public string sender_name { get; set; }
        public string created_date { get; set; }

       
        public virtual List<GroupMemberInsertViewModel> GMList { get; set; }

        public List<GroupMemberListViewModel> selectrows { get; set; }
    }

    public class GroupMemberInsertViewModel
    {
        public int id { get; set; }
        public int group_id { get; set; }
        public int[] datainfo { get; set; }
        public string name { get; set; }
    }
}
