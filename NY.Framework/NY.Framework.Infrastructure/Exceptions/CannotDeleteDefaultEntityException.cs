using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Exceptions
{
    public class CannotDeleteDefaultEntityException : Exception
    {
        public CannotDeleteDefaultEntityException()
            : base("Default Entity cannot be deleted!") { }
    }
}
