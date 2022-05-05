using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class ServiceRepository : ReadWriteRepositoryBase<Service, int>, IServiceRepository
    {
        public ServiceRepository(IDbContext context) : base(context, new string[] { })
        {

        }

        public Service FindByNameAndDept(string name, int deptid)
        {
            return CustomQuery().Where(x => x.name.Equals(name) && x.Dept_id==deptid).FirstOrDefault();
        }

        public List<Service> GetByDeptId(int deptid)
        {
            return CustomQuery().Where(s => s.Dept_id.Equals(deptid)).ToList();
        }



        public List<Service> GetService(int deptid)
        {
            //if (deptid > 0)
            //{
                return CustomQuery().Where(x => x.IsDeleted.Equals(false) && x.Dept_id.Equals(deptid)).ToList();
            //}
            //else
            //{
            //    return CustomQuery().Where(x => x.IsDeleted.Equals(false)).ToList();
            //}

        }
    }
}
