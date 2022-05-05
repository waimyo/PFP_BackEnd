using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("View_GroupList")]
    public class View_GroupList:Entity<int>
    {
      
        public string group_name { get; set; }
        public string username { get; set; }
        public int group_member { get; set; }
        public DateTime? group_date { get; set; }
        public string gdate { get; set; }
        //public string ministry_name { get; set; }
        //public int? ministry_id { get; set; }
        public int ministry_id { get; set; }
        public int user_id { get; set; }
        public int parent_id { get; set; }
        //public int created_by { get; set; }
    }
}
