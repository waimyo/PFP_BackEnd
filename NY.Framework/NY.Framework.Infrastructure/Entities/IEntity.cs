using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Entities
{
    public interface IEntity<TEntityID>
    {
        TEntityID ID { get; set; }
    }
}
