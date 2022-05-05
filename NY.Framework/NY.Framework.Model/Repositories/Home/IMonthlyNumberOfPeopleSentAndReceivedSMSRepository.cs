using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories.Home
{
   public interface IMonthlyNumberOfPeopleSentAndReceivedSMSRepository:IReadWriteRepository<MonthlyNumberOfPeopleSentAndReceivedSMS,int>
    {
        List<MonthlyNumberOfPeopleSentAndReceivedSMS> GetMonthlySentAndReceivedCounts(string fromdate,string todate,int ministry_id,int user_id);
    }
}
