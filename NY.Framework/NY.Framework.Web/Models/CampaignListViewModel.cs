using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class CampaignListViewModel
    {
        public int Id { get; set; }
        public string CampaignSerailNo { get; set; }
        public string CampaignCreatedBy { get; set; }     
        public string CampaignName { get; set; }
        public string CampaignStatus { get; set; }
        public string SmsMessage { get; set; }
        public string ClosingMessage { get; set; }
        public string GroupName { get; set; }
        public string StartTimeEndTime { get; set; }
    }
}
