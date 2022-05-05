using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("CampaignDetailList")]
    public class CampaignDetailListStoreProcedure : Entity<int>
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Department { get; set; }
        public bool Gender { get; set; }
        public string SentMessage { get; set; }
        public string ReplyMessage { get; set; }
        public DateTime? ReplyDate { get; set; }
        public string Category { get; set; }

    }
}
