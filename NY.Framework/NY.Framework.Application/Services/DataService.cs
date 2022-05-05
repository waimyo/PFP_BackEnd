using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.DataCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class DataService : ServiceBase<Data, int, IDataRepository>, IDataService
    {
        public DataService(IUnitOfWork uom, IDbContext context, IDataRepository Repo)
            : base(typeof(DataService), Repo, uom, context) { }

        public CommandResult<Data> CreateOrUpdate(Data entity)
        {
            DataCreateOrUpdateCommand cmd = new DataCreateOrUpdateCommand(uom, repo, entity);
            return ExecuteCommand(cmd);
        }

        public CommandResult<Data> Delete(Data entity)
        {
            DataDeleteCommand cmd = new DataDeleteCommand(uom, repo, entity);
            return ExecuteCommand(cmd);
        }
    }
}
