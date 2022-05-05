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
    public class DepartmentMapper
    {
        public JqueryDataTableQueryOptions<Department> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<Department> queryOption)
        {
            if (!string.IsNullOrEmpty(queryOption.SearchValue))
            {
                queryOption.FilterBy = (a => a.Name.Contains(queryOption.SearchValue) || a.Ministry.name.Contains(queryOption.SearchValue));
            }

            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<Department, object>>();

                    //if (colName == "d")
                    //{
                    //    queryOption.SortBy.Add(a => a.Name);
                    //}
                    //if (colName == "m")
                    //{
                    //    queryOption.SortBy.Add(a => a.Ministry_Id);
                    //}

                    if (colName == "name")
                    {
                        queryOption.SortBy.Add(a => a.Name);
                    }
                    else if (colName == "deptname")
                    {
                        queryOption.SortBy.Add(a => a.Ministry_Id);
                    }
                    else if (colName == "ministry_name")
                    {
                        queryOption.SortBy.Add(a => a.Ministry_Id);
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
        public JqueryDataTableQueryOptions<Department> PrepareQueryOptionForRepositoryForMin(JqueryDataTableQueryOptions<Department> queryOption, DepartmentViewModel model)
        {
            if (!string.IsNullOrEmpty(queryOption.SearchValue))
            {
                queryOption.FilterBy = (a => a.Name.Contains(queryOption.SearchValue) || a.Ministry.name.Contains(queryOption.SearchValue));
            }
            if (model.ministry_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (c => c.Ministry_Id == model.ministry_id));
            }


            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<Department, object>>();

                    //if (colName == "d")
                    //{
                    //    queryOption.SortBy.Add(a => a.Name);
                    //}
                    //if (colName == "m")
                    //{
                    //    queryOption.SortBy.Add(a => a.Ministry_Id);
                    //}

                    if (colName == "name")
                    {
                        queryOption.SortBy.Add(a => a.Name);
                    }
                    else if (colName == "deptname")
                    {
                        queryOption.SortBy.Add(a => a.Ministry_Id);
                    }
                    else if (colName == "ministry_name")
                    {
                        queryOption.SortBy.Add(a => a.Ministry_Id);
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

        public JQueryDataTablePagedResult<DepartmentViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<Department> dept)
        {
            JQueryDataTablePagedResult<DepartmentViewModel> vmlist = new JQueryDataTablePagedResult<DepartmentViewModel>();
            foreach (var res in dept.data)
            {
                DepartmentViewModel vm = new DepartmentViewModel();
                vm.id = res.ID;
                vm.name = res.Name;
                vm.ministry_id = res.Ministry_Id;
                if(res.Ministry!=null)
                {
                    vm.ministry_name = res.Ministry.name;
                }

                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = dept.recordsFiltered;
            vmlist.recordsTotal = dept.recordsTotal;
            return vmlist;
        }

        public Department MapModelToEntity(Department dept, DepartmentViewModel entrymodel)
        {
            dept.Name = entrymodel.name;
            dept.Ministry_Id = entrymodel.ministry_id;
            return dept;
        }

    }
}
