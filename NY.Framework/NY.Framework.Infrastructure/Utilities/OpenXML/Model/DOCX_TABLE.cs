using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Utilities.OpenXML.Model
{
    public class DOCX_TABLE
    {
        
        public string Caption { get; set; }
        public string FontName { get; set; }
        public string FontSize { get; set; }

        public List<DOCX_TABLE_ROW> Rows { get; set; }

        public DOCX_TABLE(string caption, string fontName, string fontSize)
        {
            
            Rows = new List<DOCX_TABLE_ROW>();
            this.Caption = caption;
            this.FontName = fontName;
            this.FontSize = fontSize;
        }
    }
}
