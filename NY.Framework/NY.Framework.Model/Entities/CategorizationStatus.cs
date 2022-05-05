using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("SP_Categorization_Status")]
    public class CategorizationStatus : Entity<int>
    {
        public string name { get; set; }
        public string username { get; set; }
        public string role_name { get; set; }
        public int min_id { get; set; }
        public string ministry_name { get; set; }
        //  public DateTime sms_time { get; set; }
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
        public int grand_total { get; set; }
    }
}
