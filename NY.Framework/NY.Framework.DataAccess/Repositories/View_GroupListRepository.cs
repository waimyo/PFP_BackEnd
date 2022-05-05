using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class View_GroupListRepository : ReadWriteRepositoryBase<View_GroupList, int>, IView_GroupListRepository
    {
        public View_GroupListRepository(IDbContext context) :
            base(context, new string[] { })
        {

        }
       
    }
}
