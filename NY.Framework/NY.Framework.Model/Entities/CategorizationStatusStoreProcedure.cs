using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("CategorizationStatusForCampaignDetail")]
    public class CategorizationStatusStoreProcedure : Entity<int>
    {
        public  int Sms_Reply_Count { get; set; }
        public string Category { get; set; }
    }
}
