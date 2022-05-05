using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("chatting")]
    public class Chatting : Entity<int>
    {
        public string description { get; set; }
        public bool ismain { get; set; }
        public DateTime created_date { get; set; }
        public int created_by { get; set; }
    }
}
