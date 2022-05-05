using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Infrastructure.Utilities
{
    public class ConvertToMyanmarNumber
    {
        public string ConvertToMyanmarNo(string no)
        {
            if (!string.IsNullOrEmpty(no))
            {
                return no.ToString().Replace("0", "၀").Replace("1", "၁").Replace("2", "၂").Replace("3", "၃").Replace("4", "၄").Replace("5", "၅")
                                     .Replace("6", "၆").Replace("7", "၇").Replace("8", "၈").Replace("9", "၉");
            }
            else
            {
                return no;
            }

        }
    }
}
