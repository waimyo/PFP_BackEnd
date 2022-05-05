using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.pfp
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : BaseController
    {
        LocationMapper mapper;
        ILocationRepository locationRepo;
        ILocationService locService;
        IView_LocationRepository vlocationRepo;

        public LocationController(ILocationRepository _locationRepo, ILocationService _locService, IView_LocationRepository _vlocationRepo)
            : base(typeof(LocationController), ProgramCodeEnum.LOCATION)
        {
            mapper = new LocationMapper();
            locationRepo = _locationRepo;
            locService = _locService;
            this.vlocationRepo = _vlocationRepo;
        }

        public JQueryDataTablePagedResult<LocationViewModel> GetAllData(LocationFilterViewModel vm)
        {
            JqueryDataTableQueryOptions<Location> queryoption = GetJQueryDataTableQueryOptions<Location>();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption, vm);
            JQueryDataTablePagedResult<Location> loclist = locationRepo.GetPagedResults(queryoption);
            JQueryDataTablePagedResult<LocationViewModel> vmlist = mapper.MapModelToListViewModel(loclist);

            return vmlist;
        }

        // GET: api/<controller>
        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                LocationFilterViewModel vm = new LocationFilterViewModel();
                vm.SearchValue = Request.Query["search"].FirstOrDefault();
                vm.LocationType = Convert.ToInt32(Request.Query["locationtype"].FirstOrDefault());
                JQueryDataTablePagedResult<LocationViewModel> vmlist = GetAllData(vm);
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.LOCATION));
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Location>());
            }

        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("GetById/")]
        public JsonResult GetById(int id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                Location loc = locationRepo.Get(id);
                LocationViewModel vm = mapper.MapModelToEntryViewModel(loc);
                return Json(vm, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Location>());
            }


        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("GetAllStateDivision")]
        //public JsonResult GetAllStateDivision()
        //{           
        //    List<Location> statedivlist = locationRepo.GetAllStateDivision();
        //    return Json(statedivlist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });           
        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("GetAllDistrict")]
        //public JsonResult GetAllDistrict(int statedivid)
        //{          
        //    List<Location> districtlist = locationRepo.GetAllDistrictByStateDivisonId(statedivid);
        //    return Json(districtlist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });          
        //}

        // POST api/<controller>
        [HttpPost]
        public IActionResult SaveOrUpdate(LocationViewModel vm)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    CommandResult<Location> result = new CommandResult<Location>();
                    if (vm.Id > 0)
                    {
                        Location loc = locationRepo.Get(vm.Id);
                        mapper.MapEntryViewModelToModel(loc, vm);
                        loc.ModifiedDate = DateTime.Now;
                        User user = GetLoggedInUser();
                        loc.ModifiedBy = user.ID;
                        result = locService.CreateOrUpdate(loc);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.UPDATE), nameof(ProgramCodeEnum.LOCATION));
                        }
                    }
                    else
                    {
                        Location loc = new Location();
                        loc = mapper.MapEntryViewModelToModel(loc, vm);
                        User user = GetLoggedInUser();
                        loc.CreatedBy = user.ID;
                        loc.CreatedDate = DateTime.Now;
                        loc.IsDeleted = false;
                        result = locService.CreateOrUpdate(loc);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.ADD), nameof(ProgramCodeEnum.LOCATION));
                        }
                    }
                    return Json(result);
                }
                catch
                {
                    return Json(GetServerJsonResult<Ministry>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Location>());
            }

        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            if (Authorize(AuthorizeAction.DELETE))
            {
                CommandResult<Location> result = new CommandResult<Location>();
                Location loc = locationRepo.Get(id);
                if (loc != null)
                {
                    result = locService.Delete(loc);
                    if (result.Success)
                    {
                        AuditLog(nameof(AuditAction.DELETE), nameof(ProgramCodeEnum.LOCATION));
                    }
                }
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Location>());
            }
        }



        [HttpGet]
        [Route("ExcelExportForLocation/")]
        public IActionResult ExcelExportForLocation()
        {
            if (Authorize(AuthorizeAction.PRINT))
            {
                List<View_Location> vmlist = new List<View_Location>();
                vmlist = vlocationRepo.GetLocaion();
                const string contentType = "application/octet-stream";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("attachment", "Content-Disposition");

                NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Myanmar3", 13);
                excel.AddHeader("Location");
                excel.AddColumn("စဉ်", typeof(string));
                excel.AddColumn("တိုင်းဒေသကြီး/ပြည်နယ်", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ခရိုင်", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("မြို့နယ်", typeof(string), NPOIExcelColumnWidth.M3);

                int count = 0;
                ConvertToMyanmarNumber convertcls = new ConvertToMyanmarNumber();
                foreach (var res in vmlist)
                {
                    count++;
                    excel.AddRow();
                    excel.SetData(0, convertcls.ConvertToMyanmarNo(count.ToString()));
                    excel.SetData(1, res.state_division);
                    excel.SetData(2, res.district);
                    excel.SetData(3, res.township);
                }

                byte[] bytes = excel.Generate();
                var fileContentResult = new FileContentResult
                (bytes, contentType)
                {
                    FileDownloadName = "Location.xls"
                };
                AuditLog(nameof(AuditAction.PRINT), nameof(ProgramCodeEnum.LOCATION));
                return fileContentResult;
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Location>());
            }

        }


    }
}
