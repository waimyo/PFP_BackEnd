using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories.Home
{
   public interface INumberOfPeopleSentAndReceivedSMSRepository:IReadWriteRepository<NumberOfPeopleSentAndReceivedSMS,int>
    {
        NumberOfPeopleSentAndReceivedSMS GetSentAndReceivedCounts(string fromdate, string todate,int ministry_id,int user_id);
    }
}
