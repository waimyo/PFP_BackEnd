
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Model.Entities
{
    [Table("role")]
    public class Role : SoftDeleteEntity<int>
    {
        public string name { get; set; }
        public bool isdefault { get; set; }
    }
}
