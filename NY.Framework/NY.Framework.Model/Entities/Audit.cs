using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("access_log")]
    public class Audit:Entity<int>
    {
        public string page_accessed { get; set; }
        public string ip_address { get; set; }
        public int? role_id { get; set; }
        public string action { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public bool deleted { get; set; }
    }
}
