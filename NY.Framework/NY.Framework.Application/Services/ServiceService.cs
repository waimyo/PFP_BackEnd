using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.DepartmentCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using NY.Framework.Model.Commands.ServicesCommands;

namespace NY.Framework.Application.Services
{
   public class ServiceService:ServiceBase<Service,int,IServiceRepository>,IServiceService
    {
        public ServiceService(IUnitOfWork uom, IDbContext context, IServiceRepository serRepo) : base(typeof(ServiceService), serRepo, uom, context) { }

       
        public CommandResult<Service> CreateOrUpdate(Service services)
        {
            ServiceCreateOrUpdateCommand cmd = new ServiceCreateOrUpdateCommand(uom, repo, services);
            return ExecuteCommand(cmd);
        }

        public CommandResult<Service> Delete(Service services)
        {
            ServiceDeleteCommand cmd = new ServiceDeleteCommand(uom, repo, services);
            return ExecuteCommand(cmd);
        }
    }
}
