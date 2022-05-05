using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Infrastructure.Entities
{
    public class ScalarValueEntity<TType> : BaseEntity
    {
        public TType Value { get; set; }
    }
}
