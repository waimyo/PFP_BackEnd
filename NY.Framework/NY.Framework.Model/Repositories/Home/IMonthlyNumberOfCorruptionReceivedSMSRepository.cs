using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories.Home
{
   public interface IMonthlyNumberOfCorruptionReceivedSMSRepository:IReadWriteRepository<MonthlyNumberOfCorruptionReceivedSMS,int>
    {
        List<MonthlyNumberOfCorruptionReceivedSMS> GetCorruptionReceivedSMs(string fromdate,string todae,int ministry_id,int user_id);
    }
}
