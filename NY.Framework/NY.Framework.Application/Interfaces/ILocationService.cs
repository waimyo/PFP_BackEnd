using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface ILocationService : IService<Location,int>
    {
        CommandResult<Location> CreateOrUpdate(Location loc);
        CommandResult<Location> Delete(Location loc);
    }
}
