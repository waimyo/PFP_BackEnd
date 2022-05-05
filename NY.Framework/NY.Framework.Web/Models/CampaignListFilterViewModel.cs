using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class CampaignListFilterViewModel
    {     
        public string Name { get; set; }
        public bool? Status { get; set; }
        public string SmsMessage { get; set; }
        public string GroupNo { get; set; }
        public int MinistryId { get; set; }
        public int CreatedUserId { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
       
    }
}
