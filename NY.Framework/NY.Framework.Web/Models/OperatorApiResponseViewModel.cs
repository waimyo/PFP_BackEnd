using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class OperatorApiResponseViewModel
    {
        public class OoredooApiResponseViewModel
        {
           // public string[] errors;
            public string status;
        }
        public class MytelApiResponseViewModel
        {
            //public string errorCode;
            //public List<MytelApiResponse> result;
            public string message;
        }

        public class MytelApiResponse
        {
            public string smsId;
            public string sourceAddress;
            public string destAddress;
            public string finalStatus;
            public string delivered;
            public string doneDate;
            public string error;
            public string submitDate;
            public string submited;
            public string submitSMSCTime;
            public string receiveSMSCReportTime;
        }

    }
}
