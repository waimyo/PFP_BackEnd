using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;

using System.Text;

namespace NY.Framework.Model.Commands.ServicesCommands
{
   public class ServiceDeleteCommand: Command<Service, int, Service, IServiceRepository>
    {
        public ServiceDeleteCommand(IUnitOfWork uom, IServiceRepository catRepo, Service services)
            : base(uom, catRepo, services) { }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<Service> PerformAction(CommandResult<Service> result)
        {
            repo.Remove(entity);
            result.Success = true;
            result.Messages.Add(NY.Framework.Infrastructure.Constants.DELETE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
   
