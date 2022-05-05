using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Entities
{
    public abstract class SoftDeleteEntity<TEntityID> : Entity<TEntityID>, ISoftDeleteEntity
    {
        [Column("deleted")]
        public bool IsDeleted { get; set; }
    }
}
