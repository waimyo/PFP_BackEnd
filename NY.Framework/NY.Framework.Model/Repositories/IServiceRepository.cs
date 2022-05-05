using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories
{
   public interface IServiceRepository:IReadWriteRepository<Service,int>
    {
        List<Service> GetService(int deptid);
        List<Service> GetByDeptId(int deptid);
        Service FindByNameAndDept(string name, int deptid);

    }
}
