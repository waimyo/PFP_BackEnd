using NY.Framework.Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace NY.Framework.Model.Entities
{
    [Table("ministry")]
    public class Ministry: AuditableEntity<int>
    {
        public string name { get; set; }
        public bool istraining { get; set; }
        public string logo { get; set; }
    }
}
