using NY.Framework.Application.Interfaces;
using System;
//using NY.Framework.Infrastructure.Entities;
//using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Commands.AuditCommands;
using NY.Framework.Model.Repositories;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Repositories;

namespace NY.Framework.Application.Services
{
    public class AuditService : ServiceBase<Model.Entities.Audit, int, Model.Repositories.IAuditRepository>, IAuditService
    {
        public AuditService(Infrastructure.Repositories.IUnitOfWork uom, IDbContext context, Model.Repositories.IAuditRepository aRepo)
            : base(typeof(AuditService), aRepo, uom, context)
        {

        }

        public CommandResult<Audit> Create(Audit audit)
        {
            AuditCreateCommand cmd = new AuditCreateCommand(uom, repo, audit);
            return ExecuteCommand(cmd);
        }
    }


}
