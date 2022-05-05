using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("chatting_participant")]
    public class ChattingParticipant : Entity<int>
    {
        public int sender { get; set; }
        [ForeignKey("sender")]
        public virtual User Sender { get; set; }

        public int receiver { get; set; }
        [ForeignKey("receiver")]
        public virtual User Receiver { get; set; }

        public int chatting_id { get; set; }
        public int reply_chatting_id { get; set; }
        public bool isread { get; set; }
    }
}
