using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.LocationCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class LocationService : ServiceBase<Location,int,ILocationRepository>,ILocationService
    {
        public LocationService(ILocationRepository locRepo,IUnitOfWork uom,IDbContext context) : 
            base(typeof(LocationService),locRepo,uom,context)
        {

        }

        public CommandResult<Location> CreateOrUpdate(Location loc)
        {
            LocationCreateOrUpdateCommand cmd = new LocationCreateOrUpdateCommand(uom,repo,loc);
            return ExecuteCommand(cmd);
        }

        public CommandResult<Location> Delete(Location loc)
        {
            LocationDeleteCommand cmd = new LocationDeleteCommand(uom,repo,loc);
            return ExecuteCommand(cmd);
        }
    }
}
