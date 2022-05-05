using NY.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Exceptions;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Infrastructure.Pagination;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace NY.Framework.DataAccess
{
    public class RepositoryBase<TEntity, TEntityID> : IRepository<TEntity, TEntityID> where TEntity : BaseEntity
    {
        protected readonly IDbContext context;
        protected string[] includes;
        public RepositoryBase(IDbContext context, string[] includes)
        {
            this.context = context;
            this.includes = includes;

        }

        public List<TEntity> Get()
        {
            return context.Set<TEntity>().ToList();
        }

        public PagedResult<TEntity> GetPagedResults(QueryOptions<TEntity> option)
        {
            PagedResult<TEntity> results = new PagedResult<TEntity>();
            if (option.FilterBy != null)
            {
                results.total = context.Set<TEntity>().Where(option.FilterBy).Count();
            }
            else
            {
                results.total = context.Set<TEntity>().Count();
            }

            if (results.total > 0)
            {
                var query = context.Set<TEntity>();
                if (option.FilterBy != null)
                {
                    query.Where(option.FilterBy);
                }


                foreach (string s in includes)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        query.Include(s);
                    }
                }
                if (option.FilterBy != null)
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        

                        var q = query.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }

                        results.entities = q.Skip(option.fromRecord).Take(option.recordPerPage).ToList();
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = query.Where(option.FilterBy).OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }

                        results.entities = tmp.Skip(option.fromRecord).Take(option.recordPerPage).ToList();
                    }
                }
                else
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = query.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);
                        
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        results.entities = q.Skip(option.fromRecord).Take(option.recordPerPage).ToList();
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = query.OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }

                        results.entities = tmp.Skip(option.fromRecord).Take(option.recordPerPage).ToList();
                    }
                }

                results.Pager.CurrentPageNumber = option.fromPage;
                results.Pager.TotalRecordCount = results.total;
                results.Pager.RecordPerPage = option.recordPerPage;


            }
            return results;
        }

        public List<TEntity> GetListWithFilter(QueryOptions<TEntity> option)
        {
            PagedResult<TEntity> results = new PagedResult<TEntity>();
            if (option.FilterBy != null)
            {
                results.total = context.Set<TEntity>().Where(option.FilterBy).Count();
            }
            else
            {
                results.total = context.Set<TEntity>().Count();
            }

            if (results.total > 0)
            {
                var query = context.Set<TEntity>();
                if (option.FilterBy != null)
                {
                    query.Where(option.FilterBy);
                }


                foreach (string s in includes)
                {
                    query.Include(s);
                }
                if (option.FilterBy != null)
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = query.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        results.entities = q.ToList();
                        
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = query.Where(option.FilterBy).OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        results.entities = tmp.ToList();
                    }
                }
                else
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = query.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        results.entities = q.ToList();
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = query.OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        results.entities = tmp.ToList();
                    }
                }
               
            }
            return results.entities;
        }

        public JQueryDataTablePagedResult<TEntity> GetPagedResults(JqueryDataTableQueryOptions<TEntity> option)
        {
            JQueryDataTablePagedResult<TEntity> results = new JQueryDataTablePagedResult<TEntity>();
            results.draw = option.Draw;
            if (option.FilterBy != null)
            {
                results.recordsTotal = context.Set<TEntity>().Where(option.FilterBy).Count();
            }
            else
            {
                results.recordsTotal = context.Set<TEntity>().Count();

            }

            if (results.recordsTotal > 0)
            {
                var query = context.Set<TEntity>();              
                foreach (string s in includes)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        query.Include(s);
                    }
                }
                if (option.FilterBy != null)
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {                        
                        var q = query.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 0)
                        {
                            for (int i = 0; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        /***for all records***/
                        if (option.Length == -1)
                        {
                            results.data = q.ToList();
                        }
                        /***for caselist excel records***/
                        else if (option.Length == -2)
                        {
                            results.data = q.Skip(option.Start).Take(results.recordsTotal).ToList();
                        }
                        else
                        {
                            results.data = q.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = query.Where(option.FilterBy).OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 0)
                        {
                            for (int i = 0; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        /***for all records***/
                        if (option.Length == -1)
                        {
                            results.data = tmp.ToList();
                        }
                        /***for caselist excel records***/
                        else if (option.Length == -2)
                        {
                            results.data = tmp.Skip(option.Start).Take(results.recordsTotal).ToList();
                        }
                        
                        else
                        {
                           results.data = tmp.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                }
                else
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = query.OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 0)
                        {
                            for (int i = 0; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        /***for all records***/
                        if (option.Length == -1)
                        {
                            results.data = q.ToList();
                        }
                        /***for caselist excel records***/
                        else if (option.Length == -2)
                        {
                            results.data = q.Skip(option.Start).Take(results.recordsTotal).ToList();
                        }
                        else
                        {
                            results.data = q.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = query.OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 0)
                        {
                            for (int i = 0; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        // var sql = tmp.tosq();
                        /***for all records***/
                        if (option.Length == -1)
                        {
                            results.data = tmp.ToList();
                        }
                        /***for caselist excel records***/
                        else if (option.Length == -2)
                        {
                            results.data = tmp.Skip(option.Start).Take(results.recordsTotal).ToList();
                        }
                        else
                        {
                            results.data = tmp.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                }

                results.recordsFiltered = results.recordsTotal;
                

            }
            return results;
        }

        
        public List<TEntity> GetListWithFilter(JqueryDataTableQueryOptions<TEntity> option)
        {
            PagedResult<TEntity> results = new PagedResult<TEntity>();
            if (option.FilterBy != null)
            {
                results.total = context.Set<TEntity>().Where(option.FilterBy).Count();
            }
            else
            {
                results.total = context.Set<TEntity>().Count();
            }

            if (results.total > 0)
            {
                var query = context.Set<TEntity>();
                if (option.FilterBy != null)
                {
                    query.Where(option.FilterBy);
                }


                foreach (string s in includes)
                {
                    query.Include(s);
                }
                if (option.FilterBy != null)
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = query.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        results.entities = q.ToList();

                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = query.Where(option.FilterBy).OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        results.entities = tmp.ToList();
                    }
                }
                else
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = query.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        results.entities = q.ToList();
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = query.OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        results.entities = tmp.ToList();
                    }
                }

            }
            return results.entities;
        }

        public TEntity Get(TEntityID id)
        {
            if (this.includes != null)
            {
                IQueryable<TEntity> query = context.Set<TEntity>().AsQueryable();
                foreach (string s in includes)
                {
                    if(!string.IsNullOrEmpty(s))
                    {
                        query.Include(s);
                    }                   
                }

                //predicate = (i=> i.ID == id); 
                var arg = Expression.Parameter(typeof(TEntity), "i");
                var predicate =
                    Expression.Lambda<Func<TEntity, bool>>(
                        Expression.Equal(
                            Expression.Property(arg, "ID"),
                            Expression.Constant(id)),
                        arg);
                return query.Where(predicate).FirstOrDefault();
            }
            else
            {
                return context.Set<TEntity>().Find(id);
            }

        }

        protected virtual IQueryable<TEntity> CustomQuery()
        {
            //db.users.where(s=>s.name=="su" && s.id==1);
            return context.Set<TEntity>().AsQueryable();
        }

        protected virtual IQueryable<TEntity> CustomQuery1()
        {
            //db.users.where(s=>s.name=="su" && s.id==1);
            return context.Set<TEntity>().AsNoTracking();
        }

        protected IQueryable<TView> RawSQL<TView>(string query, object[] parameters) where TView : BaseEntity
        {
            return context.Set<TView>().FromSql<TView>(query, parameters).AsQueryable();
          

        }

        protected ScalarValueEntity<TType> ScalarSQL<TType>(string query, object[] parameters) 
        {
            return  context.Set<ScalarValueEntity<TType>>().FromSql<ScalarValueEntity<TType>>(query, parameters).FirstOrDefault();

  

        }

        public JQueryDataTablePagedResult<TEntity>  GetProcedurePagedResults(JqueryDataTableQueryOptions<TEntity> option, string query, object[] parameters)
        {
            JQueryDataTablePagedResult<TEntity> results = new JQueryDataTablePagedResult<TEntity>();
            results.draw = option.Draw;

            if (option.FilterBy != null)
            {
                results.recordsTotal = context.Set<TEntity>().FromSql<TEntity>(query, parameters).Where(option.FilterBy).Count();
            }
            else
            {
                results.recordsTotal = context.Set<TEntity>().FromSql<TEntity>(query, parameters).Count();
            }
            if (results.recordsTotal > 0)
            {
            var qu = context.Set<TEntity>().FromSql<TEntity>(query, parameters).AsQueryable();
             
                if (option.FilterBy != null)
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = qu.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                         /*For All Records*/
                        if (option.Length == -1)
                        {
                            results.data = q.ToList();
                        }
                        else
                        {
                            results.data = q.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = qu.Where(option.FilterBy).OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        /*For All Records*/
                        if (option.Length == -1)
                        {
                            results.data = tmp.ToList();
                        }
                        else
                        {
                            results.data = tmp.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                }
                else
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = qu.OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        /*For All Records*/
                        if (option.Length == -1)
                        {
                            results.data = q.ToList();
                        }
                        else
                        {
                            results.data = q.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                    else
                    {
                        IOrderedEnumerable<TEntity> tmp = qu.OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        /*For All Records*/
                        if (option.Length == -1)
                        {
                            results.data = tmp.ToList();
                        }
                        else
                        {
                            results.data = tmp.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                }

                results.recordsFiltered = results.recordsTotal;


            }
            return results;
        }

        public JQueryDataTablePagedResult<TEntity> GetProcedurePagedResultsAsNoTracking(JqueryDataTableQueryOptions<TEntity> option, string query, object[] parameters)
        {
            JQueryDataTablePagedResult<TEntity> results = new JQueryDataTablePagedResult<TEntity>();
            results.draw = option.Draw;

            if (option.FilterBy != null)
            {
                results.recordsTotal = context.Set<TEntity>().FromSql<TEntity>(query, parameters).Where(option.FilterBy).Count();
            }
            else
            {
                results.recordsTotal = context.Set<TEntity>().FromSql<TEntity>(query, parameters).Count();
            }
            if (results.recordsTotal > 0)
            {
                var qu = context.Set<TEntity>().FromSql<TEntity>(query, parameters).AsQueryable();

                if (option.FilterBy != null)
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        //edited
                        var q = qu.AsNoTracking().Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        /*For All Records*/
                        if (option.Length == -1)
                        {
                            results.data = q.ToList();
                        }
                        else
                        {
                            results.data = q.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                    else
                    {
                        //edited
                        IOrderedEnumerable<TEntity> tmp = qu.AsNoTracking().Where(option.FilterBy).OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        /*For All Records*/
                        if (option.Length == -1)
                        {
                            results.data = tmp.ToList();
                        }
                        else
                        {
                            results.data = tmp.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                }
                else
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        //edited
                        var q = qu.AsNoTracking().OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        /*For All Records*/
                        if (option.Length == -1)
                        {
                            results.data = q.ToList();
                        }
                        else
                        {
                            results.data = q.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                    else
                    {
                        //edited
                        IOrderedEnumerable<TEntity> tmp = qu.AsNoTracking().OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 1)
                        {
                            for (int i = 1; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        /*For All Records*/
                        if (option.Length == -1)
                        {
                            results.data = tmp.ToList();
                        }
                        else
                        {
                            results.data = tmp.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                }

                results.recordsFiltered = results.recordsTotal;


            }
            return results;
        }


    }
}
