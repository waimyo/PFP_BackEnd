using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities.Home
{
   public class MonthlyCampaignCount:Entity<int>
    {
        public string yearmonths { get; set; }
        public int months { get; set; }
        public int counts { get; set; }
    }
}
