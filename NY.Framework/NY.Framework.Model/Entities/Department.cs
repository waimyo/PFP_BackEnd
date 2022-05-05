using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("department")]
    public class Department: AuditableEntity<int>
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("ministry_id")]
        public int Ministry_Id { get; set; }
        [ForeignKey("Ministry_Id")]
        public virtual Ministry Ministry { get; set; }
    }
}
