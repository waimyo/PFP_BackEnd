using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Mappers
{
    public class ChattingMapper
    {
        public JqueryDataTableQueryOptions<View_ChattingList> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<View_ChattingList> queryOption, ChattingListViewModel model)
        {
            if (!string.IsNullOrEmpty(queryOption.SearchValue))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                    (x => x.description.Contains(queryOption.SearchValue)));
            }
            if (!string.IsNullOrEmpty(model.fromdate) && !string.IsNullOrEmpty(model.todate))
            {
                DateTime fromdt = DateTime.ParseExact(model.fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime todt = DateTime.ParseExact(model.todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.created_date.Date >= fromdt.Date && d.created_date.Date <= todt.Date));
            }
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<View_ChattingList, object>>();
                    if (colName == "description")
                    {
                        queryOption.SortBy.Add((x => x.description));
                    }
                    else
                    {
                        queryOption.SortOrder = SortOrder.DESC;
                        queryOption.SortBy.Add((x => x.ID));
                    }
                }
            }
            return queryOption;
        }

        public JQueryDataTablePagedResult<ChattingListViewModel> MapModelToListViewModel(List<IGrouping<int, View_ChattingList>> list, IView_GetReplyDatabyChattingIdRepository replybychattingidRepo, int loginid)
        {
            JQueryDataTablePagedResult<ChattingListViewModel> vmlist = new JQueryDataTablePagedResult<ChattingListViewModel>();
            foreach (IGrouping<int, View_ChattingList> entity in list)
            {
                ChattingListViewModel vm = new ChattingListViewModel();
                vm.id = entity.Key;
                foreach (View_ChattingList v in entity)
                {
                    vm.id = v.ID;
                    vm.created_date = string.Format("{0:dd-MM-yyyy  hh-mm-ss tt}", v.created_date);
                    vm.description = v.description;
                    if (!string.IsNullOrEmpty(vm.receiver))
                    {
                        vm.receiver += ", " + v.receivername;
                    }
                    else
                    {
                        vm.receiver += v.receivername;
                    }
                    vm.chat_id = v.chat_id;
                }
                //vmlist.recordsFiltered = v.recordsFiltered;
               
                vm.countfornunread = replybychattingidRepo.GetCountForUnreadMsgForCPU(entity.Key, loginid).Count();
                vmlist.data.Add(vm);
                if (vmlist.data != null)
                {
                    vmlist.recordsTotal = vmlist.data.Count;
                }               
            }

            return vmlist;
        }
    }
}
