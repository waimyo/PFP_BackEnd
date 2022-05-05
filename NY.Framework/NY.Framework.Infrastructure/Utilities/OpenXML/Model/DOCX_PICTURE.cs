using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Utilities.OpenXML.Model
{
    public class DOCX_PICTURE
    {
        public string ID { get; set; }
        public string FilePath { get; set; }

        public DOCX_PICTURE()
        {

        }

        public DOCX_PICTURE(string id, string filePath)
        {
            this.ID = id;
            this.FilePath = filePath;

        }
    }
}
