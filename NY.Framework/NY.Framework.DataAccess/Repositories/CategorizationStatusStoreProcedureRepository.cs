using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class CategorizationStatusStoreProcedureRepository : ReadWriteRepositoryBase<CategorizationStatusStoreProcedure,int>, ICategorizationStatusStoreProcedureRepository
    {
        public CategorizationStatusStoreProcedureRepository(IDbContext dbContext)
            : base(dbContext, new string[] { })
        {

        }

        public List<CategorizationStatusStoreProcedure> GetCategorizationStatusByCampaignId(int campid)
        {
            return RawSQL<CategorizationStatusStoreProcedure>("exec CategorizationStatusForCampaignDetail {0}", new object[] { campid}).ToList();
        }
    }
}
