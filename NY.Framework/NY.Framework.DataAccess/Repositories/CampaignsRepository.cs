using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class CampaignsRepository : ReadWriteRepositoryBase<Campaigns, int>, ICampaignsRepository
    {
        public CampaignsRepository(IDbContext context)
            :base(context,new string[] { })
        {

        }

        public Campaigns FindByName(string name)
        {            
            return CustomQuery().Where(c => c.Name.Equals(name)).FirstOrDefault();
        }
    }
}
