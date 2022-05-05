﻿using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface IChattingService : IService<Chatting,int>
    {
        CommandResult<Chatting> CreateOrUpdateCommand(Chatting inout);
    }
}
