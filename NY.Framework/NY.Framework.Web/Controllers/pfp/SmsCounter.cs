

using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NY.Framework.Web
{
    public class SmsCounter
    {

        public bool IsUnicode(string smstext)
        {
            var asciiBytesCount = Encoding.ASCII.GetByteCount(smstext);
            var unicodBytesCount = Encoding.UTF8.GetByteCount(smstext);
            return asciiBytesCount != unicodBytesCount;
        }

        public void GetSmsCount(Sms sms)
        {
            int unicodechar_count = 0, unicodechar_count_overlimit = 0, engchar_count = 0, engchar_count_overlimit = 0;

            if (sms.Operator.Equals("ooredoo1111"))
            {
                unicodechar_count = NY.Framework.Infrastructure.Constants.OoredooUnicodeCharCount;
                unicodechar_count_overlimit = NY.Framework.Infrastructure.Constants.OoredooUnicodeCharCountOver70;
                engchar_count = NY.Framework.Infrastructure.Constants.OoredooEngCharCount;
                engchar_count_overlimit = NY.Framework.Infrastructure.Constants.OoredooEngCharCountOver160;

            }
            else if (sms.Operator.Equals("mytel1111"))
            {
                unicodechar_count = NY.Framework.Infrastructure.Constants.MytelUnicodeCharCount;
                unicodechar_count_overlimit = NY.Framework.Infrastructure.Constants.MytelUnicodeCharCountOver70;
                engchar_count = NY.Framework.Infrastructure.Constants.MytelEngCharCount;
                engchar_count_overlimit = NY.Framework.Infrastructure.Constants.MytelEngCharCountOver160;
            }
            else if (sms.Operator.Equals("telenor1111"))
            {
                unicodechar_count = NY.Framework.Infrastructure.Constants.TelenorUnicodeCharCount;
                unicodechar_count_overlimit = NY.Framework.Infrastructure.Constants.TelenorUnicodeCharCountOver70;
                engchar_count = NY.Framework.Infrastructure.Constants.TelenorEngCharCount;
                engchar_count_overlimit = NY.Framework.Infrastructure.Constants.TelenorEngCharCountOver140;
            }
            else if (sms.Operator.Equals("mpt1111"))
            {
                unicodechar_count = NY.Framework.Infrastructure.Constants.MptUnicodeCharCount;
                unicodechar_count_overlimit = NY.Framework.Infrastructure.Constants.MptUnicodeCharCountOver70;
                engchar_count = NY.Framework.Infrastructure.Constants.MptEngCharCount;
                engchar_count_overlimit = NY.Framework.Infrastructure.Constants.MptEngCharCountOver140;
            }
            else if (sms.Operator.Equals("none"))
            {
                unicodechar_count = sms.Sms_Text.Length;
                unicodechar_count_overlimit = sms.Sms_Text.Length;
                engchar_count = sms.Sms_Text.Length;
                engchar_count_overlimit = sms.Sms_Text.Length;
            }

            /*** if text msg is unicode ***/
            if (IsUnicode(sms.Sms_Text))
            {
                if (sms.Sms_Text.Length <= unicodechar_count)
                {
                    /*** if text length less than or equal 70 char,count as 1 msg ***/
                    sms.Sms_Count = 1;
                }
                else
                {
                    /*** 
                    if text length greater than 70 char (contain 71 chars) , 
                    it broken up into two parts , first part contain 67 char , second part contain 4 char 
                    ***/
                    if (sms.Operator.Equals("telenor1111"))
                    {
                        string before_s_c = sms.Sms_Text.Substring(0, unicodechar_count);
                        string after_s_c = sms.Sms_Text.Substring(unicodechar_count);
                        int s_count = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(after_s_c.Length) / unicodechar_count_overlimit));
                        sms.Sms_Count = s_count + 1;
                    }
                    else
                    {
                        int s_count = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(sms.Sms_Text.Length) / unicodechar_count_overlimit));
                        sms.Sms_Count = s_count;
                    }

                }
            }
            else
            {
                /*** if text is not  unicode ***/
                if (sms.Sms_Text.Length <= engchar_count)
                {
                    sms.Sms_Count = 1;
                }
                else
                {
                    int e_count = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(sms.Sms_Text.Length) / engchar_count_overlimit));
                    sms.Sms_Count = e_count;
                }
            }

        }

    }
}