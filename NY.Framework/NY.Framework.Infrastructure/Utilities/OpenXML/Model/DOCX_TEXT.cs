using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Utilities.OpenXML.Model
{
    public class DOCX_TEXT
    {
        public string ID { get; set; }
        public string Value { get; set; }

        public DOCX_TEXT()
        {

        }

        public DOCX_TEXT(string id, string val)
        {
            ID = id;
            Value = val;

        }
    }
}
