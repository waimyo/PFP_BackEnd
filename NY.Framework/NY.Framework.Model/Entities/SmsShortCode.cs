using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("sms_shortcode")]
    public class SmsShortCode : AuditableEntity<int>
    {
        public string Sms_Code { get; set; }
        public int Ministry_Id { get; set; }
    }
}
