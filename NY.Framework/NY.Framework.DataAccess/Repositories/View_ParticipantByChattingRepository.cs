using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class View_ParticipantByChattingRepository: ReadWriteRepositoryBase<View_ParticipantByChatting,int>, IView_ParticipantByChattingRepository
    {
        public View_ParticipantByChattingRepository(IDbContext context):base(context,new string[] { }) { }

        public List<View_ParticipantByChatting> GetDEOAccountByChatting(int chat_id)
        {
            return CustomQuery().Where(x => x.chatting_id == chat_id).ToList();
        }
    }
}
