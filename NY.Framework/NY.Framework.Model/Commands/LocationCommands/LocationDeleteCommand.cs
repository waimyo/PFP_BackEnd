using System.Collections.Generic;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Infrastructure.Repositories;

namespace NY.Framework.Model.Commands.LocationCommands
{
    public class LocationDeleteCommand : Command<Location, int, Location, ILocationRepository>
    {

        public LocationDeleteCommand(IUnitOfWork uom,ILocationRepository locRepo,Location loc)
            :base(uom,locRepo,loc)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<Location> PerformAction(CommandResult<Location> result)
        {
            repo.Remove(entity);
            result.Success = true;
            result.Messages.Add(Constants.DELETE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
