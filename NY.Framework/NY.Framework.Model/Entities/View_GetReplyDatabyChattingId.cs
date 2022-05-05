using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("View_GetReplyDatabyChattingId")]
    public class View_GetReplyDatabyChattingId:Entity<int>
    {
        public string description { get; set; }
        public DateTime created_date { get; set; }
        public int created_by { get; set; }
        public int sender { get; set; }
        public string sendername { get; set; }
        public int receiver { get; set; }
        public string receivername { get; set; }
        public int chatting_id { get; set; }
        public int reply_chatting_id { get; set; }
        public bool isread { get; set; }
    }
}
