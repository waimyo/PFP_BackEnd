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
    public class GroupMapper
    {
        public JqueryDataTableQueryOptions<View_DataInfo> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<View_DataInfo> queryOption, GroupListViewModel model, int uid)
        {
            
            if(model.department_id == 0 || model.service_id == 0 || 
                model.uploadedby == 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.userid.Equals(uid) || d.parentuserid.Equals(uid)));
            }
            if(model.department_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.departmentid.Equals(model.department_id)));
            }
            if (model.service_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.serviceid.Equals(model.service_id)));
            }
            //if (model.fromdate > 0 && model.todate > 0)
            //{
            //    DateTime fromdt = Convert.ToDateTime(model.fromdate);
            //    DateTime todt = Convert.ToDateTime(model.todate);
            //    queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.CreatedDate <= fromdt && d.CreatedDate >= todt));
            //}
            if (!string.IsNullOrEmpty(model.fromdate) && !string.IsNullOrEmpty(model.todate))
            {
                DateTime fromdt = DateTime.ParseExact(model.fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime todt = DateTime.ParseExact(model.todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.created_date.Date >= fromdt.Date && d.created_date.Date <= todt.Date));
            }
            if (model.uploadedby > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.userid.Equals(model.uploadedby)));
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
                    else if(colName == "name")
                    {
                        queryOption.SortBy.Add(a => a.name);
                    }
                    else if (colName == "mobile")
                    {
                        queryOption.SortBy.Add(a => a.mobile);
                    }
                    else if (colName == "department")
                    {
                        queryOption.SortBy.Add(a => a.department);
                    }
                    else if (colName == "service")
                    {
                        queryOption.SortBy.Add(a => a.service);
                    }
                    else if (colName == "uploadedby")
                    {
                        queryOption.SortBy.Add(a => a.username);
                    }
                    else if (colName == "uploadeddate")
                    {
                        queryOption.SortBy.Add(a => a.created_date);
                    }
                    else if (colName == "state")
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
                    else
                    {
                        queryOption.SortOrder = SortOrder.DESC;
                        queryOption.SortBy.Add((x => x.ID));
                    }
                }
            }
            return queryOption;
        }

        public JQueryDataTablePagedResult<DataListViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<Data> entity)
        {
            JQueryDataTablePagedResult<DataListViewModel> vmlist = new JQueryDataTablePagedResult<DataListViewModel>();
            foreach (var res in entity.data)
            {
                DataListViewModel vm = new DataListViewModel();
                vm.id = res.ID;
                vm.name = res.name;
                vm.mobile = res.mobile;
                if (res.gender == true) { vm.gender = "ကျား"; }
                else { vm.gender = "မ"; }
                if (res.Ministry != null) { vm.ministry = res.Ministry.name; }
                if (res.Department != null) { vm.department = res.Department.Name; }
                if (res.Service != null) { vm.service = res.Service.name; }
                if (res.LocationStateDivision != null) { vm.statedivision = res.LocationStateDivision.Name; }
                if (res.LocationDistrict != null) { vm.district = res.LocationDistrict.Name; }
                if (res.LocationTownship != null) { vm.township = res.LocationTownship.Name; }
                vm.date_of_application = string.Format("{0:dd/MM/yyyy}", res.date_of_application);
                vm.date_of_completion = string.Format("{0:dd/MM/yyyy}", res.date_of_completion);
                vm.created_by = "";
                vm.created_date = string.Format("{0:dd/MM/yyyy}", res.CreatedDate);
                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = entity.recordsFiltered;
            vmlist.recordsTotal = entity.recordsTotal;
            return vmlist;
        }

        public Groups MapModelToEntity(Groups gp, GroupListViewModel entrymodel)
        {
            gp.Name = entrymodel.name;
            return gp;
        }

        //for insert
        //public List<GroupMember> MapViewModelToEntity(GroupListViewModel modellist)
        //{
        //    List<GroupMember> lists = new List<GroupMember>();
        //    foreach (var clist in modellist.GList)
        //    {
        //        GroupMember cb = new GroupMember();
        //        //cb.group_id = gp.ID;
        //        cb.datainfo_id = clist.id;
        //        lists.Add(cb);
        //    }
        //    return lists;
        //}
        public List<GroupMember> MapViewModelToEntity(List<View_DataInfo> datalist, int gid, int uid)
        {
            List<GroupMember> lists = new List<GroupMember>();
            foreach (var l in datalist)
            {
                GroupMember cb = new GroupMember();
                //cb.group_id = gp.ID;
                cb.group_id = gid;
                cb.datainfo_id = l.ID;
                cb.CreatedBy = uid;
                cb.CreatedDate = DateTime.Now;
                lists.Add(cb);
            }
            return lists;
        }

        //For Group List
        public JqueryDataTableQueryOptions<View_GroupList> PrepareQueryOptionForRepositoryGroup(JqueryDataTableQueryOptions<View_GroupList> queryOption, GroupSearchViewModel model)
        {
            //for ministry acc filter
            if (model.ministry_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                  c => c.user_id == model.ministry_id || c.parent_id == model.ministry_id);
            }
            //for cpu acc filter
            if (model.ministry_id > 0 && model.user_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                    c => c.user_id == model.user_id);
            }
            

            if (!string.IsNullOrEmpty(model.fromdate) && !string.IsNullOrEmpty(model.todate))
            {
                DateTime fromdt = DateTime.ParseExact(model.fromdate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime todt = DateTime.ParseExact(model.todate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.group_date.Value.Date >= fromdt.Date && d.group_date.Value.Date <= todt.Date));
            }

            if (!string.IsNullOrEmpty(model.group_name))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.group_name.Contains(model.group_name)));
            }
            if(model.group_member > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (d => d.group_member.Equals(model.group_member)));
            }
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<View_GroupList, object>>();

                    if (colName == "groupno")
                    {
                        queryOption.SortBy.Add(a => a.ID);
                    }
                    else if (colName == "username")
                    {
                        queryOption.SortBy.Add(a => a.username);
                    }
                    else if (colName == "group_name")
                    {
                        queryOption.SortBy.Add(a => a.group_name);
                    }
                    else if (colName == "group_member")
                    {
                        queryOption.SortBy.Add(a => a.group_member);
                    }
                    else if (colName == "gdate")
                    {
                        queryOption.SortBy.Add(a => a.gdate);
                    }
                    else
                    {
                        
                        queryOption.SortBy.Add((x => x.ID));
                        queryOption.SortOrder = SortOrder.DESC;
                    }
                }
            }
            return queryOption;
        }

        //For Group Detail
        public GroupSearchViewModel MapEntityToViewModel(View_GroupList glist)
        {
            GroupSearchViewModel model = new GroupSearchViewModel();
            model.Id = glist.ID;
            //model.group_no = glist.group_no;
            model.group_name = glist.group_name;
            model.group_date = string.Format("{0:dd/MM/yyyy HH:mm:ss}", glist.group_date);
            model.group_member = glist.group_member;
            return model;
        }


        ////For Group Member List
        //public JqueryDataTableQueryOptions<Data> PrepareQueryOptionForRepositoryGroupMember(JqueryDataTableQueryOptions<Data> queryOption)
        //{

        //    if (queryOption.SortColumnsName.Count > 0)
        //    {
        //        foreach (string colName in queryOption.SortColumnsName)
        //        {
        //            queryOption.SortBy = new List<Func<Data, object>>();

        //            if (colName == "name")
        //            {
        //                queryOption.SortBy.Add(a => a.name);
        //            }

        //            else
        //            {
        //                queryOption.SortOrder = SortOrder.DESC;
        //                queryOption.SortBy.Add((x => x.ID));
        //            }
        //        }
        //    }
        //    return queryOption;
        //}

        //For Group Member List
        public JqueryDataTableQueryOptions<SP_GroupMemberList> PrepareQueryOptionForRepositoryGroupMember(JqueryDataTableQueryOptions<SP_GroupMemberList> queryOption)
        {
            if (!string.IsNullOrEmpty(queryOption.SearchValue))
            {
                queryOption.FilterBy = (a => a.name.Contains(queryOption.SearchValue) || 
                a.mobile.Contains(queryOption.SearchValue) ||
                a.sender_name.Contains(queryOption.SearchValue));
            }

            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<SP_GroupMemberList, object>>();
                    if (colName == "id")
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
                    else if (colName == "sender_name")
                    {
                        queryOption.SortBy.Add(a => a.sender_name);
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


        public JQueryDataTablePagedResult<GroupMemberListViewModel> MapModelToListViewModelGroupMember(JQueryDataTablePagedResult<SP_GroupMemberList> entity)
        {
            JQueryDataTablePagedResult<GroupMemberListViewModel> vmlist = new JQueryDataTablePagedResult<GroupMemberListViewModel>();
            foreach (var res in entity.data)
            {
                GroupMemberListViewModel vm = new GroupMemberListViewModel();
                vm.Id = res.ID;
                vm.datainfo_id = res.datainfo_id;
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
                //if (!string.IsNullOrEmpty(res.mobile))
                //{
                //    var first = res.mobile.Substring(0, 4);
                //    var last = res.mobile.Last().ToString();
                //    var star = "";
                //    for (int i = 0; i < res.mobile.Length - 5; i++)
                //    {
                //        star += "*";
                //    }
                //    vm.mobile = first + star + last;
                //}
                vm.sender_name = res.sender_name;
                vm.created_date = string.Format("{0:dd-MM-yyyy hh:mm:ss}", res.created_date);
                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = entity.recordsFiltered;
            vmlist.recordsTotal = entity.recordsTotal;
            return vmlist;
        }


        //for member insert
        public List<GroupMember> MapViewModelToEntityMember(GroupMemberInsertViewModel modellist)
        {
            List<GroupMember> lists = new List<GroupMember>();
            foreach (var clist in modellist.datainfo)
            {
                GroupMember cb = new GroupMember();
                cb.group_id = modellist.group_id;
                cb.datainfo_id = clist;
                lists.Add(cb);
            }
            return lists;
        }


        public Data MapModelToEntityData(Data data, int uid)
        {
            data.ModifiedBy = uid;
            data.ModifiedDate = DateTime.Now;
            data.IsDeleted = false;
            return data;
        }

    }
}
