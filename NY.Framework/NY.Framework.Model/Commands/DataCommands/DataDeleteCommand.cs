using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.DataCommands
{
    public class DataDeleteCommand : Command<Data, int, Data, IDataRepository>
    {
        public DataDeleteCommand(IUnitOfWork uom, IDataRepository Repo, Data entity)
            : base(uom, Repo, entity) { }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<Data> PerformAction(CommandResult<Data> result)
        {
            repo.Remove(entity);
            result.Success = true;
            result.Messages.Add(Constants.DELETE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
