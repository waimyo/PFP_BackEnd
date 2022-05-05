using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.MinistryCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class MinistryService: ServiceBase<Ministry, int,IMinistryRepository>, IMinistryService
    {
        public MinistryService(IUnitOfWork uom, IDbContext context, IMinistryRepository Repo)
            : base(typeof(MinistryService), Repo, uom, context) { }

        public CommandResult<Ministry> CreateOrUpdate(Ministry entity)
        {
            MinistryCreateOrUpdateCommand cmd = new MinistryCreateOrUpdateCommand(uom, repo, entity);
            return ExecuteCommand(cmd);
        }

        public CommandResult<Ministry> Delete(Ministry entity)
        {
            MinistryDeleteCommand cmd = new MinistryDeleteCommand(uom, repo, entity);
            return ExecuteCommand(cmd);
        }
    }
}
