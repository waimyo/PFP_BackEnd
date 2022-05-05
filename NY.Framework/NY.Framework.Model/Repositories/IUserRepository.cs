using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Model.Repositories
{
    public interface IUserRepository : IReadWriteRepository<User, int>
    {
        User FindById(int id);
        User FindByUserName(string userName);
        User getDefaultUser();      
        User GetUserByUserNameAndMinistryId(string uname,int minId);
        User GetUserByUserNameAndPasswordAndMinistryId(string uname, string password);
        //List<User> GetUserByMinistry(int ministryid);
        /* user group */
        List<User> FindByUser(int minId);
        List<User> GetCPUAccountByMinistry(int ministryid, int roleid);
        List<User> GetDEOAccount(int cpu_id);

        // all ministry account list
        List<User> GetMinistryuser();
        // all ministry account list except training ministry
        List<User> GetNoTrainingMinistryuser();
        int GetCPUAccountCount(int minacc_id);

    }
}
