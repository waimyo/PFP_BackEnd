using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules.DepartmentRules
{
    public class DepartmentNameMustBeUnique:IBusinessRule
    {
        IDepartmentRepository deptRepo;
        Department department;
        public DepartmentNameMustBeUnique(IDepartmentRepository _deptRepo,Department _department)
        {
            this.deptRepo = _deptRepo;
            this.department = _department;
        }

        public bool IsSatisfied()
        {
            Department dept = deptRepo.FindByDepartmentName(department.Name,department.Ministry_Id);
            if(dept != null)
            {
                if(dept.ID != department.ID)
                {
                    return false;
                }
            }
            return true;
        }

        public string[] GetRules()
        {
            string msgText = NY.Framework.Infrastructure.Constants.DepartmentNameMustBeUnique;
            return new string[] { msgText };
        }

        
    }
}
