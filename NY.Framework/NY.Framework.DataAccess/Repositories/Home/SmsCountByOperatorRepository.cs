using NY.Framework.Model.Entities.Home;
using NY.Framework.Model.Repositories.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories.Home
{
    public class SmsCountByOperatorRepository : ReadWriteRepositoryBase<SmsCountByOperator, int>, ISmsCountByOperatorRepository
    {
        public SmsCountByOperatorRepository(IDbContext context) : base(context, new string[] { }) { }
        public List<SmsCountByOperator> getSmsCount(string fromdate,string todate, int ministry_acc_id, int user_id)
        {
            return RawSQL<SmsCountByOperator>("exec SP_SmsCountByOperator {0},{1},{2},{3}", new object[] { fromdate,todate,ministry_acc_id, user_id }).ToList();
        }
    }
}
