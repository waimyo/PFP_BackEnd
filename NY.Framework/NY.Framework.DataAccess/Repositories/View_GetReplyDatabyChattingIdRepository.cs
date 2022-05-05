using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class View_GetReplyDatabyChattingIdRepository:ReadWriteRepositoryBase<View_GetReplyDatabyChattingId,int>, IView_GetReplyDatabyChattingIdRepository
    {
        public View_GetReplyDatabyChattingIdRepository(IDbContext context):base(context,new string[] { }) { }

        public List<View_GetReplyDatabyChattingId> GetByChattingId(int chat_id, int createdby_id)
        {
            return CustomQuery1().Where(x => x.chatting_id.Equals(chat_id) && (x.receiver.Equals(createdby_id) || x.sender.Equals(createdby_id))).ToList();
        }

        public List<View_GetReplyDatabyChattingId> GetByChattingId(int chat_id, int createdby_id, int parent_id)
        {
            //return CustomQuery1().Where(x => (x.receiver.Equals(createdby_id) && x.sender.Equals(parent_id)) || (x.receiver.Equals(parent_id) && x.sender.Equals(createdby_id)) && x.chatting_id.Equals(chat_id)).ToList();
            return CustomQuery1().Where(x => x.chatting_id.Equals(chat_id) && (x.sender.Equals(createdby_id) || x.receiver.Equals(createdby_id))).ToList();
            //return CustomQuery1().Where(x => x.chatting_id.Equals(chat_id) && x.receiver.Equals(createdby_id) && !x.sender.Equals(createdby_id)).ToList();
        }

        public List<IGrouping<int, View_GetReplyDatabyChattingId>> GetCountForUnreadMsgForCPU(int chat_id, int createdby_id)
        {
            return CustomQuery1().Where(x => x.isread.Equals(false) && x.chatting_id.Equals(chat_id) && x.receiver.Equals(createdby_id) && !x.sender.Equals(createdby_id)).GroupBy(x => x.reply_chatting_id).ToList();
        }
        //public int GetCountForUnreadMsgForCPU(int chat_id, int createdby_id)
        //{
        //    return CustomQuery1().Where(x => x.isread.Equals(false) && x.chatting_id.Equals(chat_id) && x.receiver.Equals(createdby_id) && !x.sender.Equals(createdby_id)).GroupBy(x => x.reply_chatting_id).Count();
        //}

        public int GetCountForUnreadMsgForDEO(int chat_id, int createdby_id, int parent_id)
        {
            return CustomQuery1().Where(x => x.isread.Equals(false) && (x.receiver.Equals(createdby_id) && x.sender.Equals(parent_id)) || (x.receiver.Equals(parent_id) && x.sender.Equals(createdby_id)) && x.chatting_id.Equals(chat_id)).GroupBy(x => x.reply_chatting_id).Count();
        }
    }
}
