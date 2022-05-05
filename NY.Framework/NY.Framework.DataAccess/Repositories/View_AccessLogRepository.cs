using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class View_AccessLogRepository : ReadWriteRepositoryBase<View_AccessLog, int>, IView_AccessLogRepository
    {
        public View_AccessLogRepository(IDbContext context) :
            base(context, new string[] { })
        {

        }
    }
}
