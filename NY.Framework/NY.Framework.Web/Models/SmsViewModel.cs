using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class SmsViewModel
    {
        public int id { get; set; }
        public int direction { get; set; }
        public int sms_code { get; set; }
        public int datainfo_id { get; set; }
        public string sms_time { get; set; }
        public string sms_text { get; set; }
        public int? campaign_id { get; set; }
        public int message_type { get; set; }
        public string operators { get; set; }
        public string reason { get; set; }
        public int? category_id { get; set; }
        public string categoryname { get; set; }
        public string phono { get; set; }
        public DateTime smstime { get; set; }
        public int? categorized_by { get; set; }
        public string name { get; set; }
        public List<SmsViewModel> selectrows { get; set; }
        public string campaign { get; set; }
      
    }
}
