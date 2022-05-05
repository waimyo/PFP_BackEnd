using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities.Home
{
    [Table("SP_SmsCountByOperator")]
    public class SmsCountByOperator : Entity<int>
    {
        public string month { get; set; }
        public int mpt_sms_in { get; set; }
        public int mpt_sms_out { get; set; }
        public int telenor_sms_in { get; set; }
        public int telenor_sms_out { get; set; }
        public int mytel_sms_in { get; set; }
        public int mytel_sms_out { get; set; }
        public int ooredoo_sms_in { get; set; }
        public int ooredoo_sms_out { get; set; }
     //   public int year { get; set; }

    }
}
