using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NY.Framework.Model.Entities
{
    [Table("View_Location")]
    public class View_Location:Entity<int>
    {
        public string state_division { get; set; }
        public string district { get; set; }
        public string township { get; set; }
    }
}
