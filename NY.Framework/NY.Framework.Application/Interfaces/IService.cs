using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Application.Interfaces
{
    public interface IService<TEntity, TEntityID> where TEntity : NY.Framework.Infrastructure.Entities.BaseEntity
    {

        
    }
}
