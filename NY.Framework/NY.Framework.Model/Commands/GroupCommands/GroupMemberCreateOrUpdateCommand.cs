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
    public class GroupMemberCreateOrUpdateCommand : Command<GroupMember, int, GroupMember, IGroupMemberRepository>
    {
        public GroupMemberCreateOrUpdateCommand(IUnitOfWork uom, IGroupMemberRepository gpmemberRepo, GroupMember groupmember)
            : base(uom, gpmemberRepo, groupmember)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new GroupMemberNameMustBeUnique(repo, entity));
            return rules;
        }

        protected override CommandResult<GroupMember> PerformAction(CommandResult<GroupMember> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(NY.Framework.Infrastructure.Constants.SAVE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
