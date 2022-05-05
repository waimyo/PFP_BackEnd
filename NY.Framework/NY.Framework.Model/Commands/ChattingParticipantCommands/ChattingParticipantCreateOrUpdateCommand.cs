using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.ChattingParticipantCommands
{
    public class ChattingParticipantCreateOrUpdateCommand : Command<ChattingParticipant, int, ChattingParticipant, IChattingParticipantRepository>
    {
        public ChattingParticipantCreateOrUpdateCommand(IUnitOfWork uom,IChattingParticipantRepository repo,ChattingParticipant entity) : base(uom, repo, entity) { }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<ChattingParticipant> PerformAction(CommandResult<ChattingParticipant> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            result.Result.Add(entity);
            return result;
        }
    }
}
