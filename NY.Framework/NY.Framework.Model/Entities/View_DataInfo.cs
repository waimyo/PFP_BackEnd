using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("View_DataInfo")]
    public class View_DataInfo: Entity<int>
    {
        public string name { get; set; }
        public string mobile { get; set; }
        public bool gender { get; set; }
        public DateTime date_of_application { get; set; }
        public DateTime date_of_completion { get; set; }
        public DateTime created_date { get; set; }

        public int userid { get; set; }
        public string username { get; set; }

        public int parentuserid { get; set; }

        public int ministry_id { get; set; }
        public string ministry { get; set; }

        public int departmentid { get; set; }
        public string department { get; set; }

        public int serviceid { get; set; }
        public string service { get; set; }

        public string statedivision { get; set; }
        public string district { get; set; }
        public string township { get; set; }
    }
}
