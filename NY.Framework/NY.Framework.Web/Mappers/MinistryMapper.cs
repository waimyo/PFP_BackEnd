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
    public class MinistryMapper
    {
        public JqueryDataTableQueryOptions<Ministry> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<Ministry> queryOption)
        {
            if (!string.IsNullOrEmpty(queryOption.SearchValue))
            {
                queryOption.FilterBy = (a => a.name.Contains(queryOption.SearchValue));
            }
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<Ministry, object>>();

                    if (colName == "name")
                    {
                        queryOption.SortBy.Add(a => a.name);
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

        public JQueryDataTablePagedResult<MinistryViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<Ministry> entity)
        {
            JQueryDataTablePagedResult<MinistryViewModel> vmlist = new JQueryDataTablePagedResult<MinistryViewModel>();
            foreach (var res in entity.data)
            {
                MinistryViewModel vm = new MinistryViewModel();
                vm.id = res.ID;
                vm.name = res.name;
                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = entity.recordsFiltered;
            vmlist.recordsTotal = entity.recordsTotal;
            return vmlist;
        }

        //public Ministry MapModelToEntity(Ministry entity, MinistryViewModel entrymodel)
        //{
        //    entity.name = entrymodel.name;
        //    //entity.logo = entrymodel.name;
        //    return entity;
        //}

    }
}
