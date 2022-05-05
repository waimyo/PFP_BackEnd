using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.GroupCommands
{
    public class GroupDeleteCommand : Command<Groups, int, Groups, IGroupRepository>
    {
        public GroupDeleteCommand(IUnitOfWork uom, IGroupRepository gpRepo, Groups group)
            : base(uom, gpRepo, group)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<Groups> PerformAction(CommandResult<Groups> result)
        {
            repo.Remove(entity);
            result.Success = true;
            result.Messages.Add(NY.Framework.Infrastructure.Constants.DELETE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
