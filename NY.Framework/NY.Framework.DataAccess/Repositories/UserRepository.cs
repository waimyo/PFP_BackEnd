using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Infrastructure.Logging;

namespace NY.Framework.DataAccess.Repositories
{
    public class UserRepository : ReadWriteRepositoryBase<User, int>, IUserRepository
    {

        Logger logger;
        public UserRepository(IDbContext context)
            : base(context,new string[] { "Role", "Role.permissions" })
        {
            logger = new Logger(typeof(UserRepository));
        }

        public List<User> GetUserListByUserNameContainsAndLogInNameContains(System.Linq.Expressions.Expression<Func<User, bool>> expr)
        {            
            return CustomQuery().Where(expr).ToList();
        }

        public User FindByUserName(string userName)
        {
            return CustomQuery().Where(u => u.username == userName).SingleOrDefault();
        }

        public User GetUserByUserNameAndMinistryId(string uname, int minId)
        {
            //return CustomQuery().Where(u => u.Name == uname & u.Ministry_Id == minId).SingleOrDefault();
            return null;
        }

        public User getDefaultUser()
        {
            return null;
            //return CustomQuery().Where(u => u.IsDefault ).SingleOrDefault();
        }

        public User FindById(int id)
        {
            return context.Set<User>().Find(id);
        }
               
        public List<User> FindByUser(int minId)
        {
            //return CustomQuery().Where(u => u.Ministry_Id == minId).ToList();
            return null;
        }

        public User GetUserByUserNameAndPasswordAndMinistryId(string uname, string password)
        {
            try {
                User user = CustomQuery().Where(u => u.username.Equals(uname) && u.status.Equals(true)).FirstOrDefault();
                if (user != null)
                {
                    if (!PasswordHashHelper.ValidatePassword(password, user.password))
                    {
                        user = null;
                    }
                }
                return user;

            }
            catch(Exception ex)
            {
                logger.LogInfo("User Validation****************"+ex.Message);
                logger.Log(ex);
                return null;
            }
           
        }

        //public List<User> GetUserByMinistry(int ministryid)
        //{
        //    return CustomQuery().Where(u => u.ministry_id == ministryid && u.IsDeleted == false && u.role_id==3).ToList();
        //}

        public List<User> GetCPUAccountByMinistry(int ministryid, int roleid)
        {
            //return CustomQuery().Where(u => u.IsDeleted == false && u.ministry_id == ministryid && u.role_id == roleid).ToList();
            return CustomQuery().Where(u => u.IsDeleted == false && u.parent_id == ministryid && u.role_id == roleid).ToList();
        }

        public List<User> GetDEOAccount(int cpu_id)
        {            
            return CustomQuery().Where(u => u.IsDeleted.Equals(false) && u.parent_id.Equals(cpu_id) && u.role_id==4).ToList();
        }

        public List<User> GetMinistryuser()
        {
            return CustomQuery().Where(u => u.IsDeleted.Equals(false) && u.role_id == 2 && u.ministry_id>0).ToList();
        }

        public List<User> GetNoTrainingMinistryuser()
        {
            return CustomQuery().Where(u => u.IsDeleted.Equals(false) && u.role_id == 2 && u.Ministry.istraining == false).ToList();
        }

        public int GetCPUAccountCount(int user_id)
        {
            return CustomQuery().Where(u => u.IsDeleted.Equals(false) && u.parent_id == user_id).Count();
        }
    }
}
