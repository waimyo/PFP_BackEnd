using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules
{
    public class ServiceNameMustBeUnique : IBusinessRule
    {

        IServiceRepository serRepo;
        Service service;
        public ServiceNameMustBeUnique(IServiceRepository serRepo, Service service)
        {
            this.serRepo = serRepo;
            this.service = service;
        }
        public bool IsSatisfied()
        {
            Service s = serRepo.FindByNameAndDept(service.name,service.Dept_id);
            if (s != null)
            {
                if (s.ID != service.ID)
                {
                    return false;
                }
            }
            return true;
        }

        public string[] GetRules()
        {
            return new string[] { Constants.ServiceNameMustBeUnique };
        }
    }
}

   