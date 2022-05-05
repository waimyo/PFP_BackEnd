using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface IView_GetReplyDatabyChattingIdRepository:IReadWriteRepository<View_GetReplyDatabyChattingId,int>
    {
        List<View_GetReplyDatabyChattingId> GetByChattingId(int chat_id, int createdby_id);
        List<View_GetReplyDatabyChattingId> GetByChattingId(int chat_id, int createdby_id, int parent_id);

        List<IGrouping<int, View_GetReplyDatabyChattingId>> GetCountForUnreadMsgForCPU(int chat_id, int createdby_id);
        int GetCountForUnreadMsgForDEO(int chat_id, int createdby_id, int parent_id);
    }
}
