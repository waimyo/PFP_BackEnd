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

namespace NY.Framework.DataAccess
{
    public class ReadWriteRepositoryBase<TEntity, TEntityID> : RepositoryBase<TEntity, TEntityID>, IReadWriteRepository<TEntity, TEntityID> where TEntity : Entity<TEntityID>, new()
    {
        public ReadWriteRepositoryBase(IDbContext context, string[] includes) 
            : base(context, includes)
        {
            

        }
        
        public void Save(TEntity entity)
        {
            

            if (EqualityComparer<TEntityID>.Default.Equals(entity.ID, default(TEntityID)))
            {
                context.Set<TEntity>().Add(entity);


            }
            else
            {
                context.Set<TEntity>().Attach(entity);

                context.SetModifedState<TEntity>(entity);

            }
        }

        public void Save(List<TEntity> entities)
        {
            foreach (TEntity ent in entities)
            {
                Save(ent);

            }
        }

        public void Remove(TEntity entity)
        {
            if (entity is IDefaultEntity)
            {
               if( ((IDefaultEntity)entity).IsDefault)
                {
                    throw new CannotDeleteDefaultEntityException();
                }
               else
                {
                    if (entity is ISoftDeleteEntity)
                    {
                        ((ISoftDeleteEntity)entity).IsDeleted = true;
                        if (entity is IAuditableEntity)
                        {
                            ((IAuditableEntity)entity).ModifiedDate = DateTime.Now;
                        }
                        context.Set<TEntity>().Attach(entity);
                        context.SetModifedState<TEntity>(entity);
                    }
                    else
                    {
                        context.SetDeletedState<TEntity>(entity);
                    }
                }
               
            }
            else
            {
                if (entity is ISoftDeleteEntity)
                {
                    ((ISoftDeleteEntity)entity).IsDeleted = true;
                    if (entity is IAuditableEntity)
                    {
                        ((IAuditableEntity)entity).ModifiedDate = DateTime.Now;
                    }
                    context.Set<TEntity>().Attach(entity);
                    context.SetModifedState<TEntity>(entity);
                }
                else
                {
                    context.SetDeletedState<TEntity>(entity);
                }
            }

        }

        public void Remove(List<TEntity> entities)
        {
            foreach (TEntity ent in entities)
            {
                Remove(ent);
            }
        }

       
        protected DbSet<TEntity> GetDBSet()
        {           
            return (DbSet<TEntity>)context.Set<TEntity>();
        }
        
        protected int RawSQL(string sql, object[] parameters)
        {
            return context.GetDatabase().ExecuteSqlCommand(sql, parameters);
        }      

    }






}
