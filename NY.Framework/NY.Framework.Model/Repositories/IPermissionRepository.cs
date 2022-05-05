using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface IPermissionRepository : IReadWriteRepository<Permission, int>
    {
        Permission GetByRoleId(int roleId);
        List<Permission> GetPermissions(int rid);
        List<Permission> GetSubMenuByProgramId(int rid, string pcode);
    }
}
