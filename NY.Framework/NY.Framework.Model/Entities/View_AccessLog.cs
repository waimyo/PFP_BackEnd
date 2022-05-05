using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("SP_AccessLog")]
    public class View_AccessLog:Entity<int>
    {
        [Column("access_time")]
        public DateTime access_time { get; set; }

        [Column("username")]
        public string username { get; set; }

        [Column("role")]
        public string role { get; set; }

        [Column("page_accessed")]
        public string page_accessed { get; set; }

        [Column("action")]
        public string action { get; set; }

        [Column("ip_address")]
        public string ip_address { get; set; }
    }
}
