using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("View_ParticipantByChatting")]
    public class View_ParticipantByChatting:Entity<int>
    {
        public string username { get; set; }
        public int chatting_id { get; set; }
    }
}
