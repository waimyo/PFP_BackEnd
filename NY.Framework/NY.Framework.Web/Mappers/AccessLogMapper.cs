using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Model.Entities;
using NY.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Mappers
{
    public class AccessLogMapper
    {
        public JqueryDataTableQueryOptions<View_AccessLog> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<View_AccessLog> queryOption)
        {
            if (!string.IsNullOrEmpty(queryOption.SearchValue))
            {
                queryOption.FilterBy = (a => a.username.Contains(queryOption.SearchValue) ||
                a.role.Contains(queryOption.SearchValue) || a.page_accessed.Contains(queryOption.SearchValue) ||
                a.ip_address.Contains(queryOption.SearchValue));
            }

            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<View_AccessLog, object>>();
                    if (colName == "access_time")
                    {
                        queryOption.SortBy.Add(a => a.access_time);
                    }
                    else if (colName == "username")
                    {
                        queryOption.SortBy.Add(a => a.username);
                    }
                    else if (colName == "role")
                    {
                        queryOption.SortBy.Add(a => a.role);
                    }
                    else if (colName == "page_accessed")
                    {
                        queryOption.SortBy.Add(a => a.page_accessed);
                    }
                    else if (colName == "page_accessed")
                    {
                        queryOption.SortBy.Add(a => a.page_accessed);
                    }
                    else if (colName == "action")
                    {
                        queryOption.SortBy.Add(a => a.action);
                    }
                    else if (colName == "ip_address")
                    {
                        queryOption.SortBy.Add(a => a.ip_address);
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

        public JQueryDataTablePagedResult<AccessLogViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<View_AccessLog> al)
        {
            JQueryDataTablePagedResult<AccessLogViewModel> vmlist = new JQueryDataTablePagedResult<AccessLogViewModel>();
            foreach (var res in al.data)
            {
                AccessLogViewModel vm = new AccessLogViewModel();
                vm.id = res.ID;
                vm.access_time = res.access_time.ToString("dd/MM/yyyy HH:mm:ss");
                vm.username = res.username;
                vm.role = res.role;
                vm.page_accessed = res.page_accessed;
                vm.action = res.action;
                vm.ip_address = res.ip_address;

                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = al.recordsFiltered;
            vmlist.recordsTotal = al.recordsTotal;
            return vmlist;
        }

    }
}
