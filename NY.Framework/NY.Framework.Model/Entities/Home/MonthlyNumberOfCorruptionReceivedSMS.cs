using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Entities.Home
{
   public class MonthlyNumberOfCorruptionReceivedSMS:Entity<int>
    {
        public string months { get; set; }
        public int corruptioncounts { get; set; }
    }
}
