using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Mappers
{
    public class ResponseStatusMapper
    {
        public JqueryDataTableQueryOptions<ResponseStatus> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<ResponseStatus> queryOption, ResponseStatusSearchViewModel searchmodel)
        {
            //if (!string.IsNullOrEmpty(queryOption.SearchValue))
            //{
            //    queryOption.FilterBy = (a => a.name.Contains(queryOption.SearchValue));
            //}
            if (searchmodel.ministry_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (s => s.ministry_id == searchmodel.ministry_id));
            }


            //if (!string.IsNullOrEmpty(searchmodel.fromdate) && !string.IsNullOrEmpty(searchmodel.todate))
            //{
            //    queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
            //        (s => s.sms_time >= Convert.ToDateTime(searchmodel.fromdate) && s.sms_time <= Convert.ToDateTime(searchmodel.todate)));
            //}
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<ResponseStatus, object>>();

                    if (colName == "name")
                    {
                        queryOption.SortBy.Add(a => a.name);
                    }
                    if (colName == "username")
                    {
                        queryOption.SortBy.Add(a => a.username);
                    }
                    if (colName == "role_name")
                    {
                        queryOption.SortBy.Add(a => a.role_name);
                    }
                    if (colName == "send")
                    {
                        queryOption.SortBy.Add(a => a.sms_send_count);
                    }
                    if (colName == "received")
                    {
                        queryOption.SortBy.Add(a => a.sms_receive_count);
                    }
                    if (colName == "response_rate")
                    {
                        queryOption.SortBy.Add(a => a.response_rate);
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

        public JQueryDataTablePagedResult<ResponseStatusListViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<ResponseStatus> entity)
        {
            JQueryDataTablePagedResult<ResponseStatusListViewModel> vmlist = new JQueryDataTablePagedResult<ResponseStatusListViewModel>();
            foreach (var res in entity.data)
            {
                ResponseStatusListViewModel vm = new ResponseStatusListViewModel();
                vm.id = res.ID;
                vm.name = res.name;
                vm.username = res.username;
                vm.role_name = res.role_name;
                vm.sms_send_count = res.sms_send_count;
                vm.sms_receive_count = res.sms_receive_count;
              //  vm.response_rate = res.response_rate;
                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = entity.recordsFiltered;
            vmlist.recordsTotal = entity.recordsTotal;
            return vmlist;
        }
    }
}
