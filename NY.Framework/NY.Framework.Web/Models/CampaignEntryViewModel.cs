using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class CampaignEntryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string SmsMessage { get; set; }
        public string ClosingMessage { get; set; }
        public int SmsCodeId { get; set; }
        public string SmsShortCodeText { get; set; }
        public int GroupId { get; set; }
    
    }
  
}
