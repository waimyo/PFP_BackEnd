using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException()
        : base("Access Denied")
        {
           
        }        
    }
}
