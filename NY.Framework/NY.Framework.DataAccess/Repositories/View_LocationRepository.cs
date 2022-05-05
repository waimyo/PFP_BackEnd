using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class View_LocationRepository:ReadWriteRepositoryBase<View_Location,int>,IView_LocationRepository
    {
        public View_LocationRepository(IDbContext context)
            :base(context,new string[] { })
        {

        }

        public List<View_Location> GetLocaion()
        {
            return CustomQuery().ToList();
        }
    }
}
