using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Model.Entities
{
    [Table("user")]
    public class User : AuditableEntity<int>
    {
        public string name { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; } 
        
        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public virtual Role Role { get; set; }

        public int ministry_id { get; set; }
        [ForeignKey("ministry_id")]
        public virtual Ministry Ministry { get; set; }

        //public int parent_min { get; set; }
        //[ForeignKey("parent_min")]
        //public virtual User ParentMIN { get; set; }

        //public int parent_cpu { get; set; }
        //[ForeignKey("parent_cpu")]
        //public virtual User ParentCPU { get; set; }

        public int parent_id { get; set; }
        [ForeignKey("parent_id")]
        public virtual User ParentUser { get; set; }

        public string profile_img { get; set; }
        public bool status { get; set; }

        public int location_id { get; set; }
        [ForeignKey("location_id")]
        public virtual Location Location { get; set; }

        //public int location_district { get; set; }
        //public int location_township { get; set; }
        
        [NotMapped]
        public string Token { get; set; }
    }
}
