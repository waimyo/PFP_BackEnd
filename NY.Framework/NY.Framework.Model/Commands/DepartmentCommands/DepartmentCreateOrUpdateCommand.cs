using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Model.Rules.DepartmentRules;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.DepartmentCommands
{
    public class DepartmentCreateOrUpdateCommand:Command<Department,int,Department, IDepartmentRepository>
    {
        public DepartmentCreateOrUpdateCommand(IUnitOfWork uom,IDepartmentRepository deptRepo,Department department)
            : base(uom, deptRepo, department)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new DepartmentNameMustBeUnique(repo, entity));
            return rules;
        }

        protected override CommandResult<Department> PerformAction(CommandResult<Department> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(NY.Framework.Infrastructure.Constants.SAVE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
