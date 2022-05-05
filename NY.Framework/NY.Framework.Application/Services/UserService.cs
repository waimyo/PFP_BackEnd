using NY.Framework.Application.Interfaces;
using System.Collections.Generic;
using NY.Framework.Model.Repositories;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Commands.UserCommands;

namespace NY.Framework.Application.Services
{
    
    public class UserService : ServiceBase<User, int, IUserRepository>, IUserService
    {
        public UserService(IUnitOfWork uom, IDbContext dbContext, IUserRepository userRepo)
            :base(typeof(UserService), userRepo, uom, dbContext) { }
        
        public User FindByUserNameAndPassword(string userName, string password)
        {
            User user = repo.FindByUserName(userName);

            if(user != null)
            {
                if(!NY.Framework.Infrastructure.Utilities.PasswordHashHelper.ValidatePassword(password, user.password))
                {
                    user = null;
                }                
            }
            return user;
        }

        public CommandResult<User> CreateOrUpdate(User user)
        {
            UserCreateCommand cmd = new UserCreateCommand(uom,repo,user);
            return ExecuteCommand(cmd);
        }

        public CommandResult<User> Delete(User user)
        {
            UserDeleteCommand cmd = new UserDeleteCommand(uom,repo,user);
            return ExecuteCommand(cmd);
        }
    }
}
