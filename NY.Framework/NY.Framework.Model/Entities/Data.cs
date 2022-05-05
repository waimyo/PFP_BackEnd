using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("datainfo")]
    public class Data: AuditableEntity<int>
    {
        public string name { get; set; }
        public string mobile { get; set; }
        public bool gender { get; set; }

        public int service_id { get; set; }
        [ForeignKey("service_id")]
        public virtual Service Service { get; set; }

        public int department_id { get; set; }
        [ForeignKey("department_id")]
        public virtual Department Department { get; set; }

        public int ministry_id { get; set; }
        [ForeignKey("ministry_id")]
        public virtual Ministry Ministry { get; set; }

        public DateTime date_of_application { get; set; }
        public DateTime date_of_completion { get; set; }

        [ForeignKey("location_state_id")]
        public virtual Location LocationStateDivision { get; set; }
        public int location_district_id { get; set; }

        [ForeignKey("location_district_id")]
        public virtual Location LocationDistrict { get; set; }
        public int location_township_id { get; set; }

        [ForeignKey("location_township_id")]
        public virtual Location LocationTownship { get; set; }
        public int location_state_id { get; set; }
        
        public int uploaded_file_id { get; set; }

        [Column("created_by")]
        public new int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }
    }
}
