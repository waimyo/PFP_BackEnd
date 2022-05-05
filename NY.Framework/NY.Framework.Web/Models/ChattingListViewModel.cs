using Microsoft.AspNetCore.Http;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Models
{
    public class ChattingListViewModel
    {
        public int id { get; set; }
        public int chat_id { get; set; }
        public int countfornunread { get; set; }
        public string sender { get; set; }
        public string receiver { get; set; }
        public string description { get; set; }
        public string created_date { get; set; }

        
        public string fromdate { get; set; }
        public string todate { get; set; }

        public List<ChattingFile> File { get; set; }
    }

    public class files
    {
        IFormFile file { get; set; }
    }
}
