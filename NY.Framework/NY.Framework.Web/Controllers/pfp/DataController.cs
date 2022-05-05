using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

namespace NY.Framework.Web.Controllers.pfp
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : BaseController
    {
        DataMapper mapper;
        IDataService service;
        IDataRepository repo;
        ILocationRepository locationRepo;
        IView_DataInfoRepository viewdatainfoRepo;

        public DataController(IDataService _service, IDataRepository _repo, ILocationRepository _locationRepo, IView_DataInfoRepository _viewdatainfoRepo) 
            : base(typeof(DataController), ProgramCodeEnum.DATA)
        {
            mapper = new DataMapper();
            this.service = _service;
            this.repo = _repo;
            this.locationRepo = _locationRepo;
            this.viewdatainfoRepo = _viewdatainfoRepo;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.DATA));
                JQueryDataTablePagedResult<DataListViewModel> vmlist = GetAllData();                
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Data>());
            }
        }

        public JQueryDataTablePagedResult<DataListViewModel> GetAllData()
        {
            JqueryDataTableQueryOptions<View_DataInfo> queryoption = GetJQueryDataTableQueryOptions<View_DataInfo>();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption, MapSearchData());
            queryoption = GetQueryOption(queryoption);
            JQueryDataTablePagedResult<View_DataInfo> list = viewdatainfoRepo.GetPagedResults(queryoption);
            JQueryDataTablePagedResult<DataListViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return vmlist;
        }

        private DataSearchViewModel MapSearchData()
        {
            DataSearchViewModel model = new DataSearchViewModel();
            model.gender = Request.Query["gender"].ToString();
            model.fromdate = Request.Query["fromdate"].ToString();
            model.todate = Request.Query["todate"].ToString();
            model.name = Request.Query["name"];
            model.service = Request.Query["service"];
            string ministry_id = Request.Query["ministry_id"].ToString();            
            if (!string.IsNullOrEmpty(ministry_id))
            {
                model.ministry_id = Convert.ToInt32(ministry_id);
                if (model.ministry_id > 0)
                {
                    model.ministry_id = userRepo.Get(model.ministry_id).ministry_id;
                }                
            }
            string user_id = Request.Query["user_id"].ToString();
            if (!string.IsNullOrEmpty(user_id))
            {
                model.user_id = Convert.ToInt32(user_id);
            }
            return model;
        }

        private JqueryDataTableQueryOptions<View_DataInfo> GetQueryOption(JqueryDataTableQueryOptions<View_DataInfo> queryoption)
        {
            
            User loginuser = GetLoggedInUser();
            if (loginuser.role_id == 4)
            {
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, x => x.userid.Equals(loginuser.ID));
            }
            else if (loginuser.role_id == 3)
            {
                queryoption.FilterBy1 = x => x.userid.Equals(loginuser.ID);
                queryoption.FilterBy1 = LinqExpressionHelper.AppendOr(queryoption.FilterBy1, x => x.parentuserid.Equals(loginuser.ID));
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, queryoption.FilterBy1);
            }
            else if (loginuser.role_id == 2)
            {
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, x => x.ministry_id.Equals(loginuser.ministry_id));
            }
            return queryoption;
        }

        [HttpPost]
        public IActionResult SaveOrUpdate(DataViewModel entrymodel)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    CommandResult<Data> result = new CommandResult<Data>();
                    Data entity = new Data();
                    if(entrymodel.location_district==0 && entrymodel.location_township > 0)
                    {
                        entrymodel.location_district= (Int32)locationRepo.Get(entrymodel.location_township).Parent_Id;
                    }
                    if (entrymodel.id > 0)
                    {
                        entity = repo.Get(entrymodel.id);
                        //entity = mapper.MapModelToEntity(entity, entrymodel);
                        entity.name = entrymodel.name;
                        entity.ModifiedBy = GetLoggedInId();
                        entity.ModifiedDate = DateTime.Now;
                        result = service.CreateOrUpdate(entity);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.UPDATE), nameof(ProgramCodeEnum.DATA));
                        }
                    }
                    else
                    {
                        entity = mapper.MapModelToEntity(entity, entrymodel);
                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = GetLoggedInId();
                        result = service.CreateOrUpdate(entity);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.ADD), nameof(ProgramCodeEnum.DATA));
                        }
                    }
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(GetServerJsonResult<Data>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Data>());
            }
        }
        
        [HttpGet]
        [Route("DataInfoExcelExport")]
        public IActionResult ExcelExport()
        {
            if (Authorize(AuthorizeAction.PRINT))
            {
                #region Get Data List With Filter

                JqueryDataTableQueryOptions<View_DataInfo> queryoption = GetJQueryDataTableQueryOptions<View_DataInfo>();
                queryoption = mapper.PrepareQueryOptionForRepository(queryoption, MapSearchData());
                queryoption = GetQueryOption(queryoption);
                List<View_DataInfo> list = viewdatainfoRepo.GetListWithFilter(queryoption);
                List<DataListViewModel> vmlist = mapper.MapModelToListViewModel(list);
              
                
                #endregion

                #region Map Excel Data
                const string contentType = "application/octet-stream";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("attachment", "Content-Disposition");
                NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 12);
                excel.AddHeader("Data Portal");
                excel.AddColumn("ID", typeof(string));
                excel.AddColumn("အမည်", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("ဖုန်းနံပါတ်", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ကျား/မ", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ဝန်ကြီးဌာန", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ဦးစီးဌာန", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ဝန်ဆောင်မှုအမျိုးအစား", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ပြည်နယ်/တိုင်းဒေသကြီး", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ခရိုင်", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("မြို့နယ်", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ဝန်ဆောင်မှုရယူရန် ရုံးသို့လာရောက်သည့်နေ့", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ဝန်ဆောင်မှုရရှိခဲ့သည့်နေ့", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ပေးပို့သူ", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("အချက်အလက်များတင်သွင်းသည့်နေ့", typeof(string), NPOIExcelColumnWidth.M3);
                foreach (var res in vmlist)
                {
                    excel.AddRow();
                    excel.SetData(0, res.id);
                    excel.SetData(1, res.name);
                    excel.SetData(2, res.mobile);
                    excel.SetData(3, res.gender);
                    excel.SetData(4, res.ministry);
                    excel.SetData(5, res.department);
                    excel.SetData(6, res.service);
                    excel.SetData(7, res.statedivision);
                    excel.SetData(8, res.district);
                    excel.SetData(9, res.township);
                    excel.SetData(10, res.date_of_application);
                    excel.SetData(11, res.date_of_completion);
                    excel.SetData(12, res.created_by);
                    excel.SetData(13, res.created_date);
                }
                byte[] bytes = excel.Generate();
                var fileContentResult = new FileContentResult
                (bytes, contentType)
                {
                    FileDownloadName = "Excel.xls"
                };
                AuditLog(nameof(AuditAction.PRINT), nameof(ProgramCodeEnum.DATA));
                return fileContentResult;
                #endregion
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Data>());
            }
        }
    }
}
