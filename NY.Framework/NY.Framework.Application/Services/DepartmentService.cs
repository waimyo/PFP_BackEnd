using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.DepartmentCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class DepartmentService : ServiceBase<Department, int, IDepartmentRepository>, IDepartmentService
    {
        public DepartmentService(IUnitOfWork uom,IDbContext context, IDepartmentRepository deptRepo)
            : base(typeof(DepartmentService), deptRepo, uom,context)
        {

        }
        public CommandResult<Department> CreateOrUpdate(Department department)
        {
            DepartmentCreateOrUpdateCommand cmd = new DepartmentCreateOrUpdateCommand(uom,repo,department);
            return ExecuteCommand(cmd);
        }

        public CommandResult<Department> Delete(Department department)
        {
            DepartmentDeleteCommand cmd = new DepartmentDeleteCommand(uom, repo, department);
            return ExecuteCommand(cmd);
        }
    }
}
