using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models.Home
{
    public class TotalCampaignCountViewModel
    {
        public int id { get; set; }
        public int counts { get; set; }
        public int sentcounts { get; set; }
        public int receivedcounts { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string year { get; set; }
        public int ministry_id { get; set; }
        public int user_id { get; set; }
        public int direction { get; set; }
        public int yearmonths { get; set; }
        public int? corruptioncounts {get;set;}
    }
}
