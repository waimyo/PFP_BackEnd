using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface IRoleRepository : IReadWriteRepository<Role, int>
    {
        Role FindByRoleId(Int64 id);
        Role getAnonymousRole();
       // Role getDefaultRole();

        List<Role> GetRoleByIsDefault();
        List<Role> GetRoleByNotDefault();

        List<Role> GetAllRoles();
    }
}
