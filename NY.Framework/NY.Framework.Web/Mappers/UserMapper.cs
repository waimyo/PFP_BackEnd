using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Mappers
{
    public class UserMapper
    {
        public JqueryDataTableQueryOptions<User> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<User> queryOption, UserFilterViewModel vm)
        {
            if (!string.IsNullOrEmpty(vm.search) && vm.role_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, u => (u.role_id==vm.role_id) &&
                (u.name.Contains(vm.search) || u.username.Contains(vm.search) || u.email.Contains(vm.search) ||
                u.Ministry.name.Contains(vm.search))
                );
            }
            else if (!string.IsNullOrEmpty(vm.search) && vm.role_id == 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, u =>
                u.name.Contains(vm.search) || u.username.Contains(vm.search) || u.email.Contains(vm.search) ||
                u.Ministry.name.Contains(vm.search));
            }
            if (string.IsNullOrEmpty(vm.search) && vm.role_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, u => u.role_id == vm.role_id);
            }
            if (vm.ministry_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, u => u.ministry_id == vm.ministry_id);
            }
            if (!string.IsNullOrEmpty(vm.fromdate) && !string.IsNullOrEmpty(vm.todate))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (s => s.CreatedDate >= Convert.ToDateTime(vm.fromdate) && s.CreatedDate <= Convert.ToDateTime(vm.todate)));
            }
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<User, object>>();

                    if (colName == "name")
                    {
                        queryOption.SortBy.Add(a => a.name);
                    }
                    else if (colName == "status")
                    {
                        queryOption.SortBy.Add(a => a.status);
                    }
                    else if (colName == "username")
                    {
                        queryOption.SortBy.Add(a => a.username);
                    }
                    else if (colName == "email")
                    {
                        queryOption.SortBy.Add(a => a.email);
                    }
                    else if (colName == "parent_ministry")
                    {
                        queryOption.SortBy.Add(a => a.ministry_id);
                    }
                    else if (colName == "parent_cpu")
                    {
                        queryOption.SortBy.Add(a => a.parent_id);
                    }
                    else if (colName == "created_date")
                    {
                        queryOption.SortBy.Add(a => a.CreatedDate);
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

        public User MapModelToEntity(UserEntryViewModel vm, User user)
        {
            user.ID = vm.Id;
            user.name = vm.name;
            user.email = vm.email;
            user.username = vm.username;
            user.role_id = vm.role_id;
            user.ministry_id = vm.ministry_id;
            user.parent_id = vm.parent_id;
            user.profile_img = vm.profile_img;
            if (vm.Id == 0)
            {
                user.status = true;
            }
            else{
                if (user.status)
                {
                    user.status = user.status;
                }
            }
                       
            if (vm.location_township > 0)
            {
                user.location_id = vm.location_township;
            }
            if(vm.location_township==0 && vm.location_district > 0)
            {
                user.location_id = vm.location_district;
            }
            if(vm.location_township==0 && vm.location_district == 0 && vm.location_state > 0)
            {
                user.location_id = vm.location_state;
            }
            if (vm.Id <= 0)
            {
                user.password = PasswordHashHelper.HashPassword(vm.password);
            }
            return user;
        }

        public UserEntryViewModel MapEntityToModel(UserEntryViewModel vm, User user)
        {
            vm.Id = user.ID;
            vm.name = user.name;
            vm.email = user.email;
            vm.username = user.username;
            vm.password = user.password;
            vm.role_id = user.role_id;
            vm.ministry_id = user.ministry_id;
            if (user.ParentUser != null)
            {
                vm.parent_minid = user.ParentUser.parent_id;
            }            
            vm.parent_id = user.parent_id;
            if (user.Location != null)
            {
                if(user.Location.Location_Type== (int)LocationType.StateDivision)
                {
                    vm.location_state = user.location_id;
                }
                if (user.Location.Location_Type == (int)LocationType.District)
                {
                    vm.location_district = user.location_id;
                    vm.location_state = (int)user.Location.Parent_Id;
                }
                if (user.Location.Location_Type == (int)LocationType.Township)
                {
                    vm.location_township = user.location_id;
                    vm.location_district = (int)user.Location.Parent_Id;
                    if (user.Location.ParentLocation != null)
                    {
                        vm.location_state = (int)user.Location.ParentLocation.Parent_Id;
                    }                    
                }
            }
            vm.profile_img = user.profile_img;
            vm.status = user.status;
            return vm;
        }

        public User MapUserToPasswordReset(UserEntryViewModel vm, User user)
        {
            user.password = PasswordHashHelper.HashPassword(vm.new_password);
            return user;
        }

        public JQueryDataTablePagedResult<UserListViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<User> user)
        {
            JQueryDataTablePagedResult<UserListViewModel> model = new JQueryDataTablePagedResult<UserListViewModel>();
            foreach (var res in user.data)
            {
                //int userid = (int)res.CreatedBy;
                UserListViewModel vm = new UserListViewModel();
                vm.id = res.ID;
                vm.name = res.name;
                vm.username = res.username;
                vm.email = res.email;
                vm.status = res.status;
                if (res.status) { vm.message = "Active"; }
                else { vm.message = "Suspended"; }
                if (res.Ministry != null) { vm.parent_ministry = res.Ministry.name; }                
                if (res.ParentUser != null) { vm.parent_cpu = res.ParentUser.name + "/" + res.ParentUser.username; }
                vm.created_date = string.Format("{0:dd-MM-yyyy hh-mm-ss}", res.CreatedDate);
                if (res.role_id == 2)
                {
                    if (res.Ministry != null) { vm.name = res.Ministry.name; }
                    vm.parent_ministry = "";
                }
                model.data.Add(vm);
            }
            model.recordsFiltered = user.recordsFiltered;
            model.recordsTotal = user.recordsTotal;
            return model;
        }
    }
}
