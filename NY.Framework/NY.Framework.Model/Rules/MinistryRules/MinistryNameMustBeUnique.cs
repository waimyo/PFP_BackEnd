using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules.MinistryRules
{
    public class MinistryNameMustBeUnique : IBusinessRule
    {
        IMinistryRepository Repo;
        Ministry entity;
        public MinistryNameMustBeUnique(IMinistryRepository _Repo, Ministry _entity)
        {
            this.Repo = _Repo;
            this.entity = _entity;
        }
        public string[] GetRules()
        {
            return new string[] { NY.Framework.Infrastructure.Constants.MinistryNameMustBeUnique };
        }

        public bool IsSatisfied()
        {
            Ministry mini = Repo.FindByName(entity.name);
            if (mini != null)
            {
                if (mini.ID != entity.ID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
