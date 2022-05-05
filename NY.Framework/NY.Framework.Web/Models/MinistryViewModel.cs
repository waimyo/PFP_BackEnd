using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class MinistryViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public IFormFile logo { get; set; }
        public byte[] imagebyte { get; set; }
    }
}
