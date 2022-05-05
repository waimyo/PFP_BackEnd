using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class ChattingFileRepository:ReadWriteRepositoryBase<ChattingFile,int>, IChattingFileRepository
    {
        public ChattingFileRepository(IDbContext context):base(context,new string []{ }) { }

        public List<ChattingFile> GetByChattingId(int chatid)
        {
            return CustomQuery().Where(x => x.chatting_id.Equals(chatid)).ToList();
        }
    }
}
