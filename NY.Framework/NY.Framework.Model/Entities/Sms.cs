using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    public class Sms : AuditableEntity<int>
    {
        public int Direction { get; set; }
        public int Sms_Code_Id { get; set; }
        [ForeignKey("Sms_Code_Id")]
        public virtual SmsShortCode smsShortCode { get; set; }
        public int? DataInfo_Id { get; set; }
        [ForeignKey("DataInfo_Id")]
        public virtual Data DataInfo { get; set; }
        public DateTime Sms_Time { get; set; }
        public string Sms_Text { get; set; }
        public int? Sms_Count { get; set; }
        public int? Campaign_Id { get; set; }
        [ForeignKey("Campaign_Id")]
        public virtual Campaigns Campaign { get; set; }
        public int Message_Type { get; set; }
        public string Operator { get; set; }
        public string Reason { get; set; }
        public int? Category_Id { get; set; }
        [ForeignKey("Category_Id")]
        public virtual Categories Category { get; set; }
        public int? Categorized_by { get; set; }
        [ForeignKey("Categorized_by")]
        public virtual User CategorizedUser { get; set; }
        public DateTime? Categorized_Time { get; set; }
        public string Sms_Sent_Status { get; set; }

        [Column("created_by")]
        public new int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
    }
}
