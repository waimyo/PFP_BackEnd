using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules.GroupRules
{
    public class GroupNameMustBeUnique : IBusinessRule
    {
        IGroupRepository gpRepo;
        Groups group;
        public GroupNameMustBeUnique(IGroupRepository _gpRepo, Groups _group)
        {
            this.gpRepo = _gpRepo;
            this.group = _group;
        }

        public bool IsSatisfied()
        {
            Groups gp = gpRepo.FindByGroupName(group.Name);
            if (gp != null)
            {
                if (gp.ID != group.ID)
                {
                    return false;
                }
            }
            return true;
        }

        public string[] GetRules()
        {
            string msgText = NY.Framework.Infrastructure.Constants.GroupNameMustBeUnique;
            return new string[] { msgText };
        }


    }
}
