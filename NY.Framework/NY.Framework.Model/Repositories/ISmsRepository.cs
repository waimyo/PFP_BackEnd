using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Infrastructure.Repositories
{
    public interface ISmsRepository : IReadWriteRepository<Sms,int>
    {
        //for campaign detail TotalResponseCount
        int GetTotalResponseCountByCampaignId(int campid);

        //for campaign detail CategorizedResponseCount
         int GetCategorizedResponseCountByCampaignId(int campid);

        //for campaign detail UnCategorizedResponseCount
         int GetUnCategorizedResponseCountByCampaignId(int campid);

        //Checking  Sender Number include in Campaign
        Sms GetCampaignBySenderMobileNumber(string mobile);
        int GetSendingCounts(int id);
        List<Sms> GetSmsMobileAndCampaignId(int campid, string mobile);
    }
}
