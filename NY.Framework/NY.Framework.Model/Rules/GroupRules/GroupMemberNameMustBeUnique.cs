using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules.GroupRules
{
    public class GroupMemberNameMustBeUnique : IBusinessRule
    {
        IGroupMemberRepository gpRepo;
        GroupMember group;
        public GroupMemberNameMustBeUnique(IGroupMemberRepository _gpRepo, GroupMember _group)
        {
            this.gpRepo = _gpRepo;
            this.group = _group;
        }

        public bool IsSatisfied()
        {
            GroupMember gp = gpRepo.FindByGroupMemberIdAndGroupId(group.datainfo_id, group.group_id);
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
            string msgText = NY.Framework.Infrastructure.Constants.GroupMemberNameMustBeUnique;
            return new string[] { msgText };
        }


    }
}
