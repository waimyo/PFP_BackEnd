using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Entities
{
    public abstract class AuditableEntity<TEntityID> : Entity<TEntityID>, IAuditableEntity, ISoftDeleteEntity
    {
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("modified_date")]
        public DateTime? ModifiedDate { get; set; }

        [Column("modified_by")]
        public int? ModifiedBy { get; set; }

        [Column("deleted")]
        public bool IsDeleted { get; set; }
    }
}
