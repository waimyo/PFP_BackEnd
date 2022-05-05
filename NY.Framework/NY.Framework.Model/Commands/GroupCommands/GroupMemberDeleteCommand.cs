using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.GroupCommands
{
    public class GroupMemberDeleteCommand : Command<GroupMember, int, GroupMember, IGroupMemberRepository>
    {
        public GroupMemberDeleteCommand(IUnitOfWork uom, IGroupMemberRepository gpmemberRepo, GroupMember groupmember)
            : base(uom, gpmemberRepo, groupmember)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<GroupMember> PerformAction(CommandResult<GroupMember> result)
        {
            repo.Remove(entity);
            result.Success = true;
            result.Messages.Add(NY.Framework.Infrastructure.Constants.DELETE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
