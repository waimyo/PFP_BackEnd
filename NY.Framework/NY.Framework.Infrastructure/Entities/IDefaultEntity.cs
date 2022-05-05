using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Entities
{
    public interface IDefaultEntity
    {

        [Column("is_default")]
        bool IsDefault { get; set; }

    }
}
