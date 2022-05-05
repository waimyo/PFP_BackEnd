using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Model.Rules.UserRules
{
    public class UserNameMustBeUnique : IBusinessRule
    {
        
        IUserRepository userRepo;
        User user;
        public UserNameMustBeUnique(IUserRepository userRepo,User user)
        {
            this.userRepo = userRepo;
            this.user = user;
        }
        public bool IsSatisfied()
        {
            User u = userRepo.FindByUserName(user.username);
            if (u != null)
            {
                if (u.ID != user.ID)
                {
                    return false;
                }
            }
            return true;
        }

        public string[] GetRules()
        {
            return new string[] { Constants.UserNameMustBeUnique };
        }
    }
}
