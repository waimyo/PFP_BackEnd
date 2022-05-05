using NY.Framework.Model.Entities.Home;
using NY.Framework.Model.Repositories.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories.Home
{
    public class TotalCorruptionCountsRepository:ReadWriteRepositoryBase<TotalCorruptionCounts,int>,ITotalCorruptionCountsRepository
    {
        public TotalCorruptionCountsRepository(IDbContext context):base(context,new string[] { })
        {

        }

        public TotalCorruptionCounts GetCorruptionCounts(string fromdate, string todate,int ministry_id,int user_id)
        {
            return RawSQL<TotalCorruptionCounts>("exec SP_TotalCorruptionCounts {0},{1},{2},{3}", new object[] { fromdate, todate,ministry_id,user_id }).FirstOrDefault();
        }
    }
}
