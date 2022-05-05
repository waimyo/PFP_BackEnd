using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories.Home
{
  public  class MonthlyNumberOfPeopleSentAndReceivedSMSRepository:ReadWriteRepositoryBase<MonthlyNumberOfPeopleSentAndReceivedSMS,int>,IMonthlyNumberOfPeopleSentAndReceivedSMSRepository
    {
        public MonthlyNumberOfPeopleSentAndReceivedSMSRepository(IDbContext context):base(context, new string[] { })
        {
        }
        public List<MonthlyNumberOfPeopleSentAndReceivedSMS> GetMonthlySentAndReceivedCounts(string fromdate,string todate,int ministry_id,int user_id)
        {
            return RawSQL<MonthlyNumberOfPeopleSentAndReceivedSMS>("exec SP_MonthlyNumberOfPeopleSentAndReceivedSMS {0},{1},{2},{3}", new object[] { fromdate,todate,ministry_id,user_id }).ToList();
        }
    }
}
