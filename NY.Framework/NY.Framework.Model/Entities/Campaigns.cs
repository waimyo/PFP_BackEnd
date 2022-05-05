using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace NY.Framework.Model.Entities
{
    [Table("campaign")]
    public class Campaigns : AuditableEntity<int>
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Sms_Message { get; set; }
        public string Closing_Message { get; set; }
        public int Sms_Code_Id { get; set; }
        [ForeignKey("Sms_Code_Id")]
        public virtual SmsShortCode smsShortCode { get; set; }
        public int Group_Id { get; set; }
        [ForeignKey("Group_Id")]
        public virtual Groups group { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime? End_Time { get; set; }
        [Column("created_by")]
        public new int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }


    }
}
