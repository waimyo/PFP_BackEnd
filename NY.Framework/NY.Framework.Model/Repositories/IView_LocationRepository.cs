using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface IView_LocationRepository:IReadWriteRepository<View_Location,int>
    {
        List<View_Location> GetLocaion();
    }
}
