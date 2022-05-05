using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Entities.Home
{
  public  class NumberOfPeopleSentAndReceivedSMS : Entity<int>
    {
        public int sentcounts { get; set; }
        public int receivedcounts { get; set; }
    }
}
