using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("category")]
    public class Categories: AuditableEntity<int>
    {
        public string name { get; set; }        
    }
}
