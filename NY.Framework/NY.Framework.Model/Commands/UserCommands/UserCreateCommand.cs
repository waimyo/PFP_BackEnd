using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Model.Rules.UserRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Model.Commands.UserCommands
{
    public class UserCreateCommand : Command<User, int, User, IUserRepository>
    {
        public UserCreateCommand(IUnitOfWork uom, IUserRepository userRepo, User user) 
            :base(uom, userRepo, user)
        {           
        }

        protected override CommandResult<User> PerformAction(CommandResult<User> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            result.Result.Add(entity);
            return result;
        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new UserNameMustBeUnique(repo,entity));
            return rules;
        }
    }
}
