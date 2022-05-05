using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class ChattingViewModel
    {
        public int mainchatting_id { get; set; }
        public int[] receiverarr { get; set; }
        public string description { get; set; }
        public IFormFile attachfile { get; set; }
        public bool ismain { get; set; }
        public IFormCollection uploadfiles { get; set; }
    }
}
