using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Entities
{
    public class MonthlyNumberOfPeopleSentAndReceivedSMS:Entity<int>
    {
        public int sentcounts { get; set; }
        public int receivedcounts { get; set; }
        public string months { get; set; }
    }
}
