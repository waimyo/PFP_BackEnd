using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.ChattingCommands
{
    public class ChattingCreateOrUpdateCommand : Command<Chatting, int, Chatting, IChattingRepository>
    {
        public ChattingCreateOrUpdateCommand(IUnitOfWork uom, IChattingRepository Repo,
            Chatting entity): base(uom, Repo, entity) { }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<Chatting> PerformAction(CommandResult<Chatting> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add("စာကိုပေးပို့ပြီးပါပြီ။");
            result.Result.Add(entity);
            return result;
        }
    }
}
