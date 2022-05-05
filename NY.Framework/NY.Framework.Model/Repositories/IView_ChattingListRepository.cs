using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.Model.Repositories
{
    public interface IView_ChattingListRepository : IReadWriteRepository<View_ChattingList,int>
    {
        JQueryDataTablePagedResult<View_ChattingList> GetPagedResultsForAsNoTracking(JqueryDataTableQueryOptions<View_ChattingList> option);
        IGrouping<int, View_ChattingList> GetByChattingId(int chat_id);
    }
}
