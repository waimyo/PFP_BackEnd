using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class DepartmentRepository : ReadWriteRepositoryBase<Department, int>, IDepartmentRepository
    {
        public DepartmentRepository(IDbContext context)
            :base(context,new string[] { })
        {

        }
        

        public Department FindByDepartmentName(string name, int minid)
        {
            return CustomQuery().Where(d => d.Name.Equals(name) && d.Ministry_Id.Equals(minid) && d.IsDeleted.Equals(false)).FirstOrDefault();
        }

        public List<Department> GetDepartment(int ministryid)
        {
            if (ministryid > 0)
            {
                return CustomQuery().Where(m => m.IsDeleted.Equals(false) && m.Ministry_Id.Equals(ministryid)).ToList();
            }
            else
            {
                return CustomQuery().Where(m => m.IsDeleted.Equals(false)).ToList();
            }
        }
    }
}
