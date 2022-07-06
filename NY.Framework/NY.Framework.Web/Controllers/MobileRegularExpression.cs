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

        public bool CheckMPTLength(string phonenumber)
        {
            string mpt7digit = @"(^(0920|0921|0922|0923|0924|0950|0951|0952|0953|0954|0955|0956|0981|0983|0984|0985|0986|0987)[0-9]+$)";
            string mpt8digit = @"(^(0941|0943|0973|0991|0949|0947)[0-9]+$)";
            string mpt9digit = @"(^(0925|0926|0940|0942|0944|0945|0988|09890|09891|09892|09893|09894|09895|098960|098961|098962|098969|09897|09898|09899|098963|098964|098965|098966|098967|098968)[0-9]+$)";

            Regex mpt7digitRegex = new Regex(mpt7digit);
            Regex mpt8digitRegex = new Regex(mpt8digit);
            Regex mpt9digitRegex = new Regex(mpt9digit);

            if (mpt7digitRegex.IsMatch(phonenumber))
            {
                if (phonenumber.Length == 10)
                    return true;
                else
                    return false;
            }
            else if (mpt8digitRegex.IsMatch(phonenumber))
            {
                if (phonenumber.Length == 11)
                    return true;
                else
                    return false;
            }
            else if (mpt9digitRegex.IsMatch(phonenumber))
            {
                if (phonenumber.Length == 12)
                    return true;
                else
                    return false;
            }
            return false;
        }
    }
}
