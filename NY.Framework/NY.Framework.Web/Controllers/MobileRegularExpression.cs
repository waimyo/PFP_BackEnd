using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NY.Framework.Web.Controllers
{
    public class MobileRegularExpression
    {
        public string Checkoperator(string phonenumber)
        {
            //string strRegex = @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]
            //    {2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)";
            string mptPattern = @"(^(092|093|094|095|0973|098|0991)[0-9]+$)";
            string mytelPattern = @"(^(096)[0-9]+$)";
            string telenorPattern = @"(^(0975|0976|0977|0978|0979)[0-9]+$)";
            string ooredooPattern = @"(^(0995|0996|0997|0998)[0-9]+$)";

            Regex mptRegex = new Regex(mptPattern);
            Regex mytelRegex = new Regex(mytelPattern);
            Regex telenorRegex = new Regex(telenorPattern);
            Regex ooredooRegex = new Regex(ooredooPattern);
            if (mptRegex.IsMatch(phonenumber))
            {
                return "mpt1111";
            }
            else if (mytelRegex.IsMatch(phonenumber))
            {
                return "mytel1111";
            }
            else if (telenorRegex.IsMatch(phonenumber))
            {
                return "telenor1111";
            }
            else if (ooredooRegex.IsMatch(phonenumber))
            {
                return "ooredoo1111";
            }
            return "none";
        }
    }
}
