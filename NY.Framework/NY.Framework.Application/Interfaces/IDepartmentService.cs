using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface IDepartmentService:IService<Department,int>
    {
        CommandResult<Department> CreateOrUpdate(Department department);
        CommandResult<Department> Delete(Department department);
    }
}
