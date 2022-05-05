using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class CategorizationStatusListViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string role_name { get; set; }
        public int satisfied { get; set; }
        public int dissatisfied { get; set; }
        public int suggestion { get; set; }
        public int appreciation { get; set; }
        public int not_relevant { get; set; }
        public int corruption { get; set; }
        public int other { get; set; }
        public int charge { get; set; }
        public int no_charge { get; set; }
        public int categorized_count { get; set; }
        public int non_categorized_count { get; set; }
        public int? grand_total { get; set; }
    }
}
