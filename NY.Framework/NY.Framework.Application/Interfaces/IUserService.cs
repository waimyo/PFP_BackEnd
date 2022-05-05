using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Model;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Application.Interfaces
{
    public interface IUserService : IService<User, int>
    {
        User FindByUserNameAndPassword(string userName, string password);
        CommandResult<User> CreateOrUpdate(User user);     
        CommandResult<User> Delete(User user);
        
    }
}
