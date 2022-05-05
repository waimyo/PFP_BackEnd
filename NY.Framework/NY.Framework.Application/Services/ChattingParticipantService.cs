using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.ChattingParticipantCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class ChattingParticipantService:ServiceBase<ChattingParticipant,int, IChattingParticipantRepository>, IChattingParticipantService
    {
        public ChattingParticipantService(IChattingParticipantRepository repo,IUnitOfWork uom, IDbContext context) : base(typeof(ChattingParticipantService), repo, uom, context) { }

        public CommandResult<ChattingParticipant> CreateOrUpdate(ChattingParticipant entity)
        {
            ChattingParticipantCreateOrUpdateCommand cmd = new ChattingParticipantCreateOrUpdateCommand(uom, repo, entity);
            return ExecuteCommand(cmd);
        }
    }
}
