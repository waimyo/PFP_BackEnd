using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class ChattingRepository: ReadWriteRepositoryBase<Chatting,int>, IChattingRepository
    {
        public ChattingRepository(IDbContext context):base(context,new string []{ }) { }
    }
}
