using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class MinistryRepository: ReadWriteRepositoryBase<Ministry,int>,IMinistryRepository
    {
        public MinistryRepository(IDbContext context):base(context,new string[] { })
        {

        }

        public Ministry FindByName(string name)
        {
            return CustomQuery().Where(x => x.name.Equals(name)).FirstOrDefault();
        }

        public List<Ministry> GetMinistryList()
        {
            return CustomQuery().Where(m => m.IsDeleted.Equals(false)).ToList();
        }
    }
}
