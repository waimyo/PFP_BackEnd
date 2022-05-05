using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Rules.UserRules;
using NY.Framework.Infrastructure;
using NY.Framework.Model.Entities;

namespace NY.Framework.Model.Commands.UserCommands
{
    public class UserDeleteCommand : Command<User, int, User, IUserRepository>
    {
        public UserDeleteCommand(IUnitOfWork uom, IUserRepository repo, User entity) :
            base(uom, repo, entity)
        {
        }

        protected override List<IBusinessRule> GetRules()
        {           
            return null;
        }

        protected override CommandResult<User> PerformAction(CommandResult<User> result)
        {
            repo.Remove(entity);
            result.Success = true;
            result.Messages.Add(Constants.DELETE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
