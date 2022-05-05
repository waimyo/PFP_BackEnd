using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class View_DataInfoRepository:ReadWriteRepositoryBase<View_DataInfo,int>, IView_DataInfoRepository
    {
        public View_DataInfoRepository(IDbContext context):base(context,new string[] { }) { }
    }
}
