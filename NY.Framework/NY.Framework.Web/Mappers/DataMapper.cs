using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Mappers
{
    public class DataMapper
    {
        public JqueryDataTableQueryOptions<View_DataInfo> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<View_DataInfo> queryOption, DataSearchViewModel searchmodel)
        {
            //queryOption.FilterBy = x => x.IsDeleted == false;
            if (!string.IsNullOrEmpty(searchmodel.name))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (s => s.name.Contains(searchmodel.name)
                || s.mobile.Contains(searchmodel.name) || s.ministry.Contains(searchmodel.name) || s.department.Contains(searchmodel.name)
                || s.statedivision.Contains(searchmodel.name)
                || s.district.Contains(searchmodel.name) || s.township.Contains(searchmodel.name)
                || s.username.Contains(searchmodel.name)));
            }
            if (string.IsNullOrEmpty(searchmodel.name) && !string.IsNullOrEmpty(searchmodel.service))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, s => s.service.Contains(searchmodel.service));
            }
            if (!string.IsNullOrEmpty(searchmodel.service) && !string.IsNullOrEmpty(searchmodel.name))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, s => s.service.Contains(searchmodel.service));
            }

            if (searchmodel.ministry_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (s => s.ministry_id == searchmodel.ministry_id));
            }
            if (searchmodel.user_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (s => s.userid == searchmodel.user_id));
            }
            if (!string.IsNullOrEmpty(searchmodel.gender))
            {
                bool gender = false;
                if (searchmodel.gender == "true")
                {
                    gender = true;
                }
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (s => s.gender == gender));
            }
            if (!string.IsNullOrEmpty(searchmodel.fromdate) && !string.IsNullOrEmpty(searchmodel.todate))
            {
                DateTime fromdt = DateTime.ParseExact(searchmodel.fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime todt = DateTime.ParseExact(searchmodel.todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (s => s.created_date.Date >= Convert.ToDateTime(fromdt) && s.created_date.Date <= Convert.ToDateTime(todt)));
            }
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<View_DataInfo, object>>();

                    if (colName == "dataid")
                    {
                        queryOption.SortBy.Add(a => a.ID);
                    }
                    else if (colName == "name")
                    {
                        queryOption.SortBy.Add(a => a.name);
                    }
                    else if (colName == "mobile")
                    {
                        queryOption.SortBy.Add(a => a.mobile);
                    }
                    else if (colName == "gender")
                    {
                        queryOption.SortBy.Add(a => a.gender);
                    }
                    else if (colName == "ministry")
                    {
                        queryOption.SortBy.Add(a => a.ministry);
                    }
                    else if (colName == "department")
                    {
                        queryOption.SortBy.Add(a => a.department);
                    }
                    else if (colName == "service")
                    {
                        queryOption.SortBy.Add(a => a.service);
                    }
                    else if (colName == "statedivision")
                    {
                        queryOption.SortBy.Add(a => a.statedivision);
                    }
                    else if (colName == "district")
                    {
                        queryOption.SortBy.Add(a => a.district);
                    }
                    else if (colName == "township")
                    {
                        queryOption.SortBy.Add(a => a.township);
                    }
                    else if (colName == "date_of_application")
                    {
                        queryOption.SortBy.Add(a => a.date_of_application);
                    }
                    else if (colName == "date_of_completion")
                    {
                        queryOption.SortBy.Add(a => a.date_of_completion);
                    }
                    else if (colName == "created_by")
                    {
                        queryOption.SortBy.Add(a => a.username);
                    }
                    else if (colName == "created_date")
                    {
                        queryOption.SortBy.Add(a => a.created_date);
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

        // list For Page
        public JQueryDataTablePagedResult<DataListViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<View_DataInfo> entity)
        {
            JQueryDataTablePagedResult<DataListViewModel> vmlist = new JQueryDataTablePagedResult<DataListViewModel>();
            foreach (var res in entity.data)
            {
                DataListViewModel vm = new DataListViewModel();
                vm.id = res.ID;
                vm.name = res.name;
                if (!string.IsNullOrEmpty(res.mobile))
                {
                    if (res.mobile.Length <= 4)
                    {
                        vm.mobile = res.mobile;
                    }
                    else
                    {
                        var first = res.mobile.Substring(0, 4);
                        var last = res.mobile.Last().ToString();
                        var star = "";
                        for (int i = 0; i < res.mobile.Length - 5; i++)
                        {
                            star += "*";
                        }
                        vm.mobile = first + star + last;
                    }
                }
                if (res.gender == true) { vm.gender = "ကျား"; }
                else { vm.gender = "မ"; }
                vm.ministry = res.ministry;
                vm.department = res.department;
                vm.service = res.service;
                vm.statedivision = res.statedivision;
                vm.district = res.district;
                vm.township = res.township;
                vm.date_of_application = string.Format("{0:dd-MM-yyyy}", res.date_of_application);
                vm.date_of_completion = string.Format("{0:dd-MM-yyyy}", res.date_of_completion);
                vm.created_by = res.username;
                vm.created_date = string.Format("{0:dd-MM-yyyy hh:mm:ss}", res.created_date);
                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = entity.recordsFiltered;
            vmlist.recordsTotal = entity.recordsTotal;
            return vmlist;
        }

        // list for Excel 
        public List<DataListViewModel> MapModelToListViewModel(List<View_DataInfo> entity)
        {
            List<DataListViewModel> vmlist = new List<DataListViewModel>();
            if (entity != null)
            {
                foreach (var res in entity)
                {
                    DataListViewModel vm = new DataListViewModel();
                    vm.id = res.ID;
                    vm.name = res.name;
                    if (!string.IsNullOrEmpty(res.mobile))
                    {
                        if (res.mobile.Length <= 4)
                        {
                            vm.mobile = res.mobile;
                        }
                        else
                        {
                            var first = res.mobile.Substring(0, 4);
                            var last = res.mobile.Last().ToString();
                            var star = "";
                            for (int i = 0; i < res.mobile.Length - 5; i++)
                            {
                                star += "*";
                            }
                            vm.mobile = first + star + last;
                        }
                    }
                    if (res.gender == true) { vm.gender = "ကျား"; }
                    else { vm.gender = "မ"; }
                    vm.ministry = res.ministry;
                    vm.department = res.department;
                    vm.service = res.service;
                    vm.statedivision = res.statedivision;
                    vm.district = res.district;
                    vm.township = res.township;
                    vm.date_of_application = string.Format("{0:dd-MM-yyyy}", res.date_of_application);
                    vm.date_of_completion = string.Format("{0:dd-MM-yyyy}", res.date_of_completion);
                    vm.created_by = res.username;
                    vm.created_date = string.Format("{0:dd-MM-yyyy hh:mm:ss}", res.created_date);
                    vmlist.Add(vm);
                }
            }
            return vmlist;
        }

        public Data MapModelToEntity(Data entity, DataViewModel entrymodel)
        {
            entity.name = entrymodel.name;
            entity.mobile = entrymodel.mobile;
            entity.gender = entrymodel.gender;
            entity.service_id = entrymodel.service_id;
            entity.department_id = entrymodel.department_id;
            entity.ministry_id = entrymodel.ministry_id;
            entity.date_of_application = entrymodel.date_of_application;
            entity.date_of_completion = entrymodel.date_of_completion;
            entity.location_state_id = entrymodel.location_state;
            entity.location_district_id = entrymodel.location_district;
            entity.location_township_id = entrymodel.location_township;
            entity.uploaded_file_id = entrymodel.uploaded_file_id;
            return entity;
        }

    }
}
