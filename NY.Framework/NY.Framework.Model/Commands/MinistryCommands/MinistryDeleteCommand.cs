using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Infrastructure;
using System.Collections.Generic;

namespace NY.Framework.Model.Commands.MinistryCommands
{
    public class MinistryDeleteCommand : Command<Ministry, int, Ministry, IMinistryRepository>
    {
        public MinistryDeleteCommand(IUnitOfWork uom, IMinistryRepository Repo, Ministry entity)
            : base(uom, Repo, entity) { }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<Ministry> PerformAction(CommandResult<Ministry> result)
        {
            repo.Remove(entity);
            result.Success = true;
            result.Messages.Add(Constants.DELETE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
