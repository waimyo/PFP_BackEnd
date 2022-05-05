using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class CampaignDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string SmsMessage { get; set; }
        public string ClosingMessage { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string GroupNo { get; set; }
        public int TotalParticipantCount { get; set; }
        public int ResponseCount { get; set; }
        public double ResponsePercent { get; set; }
        public int CategorizedResponseCount { get; set; }
        public int UncategorizedResponseCount { get; set; }
        public int NotReplyCount { get; set; }

        public class CampaignDetailListViewModel
        {
            public string Name { get; set; }
            public string Mobile { get; set; }
            public string DepartmentName { get; set; }
            public string Gender { get; set; }
            public string CategorizedResponse { get; set; }
            public string ResponseMessage { get; set; }
            public string SmsMessage { get; set; }
            public string ResponseMessageTime { get; set; }
         

        }
    }
}
