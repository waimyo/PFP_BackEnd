using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Entities
{
    public class JQueryDataTableQueryOption
    {
        public string Draw { get; set; }
        public string Start { get; set; }
        public string Length { get; set; }
        public string SortColumn { get; set; }
        public  string SortColumnDirection { get; set; }
        public string SearchValue { get; set; }
    }
}
