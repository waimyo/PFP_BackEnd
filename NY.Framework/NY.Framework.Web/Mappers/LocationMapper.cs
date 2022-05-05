using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Enumerations;
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
    public class LocationMapper
    {
        public JqueryDataTableQueryOptions<Location> PrepareQueryOptionForRepository(JqueryDataTableQueryOptions<Location> queryOption, LocationFilterViewModel vm)
        {
            queryOption.FilterBy = (l=>l.IsDeleted==false);

            if (!string.IsNullOrEmpty(vm.SearchValue)&&vm.LocationType>0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, 
                    l =>( l.Name.Contains(vm.SearchValue) || l.Pcode.Contains(vm.SearchValue)) 
                    && l.Location_Type == vm.LocationType);               
            }
            else if (!string.IsNullOrEmpty(vm.SearchValue) && vm.LocationType==0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                   l => (l.Name.Contains(vm.SearchValue) || l.Pcode.Contains(vm.SearchValue)));
            }
            else if (string.IsNullOrEmpty(vm.SearchValue) && vm.LocationType> 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
                   l => l.Location_Type == vm.LocationType);
            }
           
            if (queryOption.SortColumnsName.Count > 0)
            {
                foreach (string colName in queryOption.SortColumnsName)
                {
                    queryOption.SortBy = new List<Func<Location, object>>();
                  
                    if (colName == "name")
                    {
                        queryOption.SortBy.Add((l => l.Name));
                    }
                    else if (colName == "code")
                    {
                        queryOption.SortBy.Add((l => l.Pcode));
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

        //for list page
        public JQueryDataTablePagedResult<LocationViewModel> MapModelToListViewModel(JQueryDataTablePagedResult<Location> loclist)
        {
            JQueryDataTablePagedResult<LocationViewModel> vmlist = new JQueryDataTablePagedResult<LocationViewModel>();
            foreach (var loc in loclist.data)
            {
                LocationViewModel vm = new LocationViewModel();
                vm.Id = loc.ID;
                vm.LocationType = loc.Location_Type;
                if (loc.Location_Type == (int)LocationType.StateDivision)
                {
                    vm.StateDivisionName = loc.Name;
                    vm.StateDivisionCode = loc.Pcode;
                }
                else if (loc.Location_Type == (int)LocationType.District)
                {
                    vm.DistrictName = loc.Name;
                    vm.DistrictCode = loc.Pcode;
                    if (loc.ParentLocation != null)
                    {
                        vm.StateDivisionName = loc.ParentLocation.Name;
                        vm.StateDivisionCode = loc.ParentLocation.Pcode;
                    }
                }
                else if (loc.Location_Type == (int)LocationType.Township)
                {
                    vm.TownshipName = loc.Name;
                    vm.TownshipCode = loc.Pcode;
                    if (loc.ParentLocation != null)
                    {
                        vm.DistrictName = loc.ParentLocation.Name;
                        vm.DistrictCode = loc.ParentLocation.Pcode;
                        vm.DistrictId = loc.ParentLocation.ID;
                        if (loc.ParentLocation.ParentLocation != null)
                        {
                            vm.StateDivisionName = loc.ParentLocation.ParentLocation.Name;
                            vm.StateDivisionCode = loc.ParentLocation.ParentLocation.Pcode;
                        }
                    }
                }
                vmlist.data.Add(vm);
            }
            vmlist.recordsFiltered = loclist.recordsFiltered;
            vmlist.recordsTotal = loclist.recordsTotal;
            return vmlist;
        }

        //for save or update
        public Location MapEntryViewModelToModel(Location loc, LocationViewModel vm)
        {
            loc.Location_Type = vm.LocationType;
            if (vm.LocationType == (int)LocationType.StateDivision)
            {
                loc.Name = vm.StateDivisionName;
                loc.Pcode = vm.StateDivisionCode;
            }
            else if (vm.LocationType == (int)LocationType.District)
            {
                loc.Name = vm.DistrictName;
                loc.Pcode = vm.DistrictCode;
                loc.Parent_Id = vm.StateDivisionId;
            }
            else if (vm.LocationType == (int)LocationType.Township)
            {
                loc.Name = vm.TownshipName;
                loc.Pcode = vm.TownshipCode;
                loc.Parent_Id = vm.DistrictId;
            }

            return loc;
        }

        //for edit view
        public LocationViewModel MapModelToEntryViewModel(Location loc)
        {
            LocationViewModel vm = new LocationViewModel();
            vm.Id = loc.ID;
            vm.LocationType = loc.Location_Type;
            if (vm.LocationType == (int)LocationType.StateDivision)
            {
                vm.StateDivisionName = loc.Name;
                vm.StateDivisionCode = loc.Pcode;
            }
            else if (vm.LocationType == (int)LocationType.District)
            {
                vm.DistrictName = loc.Name;
                vm.DistrictCode = loc.Pcode;
                vm.StateDivisionId =Convert.ToInt32(loc.Parent_Id);
            }
            else if (vm.LocationType == (int)LocationType.Township)
            {
                vm.TownshipName = loc.Name;
                vm.TownshipCode = loc.Pcode;
                vm.DistrictId = Convert.ToInt32(loc.Parent_Id);
                vm.StateDivisionId = loc.ParentLocation.ParentLocation.ID;
            }
            return vm;
        }
    }
}
