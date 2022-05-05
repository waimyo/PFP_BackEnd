using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
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
    public class GroupService : ServiceBase<Groups, int, IGroupRepository>, IGroupService
    {
        public GroupService(IUnitOfWork uom, IDbContext context,IGroupRepository gpRepo)
            : base(typeof(GroupService), gpRepo, uom, context)
        {

        }
        public CommandResult<Groups> CreateOrUpdate(Groups group)
        {
            GroupCreateOrUpdateCommand cmd = new GroupCreateOrUpdateCommand(uom, repo, group);
            return ExecuteCommand(cmd);
        }

        public CommandResult<Groups> Delete(Groups group)
        {
            GroupDeleteCommand cmd = new GroupDeleteCommand(uom, repo, group);
            return ExecuteCommand(cmd);
        }
    }
}
