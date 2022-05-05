using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    public class Location : AuditableEntity<int>
    {
        public int Location_Type { get; set; }  
        public string Name { get; set; }
        public string Pcode { get; set; }
        public int? Parent_Id { get; set; }
        [ForeignKey("Parent_Id")]
        public virtual Location ParentLocation { get; set; }


    }
}
