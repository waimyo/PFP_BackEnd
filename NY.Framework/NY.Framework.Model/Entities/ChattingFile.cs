using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("chatting_file")]
    public class ChattingFile:Entity<int>
    {
        public int chatting_id { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
    }
}
