using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Commands
{
    public abstract class Command<TEntity, TEntityID, TRepoEntity, TRepository> : ICommand<TEntity>
        where TEntity : BaseEntity
        where TRepoEntity : BaseEntity
        where TRepository : IRepository<TRepoEntity, TEntityID>
    {
        CommandResult<TEntity> result;
        IUnitOfWork uom;
        protected TEntity entity;
        protected TRepository repo;
        bool commit = true;

        public Command(IUnitOfWork uom, TRepository repo, TEntity entity, bool commit = true)
        {
            this.uom = uom;
            this.entity = entity;
            this.repo = repo;
            result = new CommandResult<TEntity>();
            this.commit = commit;
        }
        public CommandResult<TEntity> Execute()
        {
            ValidateRules();

            if (result.Success)
            {
                result = PerformAction(result);
                if (result.Success)
                {
                    if(commit)
                    {
                        uom.Commit();
                    }
                    
                }
            }

            return result;
        }

        protected abstract CommandResult<TEntity> PerformAction(CommandResult<TEntity> result);

        void ValidateRules()
        {
            bool valid = true;
            List<IBusinessRule> rulelst= GetRules();
            if (rulelst!=null)
            {
                foreach (IBusinessRule rule in rulelst)
                {
                    if (!rule.IsSatisfied())
                    {
                        result.Success = false;
                        valid = false;
                        result.Messages.AddRange(rule.GetRules());
                        break;
                    }
                }
            }

            if (valid)
            {
                result.Success = true;
            }
        }

        protected abstract List<IBusinessRule> GetRules();
    }

    
}
