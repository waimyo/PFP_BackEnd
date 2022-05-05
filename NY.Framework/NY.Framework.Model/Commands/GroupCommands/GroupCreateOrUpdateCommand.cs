using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Model.Rules.GroupRules;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.GroupCommands
{
    public class GroupCreateOrUpdateCommand : Command<Groups, int, Groups, IGroupRepository>
    {
        public GroupCreateOrUpdateCommand(IUnitOfWork uom, IGroupRepository gpRepo, Groups group)
            : base(uom, gpRepo, group)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new GroupNameMustBeUnique(repo, entity));
            return rules;
        }

        protected override CommandResult<Groups> PerformAction(CommandResult<Groups> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(NY.Framework.Infrastructure.Constants.SAVE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
