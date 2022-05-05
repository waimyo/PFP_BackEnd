using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class ResponseStatusRepository : ReadWriteRepositoryBase<ResponseStatus,int>,IResponseStatusRepository
    {
        public ResponseStatusRepository(IDbContext context) : base(context, new string[] { }) { }

        public List<ResponseStatus> getResponseStats(int ministry_id, string fromdate, string todate)
        {
            return RawSQL<ResponseStatus>("exec SP_Response_Status {0},{1},{2}", new object[] { ministry_id, fromdate, todate }).ToList();
        }
    }
}
