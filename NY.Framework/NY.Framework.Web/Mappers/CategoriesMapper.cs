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
    public class CategoriesMapper
    {
        public JqueryDataTableQueryOptions<Categories>Preparetorepository(JqueryDataTableQueryOptions<Categories> queryoption)
        {
            if (!string.IsNullOrEmpty(queryoption.SearchValue))
            {
                queryoption.FilterBy = (c => c.name.Contains(queryoption.SearchValue));
            }
            if(queryoption.SortColumnsName.Count>0)
            {
                foreach (var colName in queryoption.SortColumnsName)
                {
                    queryoption.SortBy = new List<Func<Categories, object>>();
                    if (colName == "name")
                    {
                        queryoption.SortBy.Add(c => c.name);
                    }
                    else
                    {
                        queryoption.SortOrder = SortOrder.DESC;
                        queryoption.SortBy.Add((x => x.ID));
                    }
                }
               
            }
            return queryoption;
        }
        public Categories MapEntityToViewModel(Categories categories,CategoriesViewModel model)
        {
            //CategoriesViewModel model = new CategoriesViewModel();
            //categories.ID = model.id;
            categories.name = model.name; 
            return categories;
        }
        public JQueryDataTablePagedResult<CategoriesViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<Categories> categories)
        {
            JQueryDataTablePagedResult<CategoriesViewModel> vmlst = new JQueryDataTablePagedResult<CategoriesViewModel>();
            foreach (var res in categories.data)
            {
                CategoriesViewModel vm = new CategoriesViewModel
                {
                    id = res.ID,
                    name = res.name
                };
                vmlst.data.Add(vm);
            }
            vmlst.recordsFiltered = categories.recordsFiltered;
            vmlst.recordsTotal = categories.recordsTotal;
            return vmlst;

        }
    }
}
