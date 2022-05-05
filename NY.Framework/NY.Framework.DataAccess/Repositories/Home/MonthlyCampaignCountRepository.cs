using NY.Framework.Model.Entities.Home;
using NY.Framework.Model.Repositories.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories.Home
{
   public class MonthlyCampaignCountRepository:ReadWriteRepositoryBase<MonthlyCampaignCount,int>,IMonthlyCampaignCountRepository
    {
        public MonthlyCampaignCountRepository(IDbContext context)
           : base(context, new string[] { })
        {

        }

        public List<MonthlyCampaignCount> GetMonthlyCount(string fromdate,string todate,int ministry_id,int user_id)
        {
            return RawSQL<MonthlyCampaignCount>("exec SP_MonthlyCampaignCount {0},{1},{2},{3}", new object[] {fromdate,todate,ministry_id,user_id}).ToList();
        }

      
    }
}
