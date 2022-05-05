using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class GroupRepository : ReadWriteRepositoryBase<Groups, int>, IGroupRepository
    {
        public GroupRepository(IDbContext context):
            base(context,new string[] { })
        {

        }
        public Groups FindByGroupName(string name)
        {
            return CustomQuery().Where(g => g.Name.Equals(name) && g.IsDeleted.Equals(false)).FirstOrDefault();
        }
    }
}
