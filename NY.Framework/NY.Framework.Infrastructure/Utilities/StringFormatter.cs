using NY.Framework.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Infrastructure.Utilities
{
    public static class StringFormatter
    {
        public static string GetAuditStringFormatted(string key, object value)
        {
            string val = "";
            if(value != null)
            {
                if(val is DateTime)
                {

                }
            }

            return key + " : " + value + " | ";
        }

        public static string FormatDateTimeYYYYMMddHHmmssttt(DateTime value)
        {
            string val = "";
            if (value != null)
            {
                val = value.ToString("YYYYMMddHHmmssfff");
            }

            return val;
        }

        
    }
}
