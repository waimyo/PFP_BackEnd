using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class SmsInboundViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public string Coding { get; set; }
        public string Origin_Connector { get; set; }

        public class MytelInboundViewModel
        {
            public string SourceAddr { get; set; }
            public string DestAddr { get; set; }
            public string Content { get; set; }
        }

        public class OoredooInboundViewModel
        {
            public string InNumber { get; set; }
            public string Sender { get; set; }     
            public string Content { get; set; }   
        }

        public class MptTelenorInboundViewModel
        {
            public string From { get; set; }
            public string To { get; set; }
            public string Content { get; set; }
            public int Coding { get; set; }
            public string Origin_Connector { get; set; }
        }
    }
}
