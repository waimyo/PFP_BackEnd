using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Utilities.OpenXML
{
    public class ReportFileViewModel
    {
        public byte[] file { get; set; }
        public string filename { get; set; }
        public bool success { get; set; }
    }
}
