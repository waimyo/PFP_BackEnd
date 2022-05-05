using NY.Framework.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Entities
{
    [Table("audit_view")]
    public class UserLog : Entity<int>
    {
        [Column("user_name")]
        public string UserName { get; set; }

        [Column("ip_address")]
        public string IPAddress { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("nm")]
        public string Name { get; set; }

        [Column("dept_nm")]
        public string Department { get; set; }

        [Column("program")]
        public string Program { get; set; }

        [Column("time_accessed")]
        public DateTime TimeAccessed { get; set; }

        [Column("data")]
        public string Data { get; set; }

        [Column("action")]
        public string Action { get; set; }

        //[Column("ministry_id")]
        //public int Ministry { get; set; }

        public int? role_level { get; set; }


    }

}
