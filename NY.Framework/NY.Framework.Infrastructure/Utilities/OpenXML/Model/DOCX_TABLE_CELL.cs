using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Utilities.OpenXML.Model
{
    public class DOCX_TABLE_CELL
    {
        public string Value { get; set; }
        public DOCX_TABLE_CELL()
        {
        }

        public DOCX_TABLE_CELL(string val)
        {
            Value = val;
        }
    }
}
