using Microsoft.Extensions.Caching.Distributed;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Model;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Application.Interfaces
{
    public interface IDistributedCacheService
    {

        byte[] Get(string key);
        void Remove(string key);
        Task RemoveAllProgram();
        Task RemoveAll();
    }
}
