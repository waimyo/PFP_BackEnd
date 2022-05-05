using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.GroupCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class GroupMemberService : ServiceBase<GroupMember, int, IGroupMemberRepository>, IGroupMemberService
    {
        public GroupMemberService(IUnitOfWork uom, IDbContext context, IGroupMemberRepository gpmemberRepo)
            : base(typeof(GroupMemberService), gpmemberRepo, uom, context)
        {

        }
        public CommandResult<GroupMember> CreateOrUpdate(GroupMember gpmember)
        {
            GroupMemberCreateOrUpdateCommand cmd = new GroupMemberCreateOrUpdateCommand(uom, repo, gpmember);
            return ExecuteCommand(cmd);
        }

        public CommandResult<GroupMember> CreateOrUpdateList(List<GroupMember> gpmember)
        {
            CommandResult<GroupMember> result = new CommandResult<GroupMember>();
            repo.Save(gpmember);
            uom.Commit();
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            return result;
        }

        public CommandResult<GroupMember> Delete(GroupMember gpmember)
        {
            GroupMemberDeleteCommand cmd = new GroupMemberDeleteCommand(uom, repo, gpmember);
            return ExecuteCommand(cmd);
        }
    }
}
