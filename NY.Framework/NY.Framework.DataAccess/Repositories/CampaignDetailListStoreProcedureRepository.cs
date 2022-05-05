using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class CampaignDetailListStoreProcedureRepository : ReadWriteRepositoryBase<CampaignDetailListStoreProcedure, int>, ICampaignDetailListStoreProcedureRepository
    {
        public CampaignDetailListStoreProcedureRepository(IDbContext context)
            : base(context, new string[] { })
        {

        }
      
    }
}
