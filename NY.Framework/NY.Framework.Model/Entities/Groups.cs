using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("group")]
    public class Groups: AuditableEntity<int>
    {
        [Column("name")]
        public string Name { get; set; }

        //public string
        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }
        [Column("created_by")]
        public new int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        public override string ToAuditString()
        {
            return StringFormatter.GetAuditStringFormatted("id", ID.ToString()) +
                    StringFormatter.GetAuditStringFormatted("name", Name);
        }
    }
}
