using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class GroupMemberRepository : ReadWriteRepositoryBase<GroupMember, int>, IGroupMemberRepository
    {
        public GroupMemberRepository(IDbContext context) :
            base(context, new string[] { })
        {

        }

        public GroupMember FindByGroupMemberIdAndGroupId(int dataid, int groupid)
        {
            return CustomQuery().Where(g => g.group_id == groupid && g.datainfo_id == dataid && g.IsDeleted == false).FirstOrDefault();
        }

        public List<GroupMember> GetGroupMembersByGroupId(int groupid)
        {
            return CustomQuery().Where(g=>g.group_id==groupid && g.IsDeleted==false).ToList();
        }
    }
}
