using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
   
    [Table("SP_GroupMemberList")]
    public class SP_GroupMemberList : Entity<int>
    {
        //public int groupmember_id { get; set; }
        public int group_id { get; set; }
        public int datainfo_id { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public int? sender_id { get; set; }
        public string sender_name { get; set; }
        public DateTime? created_date { get; set; }
        public string ministry_name { get; set; }
    }
}
