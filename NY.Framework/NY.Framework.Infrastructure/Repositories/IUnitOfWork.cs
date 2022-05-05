using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }

    
}
