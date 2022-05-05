using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class DataViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public bool gender { get; set; }
        public int service_id { get; set; }
        public int department_id { get; set; }
        public int ministry_id { get; set; }
        public DateTime date_of_application { get; set; }
        public DateTime date_of_completion { get; set; }
        public int location_state { get; set; }
        public int location_district { get; set; }
        public int location_township { get; set; }        
        public int uploaded_file_id { get; set; }
        public IFormFile uploadfile { get; set; }
    }
}
