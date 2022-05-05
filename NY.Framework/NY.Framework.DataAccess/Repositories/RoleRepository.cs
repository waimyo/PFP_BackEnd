using NY.Framework.Infrastructure;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class RoleRepository : ReadWriteRepositoryBase<Role, int>, IRoleRepository
    {
        public RoleRepository(IDbContext context)
     : base(context, new string[] { "" }) { }


        //public Role getDefaultRole()
        //{
        //    return CustomQuery().Where(r => r.IsDeleted.Equals(false) && r.name != Constants.SYSTEM_ADMINISTRATOR_ROLE && r.is_default).SingleOrDefault();
        //}

        public Role getAnonymousRole()
        {
            //return CustomQuery().Where(r => r.IsDeleted.Equals(false) && r.name == Constants.ANONYMOUS_ROLE && r.is_default).SingleOrDefault();
            return CustomQuery().Where(r => r.IsDeleted.Equals(false) && r.name == Constants.ANONYMOUS_ROLE).SingleOrDefault();
        }

        public Role FindByRoleId(long id)
        {
            return CustomQuery().Where(r => r.IsDeleted.Equals(false) && r.ID == id).SingleOrDefault();
        }

        public List<Role> GetRoleByIsDefault()
        {
            return CustomQuery().Where(r => r.IsDeleted.Equals(false) && r.ID != 4).ToList();
        }

        public List<Role> GetRoleByNotDefault()
        {
            //return CustomQuery().Where(r => r.IsDeleted.Equals(false) && r.ID == 4).ToList();
            return CustomQuery().Where(r => r.IsDeleted.Equals(false) && r.isdefault.Equals(false)&&r.ID!=5).ToList();
        }

        public List<Role> GetAllRoles()
        {
            return CustomQuery().Where(r => r.IsDeleted.Equals(false)).ToList();
        }
    }
}
