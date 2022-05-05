using Microsoft.EntityFrameworkCore;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
    public class View_ChattingListRepository:ReadWriteRepositoryBase<View_ChattingList,int>, IView_ChattingListRepository
    {
        public View_ChattingListRepository(IDbContext context):base(context,new string[] { }) { }

        public IGrouping<int, View_ChattingList> GetByChattingId(int chat_id)
        {
            return CustomQuery().Where(x => x.chat_id.Equals(chat_id)).GroupBy(x => x.chat_id).FirstOrDefault();
        }

        public JQueryDataTablePagedResult<View_ChattingList> GetPagedResultsForAsNoTracking(JqueryDataTableQueryOptions<View_ChattingList> option)
        {
            JQueryDataTablePagedResult<View_ChattingList> results = new JQueryDataTablePagedResult<View_ChattingList>();
            results.draw = option.Draw;
            if (option.FilterBy != null)
            {
                results.recordsTotal = context.Set<View_ChattingList>().Where(option.FilterBy).Count();
            }
            else
            {
                results.recordsTotal = context.Set<View_ChattingList>().Count();
            }
            if (results.recordsTotal > 0)
            {
                var query = context.Set<View_ChattingList>().AsNoTracking();
                foreach (string s in includes)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        query.Include(s);
                    }
                }
                if (option.FilterBy != null)
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = query.Where(option.FilterBy).OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 0)
                        {
                            for (int i = 0; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        /***for all records***/
                        if (option.Length == -1)
                        {
                            results.data = q.ToList();
                        }
                        /***for caselist excel records***/
                        else if (option.Length == -2)
                        {
                            results.data = q.Skip(option.Start).Take(results.recordsTotal).ToList();
                        }
                        else
                        {
                            results.data = q.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                    else
                    {
                        IOrderedEnumerable<View_ChattingList> tmp = query.Where(option.FilterBy).OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 0)
                        {
                            for (int i = 0; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        /***for all records***/
                        if (option.Length == -1)
                        {
                            results.data = tmp.ToList();
                        }
                        /***for caselist excel records***/
                        else if (option.Length == -2)
                        {
                            results.data = tmp.Skip(option.Start).Take(results.recordsTotal).ToList();
                        }

                        else
                        {
                            results.data = tmp.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                }
                else
                {
                    if (option.SortOrder == SortOrder.DESC)
                    {
                        var q = query.OrderByDescending(option.SortBy[0]);

                        if (option.SortBy.Count > 0)
                        {
                            for (int i = 0; i < option.SortBy.Count; i++)
                            {
                                q = q.ThenByDescending(option.SortBy[i]);
                            }
                        }
                        /***for all records***/
                        if (option.Length == -1)
                        {
                            results.data = q.ToList();
                        }
                        /***for caselist excel records***/
                        else if (option.Length == -2)
                        {
                            results.data = q.Skip(option.Start).Take(results.recordsTotal).ToList();
                        }
                        else
                        {
                            results.data = q.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                    else
                    {
                        IOrderedEnumerable<View_ChattingList> tmp = query.OrderBy(option.SortBy[0]);
                        if (option.SortBy.Count > 0)
                        {
                            for (int i = 0; i < option.SortBy.Count; i++)
                            {
                                tmp = tmp.ThenBy(option.SortBy[i]);
                            }
                        }
                        // var sql = tmp.tosq();
                        /***for all records***/
                        if (option.Length == -1)
                        {
                            results.data = tmp.ToList();
                        }
                        /***for caselist excel records***/
                        else if (option.Length == -2)
                        {
                            results.data = tmp.Skip(option.Start).Take(results.recordsTotal).ToList();
                        }
                        else
                        {
                            results.data = tmp.Skip(option.Start).Take(option.Length).ToList();
                        }
                    }
                }
                results.recordsFiltered = results.recordsTotal;
            }
            return results;
        }
    }
}
