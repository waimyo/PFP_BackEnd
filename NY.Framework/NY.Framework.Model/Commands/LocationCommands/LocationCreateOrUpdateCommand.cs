using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using NY.Framework.Model.Rules.LocationRules;

namespace NY.Framework.Model.Commands.LocationCommands
{
    public class LocationCreateOrUpdateCommand : Command<Location, int, Location, ILocationRepository>
    {
        public LocationCreateOrUpdateCommand(IUnitOfWork uom,ILocationRepository locationRepo,
            Location location) 
        : base(uom,locationRepo,location)
        {

        }


        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new LocationNameMustBeUnique(repo,entity));
            rules.Add(new LocationCodeMustBeUnique(repo,entity));
            return rules;
        }

        protected override CommandResult<Location> PerformAction(CommandResult<Location> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            result.Result.Add(entity);
            return result;
        }
    }
}
