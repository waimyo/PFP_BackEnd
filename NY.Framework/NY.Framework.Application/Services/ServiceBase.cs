using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Logging;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Application.Services
{
    public abstract class ServiceBase<TEntity, TEntityID, TRepository> : IService<TEntity, TEntityID>
        where TEntity : NY.Framework.Infrastructure.Entities.BaseEntity
        where TRepository : IRepository<TEntity, TEntityID>
    {
        protected Logger logger;
        protected IUnitOfWork uom;
        protected IDbContext dbContext;
        protected TRepository repo;

        public ServiceBase(Type type, TRepository repo, IUnitOfWork uom, IDbContext dbContext)
        {
            logger = new Logger(type);
            this.uom = uom;
            this.dbContext = dbContext;
            this.repo = repo;
        }
        
        protected virtual CommandResult<TEntity> ExecuteCommand(ICommand<TEntity> cmd)
        {
            CommandResult<TEntity> result = new CommandResult<TEntity>();
            try
            {
                
                result = cmd.Execute();
            }
            catch (Exception ex)
            {
                Int32 ErrorCode = ((SqlException)ex.InnerException).Number;
                logger.Log(ex);
                result.Success = false;
                /***To Check ForeignKey Constraint Error***/
                result.ErrorCode = ErrorCode;
                result.Messages.Add(ex.Message);                
                 
            }
            return result;
        }
        protected virtual CommandResult<TCustomEntity> ExecuteCommand <TCustomEntity> 
        (ICommand<TCustomEntity> cmd) where TCustomEntity : BaseEntity
        {
            CommandResult<TCustomEntity> result = new CommandResult<TCustomEntity>();
            try
            {

                result = cmd.Execute();
                
            }
            catch (Exception ex)
            {
                Int32 ErrorCode = ((SqlException)ex.InnerException).Number;                
                logger.Log(ex);
                result.Success = false;
                /***Checking, Error is Foreign Key Constratint ?***/
                if (ErrorCode == 547)
                {
                    result.Messages.Add(Constants.ForeignKeyConstraint);
                }
                else
                {
                    result.Messages.Add(ex.Message);
                }

            }
            return result;
        }
    }
}
