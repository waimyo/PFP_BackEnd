using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class SmsRepository : ReadWriteRepositoryBase<Sms,int>,ISmsRepository
    {
        public SmsRepository(IDbContext dbContext) 
            : base(dbContext,new string[] { })
        {

        }

        //Checking  Sender Number include in Campaign
        public Sms GetCampaignBySenderMobileNumber(string mobile)
        {
            return CustomQuery().Where(s=>s.Direction==(int)SmsDirection.Sent && s.Campaign_Id!=0 &&
                                                                   s.DataInfo.mobile.Equals(mobile)).OrderByDescending(s=>s.ID).FirstOrDefault();
        }

        //for campaign detail TotalResponseCount
        public int GetTotalResponseCountByCampaignId(int campid)
        {
            return CustomQuery().Where(s => s.Campaign_Id == campid && s.Direction == (int)SmsDirection.Received &&
          s.Message_Type == (int)MessageType.Valid_Reply && s.IsDeleted == false).Select(s => s.DataInfo_Id).Distinct().Count();

        }

        //For Campaign Detail CategorizedResponseCount

        public int GetCategorizedResponseCountByCampaignId(int campid)
        {
            return CustomQuery().Where(s => s.Campaign_Id == campid && s.Direction == (int)SmsDirection.Received &&
            s.Message_Type == (int)MessageType.Valid_Reply && s.Category_Id > 0 && s.IsDeleted == false).Select(s => s.DataInfo_Id).Count();
        }

        //For Campaign Detail UnCategorizedResponseCount
        public int GetUnCategorizedResponseCountByCampaignId(int campid)
        {
            return CustomQuery().Where(s => s.Campaign_Id == campid && s.Direction == (int)SmsDirection.Received &&
           s.Message_Type == (int)MessageType.Valid_Reply && s.Category_Id == null && s.IsDeleted == false).Select(s=>s.DataInfo_Id).Count();
        }
        public int GetSendingCounts(int id)
        {
           return CustomQuery().Where(b => b.ID.Equals(id)).Count();
        }
        public List<Sms> GetSmsMobileAndCampaignId(int campid,string mobile)
        {
            return CustomQuery().Where(g => g.Campaign_Id == campid && g.DataInfo.mobile==mobile && g.Direction==2 && g.IsDeleted == false).ToList();
        }

     
    }
}
