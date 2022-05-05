using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class CategorizationStatusRepository : ReadWriteRepositoryBase<CategorizationStatus, int>, ICategorizationStatusRepository
    {
        public CategorizationStatusRepository(IDbContext context) : base(context, new string[] { }) { }

        public List<CategorizationStatus> getCategorization(int ministry_id, string fromdate, string todate)
        {
            return RawSQL<CategorizationStatus>("exec SP_Categorization_Status {0},{1},{2}", new object[] { ministry_id, fromdate,todate }).ToList();
        }
    }
}
