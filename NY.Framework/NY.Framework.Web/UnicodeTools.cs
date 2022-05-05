using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Web
{
    public class UnicodeTools
    {
        public string Encode(string text)
        {
            Encoding ucs2 = Encoding.BigEndianUnicode;
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(text);
            byte[] ucs2Bytes = Encoding.Convert(utf8, ucs2, utfBytes);
            string hexstr = ByteArrayToString(ucs2Bytes);
            return hexstr;
        }

        public string Decode(string hexstring)
        {
            Encoding ucs2 = Encoding.BigEndianUnicode;
            Encoding utf8 = Encoding.UTF8;
            byte[] ucs2byte = ucs2.GetBytes(hexstring);
            byte[] utf8byte = Encoding.Convert(ucs2, utf8, ucs2byte);
            string unicodetext = ucs2.GetString(utf8byte);
            return unicodetext;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)          
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
