using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.ChattingCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;

namespace NY.Framework.Application.Services
{
    public class ChattingService:ServiceBase<Chatting,int, IChattingRepository>, IChattingService
    {
        public ChattingService(IChattingRepository repo,IUnitOfWork uom,IDbContext context) : base(typeof(ChattingService), repo, uom, context) { }

        public CommandResult<Chatting> CreateOrUpdateCommand(Chatting campaign)
        {
            ChattingCreateOrUpdateCommand cmd = new ChattingCreateOrUpdateCommand(uom, repo, campaign);
            return ExecuteCommand(cmd);
        }
    }
}
