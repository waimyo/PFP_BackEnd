using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using NY.Framework.Infrastructure;
using System.Text;
using NY.Framework.Model.Rules;

namespace NY.Framework.Model.Commands.DepartmentCommands
{
    public class ServiceCreateOrUpdateCommand: Command<Service, int, Service, IServiceRepository>
    {
        public ServiceCreateOrUpdateCommand(IUnitOfWork uom, IServiceRepository repo, Service services) : base(uom, repo, services)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new ServiceNameMustBeUnique(repo, entity));
            return rules;
        }

        protected override CommandResult<Service> PerformAction(CommandResult<Service> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            result.Result.Add(entity);
            return result;
        }
    }
}

  
