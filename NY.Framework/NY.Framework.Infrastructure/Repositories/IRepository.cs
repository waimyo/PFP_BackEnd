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
    public interface IRepository<TEntity, TEntityID> where TEntity : BaseEntity
    {
        List<TEntity> Get();
       PagedResult<TEntity> GetPagedResults(QueryOptions<TEntity> option);
        List<TEntity> GetListWithFilter(QueryOptions<TEntity> option);
        JQueryDataTablePagedResult<TEntity> GetPagedResults(JqueryDataTableQueryOptions<TEntity> option);
        JQueryDataTablePagedResult<TEntity> GetProcedurePagedResults(JqueryDataTableQueryOptions<TEntity> option, string query, object[] parameters);
        JQueryDataTablePagedResult<TEntity> GetProcedurePagedResultsAsNoTracking(JqueryDataTableQueryOptions<TEntity> option, string query, object[] parameters);
        List<TEntity> GetListWithFilter(JqueryDataTableQueryOptions<TEntity> option);
        TEntity Get(TEntityID id);
    }
}
