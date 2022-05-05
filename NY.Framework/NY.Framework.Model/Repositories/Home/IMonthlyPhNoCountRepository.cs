using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Model.Repositories.Home
{
    public interface IMonthlyPhNoCountRepository  : IRepository<MonthlyPhnoCount, int>
    {
        List<MonthlyPhnoCount> getMonthlyPhnoCount(string fromdate,string todate,int ministry_id,int user_id);
    }
}
