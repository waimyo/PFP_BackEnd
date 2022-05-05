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
    public class CategorizationStatusMapper
    {
        public JqueryDataTableQueryOptions<CategorizationStatus> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<CategorizationStatus> queryOption, CategorizationStatusSearchViewModel searchmodel)
        {
            //if (!string.IsNullOrEmpty(queryOption.SearchValue))
            //{
            //    queryOption.FilterBy = (a => a.name.Contains(queryOption.SearchValue));
            //}
            if (searchmodel.ministry_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (s => s.min_id == searchmodel.ministry_id));
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
                    queryOption.SortBy = new List<Func<CategorizationStatus, object>>();

                    if (colName == "account")
                    {
                        queryOption.SortBy.Add(a => a.name);
                    }
                    if (colName == "name")
                    {
                        queryOption.SortBy.Add(a => a.username);
                    }
                    if (colName == "role")
                    {
                        queryOption.SortBy.Add(a => a.role_name);
                    }
                    if (colName == "satisfied")
                    {
                        queryOption.SortBy.Add(a => a.satisfied);
                    }
                    if (colName == "dissatisfied")
                    {
                        queryOption.SortBy.Add(a => a.dissatisfied);
                    }
                    if (colName == "suggestion")
                    {
                        queryOption.SortBy.Add(a => a.suggestion);
                    }
                    if (colName == "appreciation")
                    {
                        queryOption.SortBy.Add(a => a.appreciation);
                    }
                    if (colName == "not_relevant")
                    {
                        queryOption.SortBy.Add(a => a.not_relevant);
                    }
                    if (colName == "corruption")
                    {
                        queryOption.SortBy.Add(a => a.corruption);
                    }
                    if (colName == "other")
                    {
                        queryOption.SortBy.Add(a => a.other);
                    }
                    if (colName == "charge")
                    {
                        queryOption.SortBy.Add(a => a.charge);
                    }
                    if (colName == "no_charge")
                    {
                        queryOption.SortBy.Add(a => a.no_charge);
                    }
                    if (colName == "categorized")
                    {
                        queryOption.SortBy.Add(a => a.categorized_count);
                    }
                    if (colName == "uncategorized")
                    {
                        queryOption.SortBy.Add(a => a.non_categorized_count);
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

        public JQueryDataTablePagedResult<CategorizationStatusListViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<CategorizationStatus> entity)
        {
            JQueryDataTablePagedResult<CategorizationStatusListViewModel> vmlist = new JQueryDataTablePagedResult<CategorizationStatusListViewModel>();
            foreach (var res in entity.data)
            {
                CategorizationStatusListViewModel vm = new CategorizationStatusListViewModel();
                vm.id = res.ID;
                vm.name = res.name;
                vm.username = res.username;
                vm.role_name = res.role_name;
                vm.satisfied = res.satisfied;
                vm.dissatisfied = res.dissatisfied;
                vm.suggestion = res.appreciation;

                vm.not_relevant = res.not_relevant;
                vm.corruption = res.corruption;
                vm.other = res.other;
                vm.charge = res.charge;
                vm.no_charge = res.no_charge;
                vm.categorized_count = res.categorized_count;
                vm.non_categorized_count = res.non_categorized_count;
              //  vm.grand_total = res.grand_total;
                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = entity.recordsFiltered;
            vmlist.recordsTotal = entity.recordsTotal;
            return vmlist;
        }
    }
}
