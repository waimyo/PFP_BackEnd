using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.ChattingFileCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class ChattingFileService:ServiceBase<ChattingFile,int, IChattingFileRepository>, IChattingFileService
    {
        public ChattingFileService(IChattingFileRepository repo,IUnitOfWork uom, IDbContext context) : base(typeof(ChattingFileService),repo, uom, context) { }

        public CommandResult<ChattingFile> CreateOrUpdate(ChattingFile entity)
        {
            ChattingFileCreateOrUpdateCommand cmd = new ChattingFileCreateOrUpdateCommand(uom, repo, entity);
            return ExecuteCommand(cmd);
        }
    }
}
