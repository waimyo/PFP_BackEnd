using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface ICategorizationStatusRepository : IRepository<CategorizationStatus, int>
    {
        List<CategorizationStatus> getCategorization(int ministry_id, string fromdate,string todate);
    }
}
