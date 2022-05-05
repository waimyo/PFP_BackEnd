using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.ChattingFileCommands
{
    public class ChattingFileCreateOrUpdateCommand : Command<ChattingFile, int, ChattingFile, IChattingFileRepository>
    {
        public ChattingFileCreateOrUpdateCommand(IUnitOfWork uom, IChattingFileRepository repo,ChattingFile entity) : base(uom, repo, entity) { }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<ChattingFile> PerformAction(CommandResult<ChattingFile> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            result.Result.Add(entity);
            return result;
        }
    }
}
