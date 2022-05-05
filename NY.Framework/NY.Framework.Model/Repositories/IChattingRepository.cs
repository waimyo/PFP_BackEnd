﻿using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface IChattingRepository:IReadWriteRepository<Chatting,int>
    {
    }
}
