using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("service")]
    public class Service: AuditableEntity<int>
    {
        public string name { get; set; }       
        [Column("dept_id")]
        public int Dept_id { get; set; }      
        [ForeignKey("Dept_id")]
        public virtual Department Department { get; set; }
    }
}
