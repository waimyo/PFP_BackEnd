using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("group_member")]
    public class GroupMember : AuditableEntity<int>
    {
        [Column("group_id")]
        public int group_id { get; set; }
        [ForeignKey("datainfo_id")]
        public virtual Groups groups { get; set; }

        public int datainfo_id { get; set; }
        [ForeignKey("datainfo_id")]
        public virtual Data data { get; set; }
        
    }
}
