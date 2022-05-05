using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories.Home
{
    public interface ISmsCountByOperatorRepository : IRepository<SmsCountByOperator, int>
    {
        List<SmsCountByOperator> getSmsCount(string fromdate,string todate,int ministry_acc_id, int user_id);
    }
}
