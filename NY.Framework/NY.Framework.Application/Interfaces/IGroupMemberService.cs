using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface IGroupMemberService : IService<GroupMember, int>
    {
        CommandResult<GroupMember> CreateOrUpdate(GroupMember gpmember);
        CommandResult<GroupMember> Delete(GroupMember gpmember);
        CommandResult<GroupMember> CreateOrUpdateList(List<GroupMember> gpmember);
    }
}
