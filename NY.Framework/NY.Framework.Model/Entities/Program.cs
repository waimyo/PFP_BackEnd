using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Model.Entities
{
    [Table("program")]
    public class Program : SoftDeleteEntity<int>
    {
        public string program_name { get; set; }

        public int parent { get; set; }
        [ForeignKey("parent")]
        public virtual Program program { get; set; }

        public string name { get; set; }
        public int code { get; set; }
        public string href { get; set; }
        public string icon { get; set; }
        public bool? status { get; set; }
        public int index_no { get; set; }
    }
}
