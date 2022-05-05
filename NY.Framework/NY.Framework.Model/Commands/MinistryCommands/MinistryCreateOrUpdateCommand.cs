using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Model.Rules.MinistryRules;
using NY.Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.MinistryCommands
{
    public class MinistryCreateOrUpdateCommand : Command<Ministry, int, Ministry, IMinistryRepository>
    {        
        public MinistryCreateOrUpdateCommand(IUnitOfWork uom, IMinistryRepository Repo, Ministry entity)
        : base(uom, Repo, entity) { }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new MinistryNameMustBeUnique(repo, entity));
            return rules;
        }

        protected override CommandResult<Ministry> PerformAction(CommandResult<Ministry> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
