using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class SP_GroupMemberListRepository : ReadWriteRepositoryBase<SP_GroupMemberList, int>, ISP_GroupMemberListRepository
    {
        public SP_GroupMemberListRepository(IDbContext context) :
            base(context, new string[] { })
        {

        }

    }
}
