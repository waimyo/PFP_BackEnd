using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class ChattingParticipantRepository: ReadWriteRepositoryBase<ChattingParticipant,int>, IChattingParticipantRepository
    {
        public ChattingParticipantRepository(IDbContext context):base(context,new string[] { }) { }

        public List<ChattingParticipant> GetByChattingId(int chatid, int loginid)
        {
            return CustomQuery().Where(x => x.chatting_id.Equals(chatid) && x.receiver.Equals(loginid) && x.reply_chatting_id!=0).ToList();
        }
    }
}
