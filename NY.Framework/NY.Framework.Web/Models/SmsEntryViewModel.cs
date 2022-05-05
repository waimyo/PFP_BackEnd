using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class SmsEntryViewModel
    {
        public int Direction { get; set; }
        public int SmsCodeId { get; set; }
        public int DataInfoId { get; set; }
        public DateTime SmsTime { get; set; }
        public string SmsText { get; set; }
        public  int CampaignId { get; set; }
        public int MessageType { get; set; }
        public string Operator { get; set; }
        public string Reason { get; set; }
    }
}
