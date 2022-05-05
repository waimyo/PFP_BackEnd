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
    [Table("permission")]
    public class Permission : SoftDeleteEntity<int>
    {
        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public virtual Role role { get; set; }

        public int program_id { get; set; }
        [ForeignKey("program_id")]
        public virtual Program program { get; set; }

        [Column("isview")] // 1
        public AuthorizeAction View { get; set; }

        [Column("iscreateorupdate")] //2
        public AuthorizeAction CreateOrUpdate { get; set; }

        [Column("isdelete")] //3
        public AuthorizeAction Delete { get; set; }

        [Column("isprint")] //4
        public AuthorizeAction Print { get; set; }

        [NotMapped]
        public string program_code { get; set; }
    }
}
