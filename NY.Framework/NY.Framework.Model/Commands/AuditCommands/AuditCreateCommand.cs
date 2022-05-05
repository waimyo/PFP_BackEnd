using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.AuditCommands
{
    public class AuditCreateCommand : Command<Audit, int, Audit, Repositories.IAuditRepository>
    {
        public AuditCreateCommand(IUnitOfWork uom, Repositories.IAuditRepository aRepo, Audit audit)
            : base(uom, aRepo, audit)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            return rules;
        }

        protected override CommandResult<Audit> PerformAction(CommandResult<Audit> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(NY.Framework.Infrastructure.Constants.SAVE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
