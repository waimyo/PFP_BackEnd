using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("SP_Response_Status")]
    public class ResponseStatus : Entity<int>
    {
        public string name { get; set; }
        public string username { get; set; }
        public string role_name { get; set; }
        public int ministry_id { get; set; }
    //    public DateTime sms_time { get; set; }
        public int sms_send_count { get; set; }
        public int sms_receive_count { get; set; }
        public Double? response_rate { get; set; }

     
    }
}
