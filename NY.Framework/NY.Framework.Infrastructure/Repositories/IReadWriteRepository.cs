using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Repositories
{
    public interface IReadWriteRepository<TEntity, TEntityID> : IRepository<TEntity, TEntityID> where TEntity : BaseEntity
    {
        void Save(TEntity entity);
        void Save(List<TEntity> entities);
        void Remove(TEntity entity);
        void Remove(List<TEntity> entities);

    }
}
