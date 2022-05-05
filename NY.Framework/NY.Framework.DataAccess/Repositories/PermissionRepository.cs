using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class PermissionRepository : ReadWriteRepositoryBase<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(IDbContext context) : base(context, new string[] { }) { }

        public Permission GetByRoleId(int roleId)
        {
            return CustomQuery().Where(p => p.role_id.Equals(roleId)).FirstOrDefault();
        }

        public List<Permission> GetPermissions(int rid)
        {
            return CustomQuery().Where(p => p.role_id.Equals(rid) && p.IsDeleted == false).ToList();
        }

        public List<Permission> GetSubMenuByProgramId(int rid, string pcode)
        {
            return CustomQuery().Where(p => p.role_id.Equals(rid) && p.program.program.name.Equals(pcode) && p.program.status.Equals(false)).ToList();
        }
    }
}
