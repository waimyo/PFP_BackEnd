using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface IChattingFileService:IService<ChattingFile,int>
    {
        CommandResult<ChattingFile> CreateOrUpdate(ChattingFile entity);
    }
}
