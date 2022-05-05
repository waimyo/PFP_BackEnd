using NY.Framework.Model.Entities.Home;
using NY.Framework.Model.Repositories.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories.Home
{
    public class MonthlyPhNoCountRepository : ReadWriteRepositoryBase<MonthlyPhnoCount, int>, IMonthlyPhNoCountRepository
    {
        public MonthlyPhNoCountRepository(IDbContext context) : base(context, new string[] { }) { }

        public List<MonthlyPhnoCount> getMonthlyPhnoCount(string fromdate,string todate,int ministry_id, int user_id)
        {
            return RawSQL<MonthlyPhnoCount>("exec SP_Monthly_Phno_Count {0},{1},{2},{3}", new object[] { fromdate,todate,ministry_id,user_id}).ToList();
           
        }
    }
}
