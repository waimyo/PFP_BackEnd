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
    public class ServiceMapper
    {
        public JqueryDataTableQueryOptions<Service> Preparetorepository(JqueryDataTableQueryOptions<Service> queryoption)
        {
            if (!string.IsNullOrEmpty(queryoption.SearchValue))
            {
                queryoption.FilterBy = (c => c.name.Contains(queryoption.SearchValue) || c.Department.Name.Contains(queryoption.SearchValue) || c.Department.Ministry.name.Contains(queryoption.SearchValue));
            }
           
            if (queryoption.SortColumnsName.Count > 0)
            {
                foreach (var colName in queryoption.SortColumnsName)
                {
                    queryoption.SortBy = new List<Func<Service, object>>();
                    if (colName == "name")
                    {
                        queryoption.SortBy.Add(c => c.name);
                    }
                    else if (colName == "department_name")
                    {
                        queryoption.SortBy.Add(c => c.Dept_id);
                    }
                    else if (colName == "ministry_name")
                    {
                        queryoption.SortBy.Add(c => c.Department.Ministry_Id);
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
        public JqueryDataTableQueryOptions<Service> PreparetorepositoryForService(JqueryDataTableQueryOptions<Service> queryoption, ServicesViewModel model)
        {
            if (!string.IsNullOrEmpty(queryoption.SearchValue))
            {
                queryoption.FilterBy = (c => c.name.Contains(queryoption.SearchValue) || c.Department.Name.Contains(queryoption.SearchValue) || c.Department.Ministry.name.Contains(queryoption.SearchValue));
            }
            if (model.ministry_id > 0)
            {
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, (c => c.Department.Ministry_Id == model.ministry_id));
            }

            if (queryoption.SortColumnsName.Count > 0)
            {
                foreach (var colName in queryoption.SortColumnsName)
                {
                    queryoption.SortBy = new List<Func<Service, object>>();
                    if (colName == "name")
                    {
                        queryoption.SortBy.Add(c => c.name);
                    }
                    else if (colName == "department_name")
                    {
                        queryoption.SortBy.Add(c => c.Dept_id);
                    }
                    else if (colName == "ministry_name")
                    {
                        queryoption.SortBy.Add(c => c.Department.Ministry_Id);
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
        public Service MapEntityToViewModel(Service services, ServicesViewModel model)
        {
            services.ID = model.id;
            services.name = model.name;
            if (model.deptid != 0)
            {
                services.Dept_id = model.deptid;
            }

            return services;
        }
        public ServicesViewModel MapEntityToViewModel(Service service)
        {
            ServicesViewModel model = new ServicesViewModel();
            model.id = service.ID;
            model.deptid = service.Dept_id;
            model.ministry_id = service.Department.Ministry_Id;
            model.ministry_name = service.Department.Ministry.name;
            model.name = service.name;
            model.department_name = service.Department.Name;
            return model;

        }

        public JQueryDataTablePagedResult<ServicesViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<Service> services)
        {
            JQueryDataTablePagedResult<ServicesViewModel> vmlst = new JQueryDataTablePagedResult<ServicesViewModel>();
            foreach (var res in services.data)
            {
                ServicesViewModel vm = new ServicesViewModel();
                vm.id = res.ID;
                vm.name = res.name;
               // vm.ministry_id = res.Department.Ministry_Id;
                vm.deptid = res.Dept_id;
                if (res.Department != null)
                {
                    vm.department_name = res.Department.Name;
                    if (res.Department.Ministry != null)
                    {
                        vm.ministry_name = res.Department.Ministry.name;
                    }
                }
                
                vmlst.data.Add(vm);
            }
            vmlst.recordsFiltered = services.recordsFiltered;
            vmlst.recordsTotal = services.recordsTotal;
            return vmlst;

        }
    }    
}  