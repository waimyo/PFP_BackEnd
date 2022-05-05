using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;

namespace NY.Framework.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MinistryController : BaseController
    {
        MinistryMapper mapper;
        IMinistryService service;
        IMinistryRepository repo;
        IFileSystemService fileService;

        public MinistryController(IMinistryService _service, IMinistryRepository _repo, IFileSystemService _fileService) : base(typeof(MinistryController), ProgramCodeEnum.MINISTRY)
        {
            mapper = new MinistryMapper();
            this.service = _service;
            this.repo = _repo;
            this.fileService = _fileService;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<MinistryViewModel> vmlist = GetAllData();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.MINISTRY));
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Ministry>());
            }
        }

        public JQueryDataTablePagedResult<MinistryViewModel> GetAllData()
        {
            NY.Framework.Infrastructure.JqueryDataTableQueryOptions<Ministry> queryoption = GetJQueryDataTableQueryOptions<Ministry>();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption);
            if (queryoption.FilterBy == null)
            {
                queryoption.FilterBy = (a => a.IsDeleted == false);
            }
            else
            {
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, (a => a.IsDeleted == false));
            }
            NY.Framework.Infrastructure.Pagination.JQueryDataTablePagedResult<Ministry> list = repo.GetPagedResults(queryoption);
            JQueryDataTablePagedResult<MinistryViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return vmlist;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult SaveOrUpdate([FromForm]MinistryViewModel entrymodel)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    User user = GetLoggedInUser();
                    CommandResult<Ministry> result = new CommandResult<Ministry>();
                    Ministry entity = new Ministry();
                    Guid guId = Guid.NewGuid();
                    string logoname = guId.ToString();
                    
                    if (entrymodel.id > 0)
                    {
                        entity = repo.Get(entrymodel.id);
                        string oldlogo = entity.logo;
                        entity.name = entrymodel.name;
                        if (entrymodel.logo != null) { entity.logo = logoname; }
                        entity.ModifiedBy = user.ID;
                        entity.ModifiedDate = DateTime.Now;
                        result = service.CreateOrUpdate(entity);
                        if (result.Success & oldlogo != null)
                        {
                            fileService.RemoveFile(NY.Framework.Infrastructure.Constants.MinistryLogoPath + oldlogo);
                        }
                    }
                    else
                    {
                        entity.name = entrymodel.name;
                        if (entrymodel.logo != null) { entity.logo = logoname; }
                        entity.CreatedBy = user.ID;
                        entity.CreatedDate = DateTime.Now;
                        result = service.CreateOrUpdate(entity);
                    }
                    if (result.Success)
                    {
                        if (entrymodel.logo != null)
                        {
                            fileService.SaveFile(entrymodel.logo, NY.Framework.Infrastructure.Constants.MinistryLogoPath, logoname);
                        }
                        AuditLog(nameof(AuditAction.UPDATE), nameof(ProgramCodeEnum.MINISTRY));
                    }
                    //for ministry logo update in front end
                    User loginuser=userRepo.Get(user.ID);
                    string base64Image = "", ministryname="";
                    if (loginuser != null)
                    {
                        ministryname = loginuser.Ministry.name;
                        string filepath = NY.Framework.Infrastructure.Constants.MinistryLogoPath + loginuser.Ministry.logo;
                        if (System.IO.File.Exists(filepath))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(filepath);
                            base64Image = Convert.ToBase64String(imageArray);
                        }
                    }
                   
                   
                    return Json(new { result, base64Image, ministryname });
                }
                catch (Exception ex)
                {
                    return Json(GetServerJsonResult<Ministry>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Ministry>());
            }
        }
        [HttpGet]
        [Route("ExcelExport")]
        public IActionResult ExcelExport()
        {
            if (Authorize(AuthorizeAction.PRINT))
            {
                JQueryDataTablePagedResult<MinistryViewModel> vmlist = GetAllData();
                const string contentType = "application/octet-stream";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("attachment", "Content-Disposition");
                NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 12);
                excel.AddHeader("Ministry");
                excel.AddColumn("No", typeof(string));
                excel.AddColumn("ဝန်ကြီးဌာနအမည်", typeof(string), NPOIExcelColumnWidth.L3);
                int count = 0;
                ConvertToMyanmarNumber convertcls = new ConvertToMyanmarNumber();
                foreach (var res in vmlist.data)
                {
                    count++;
                    excel.AddRow();
                    excel.SetData(0, convertcls.ConvertToMyanmarNo(count.ToString()));
                    excel.SetData(1, res.name);
                }
                byte[] bytes = excel.Generate();

                var fileContentResult = new FileContentResult
                (bytes, contentType)
                {
                    FileDownloadName = "Excel.xls"
                };

                AuditLog(nameof(AuditAction.PRINT), nameof(ProgramCodeEnum.MINISTRY));
                return fileContentResult;
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Ministry>());
            }
        }
        [HttpGet]
        [Route("GetById")]
        public JsonResult GetById(int id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                Ministry entity = repo.Get(id);
                MinistryViewModel vm = new MinistryViewModel();
                vm.name = entity.name;
                vm.id = entity.ID;
                if (!string.IsNullOrEmpty(entity.logo))
                {
                    vm.imagebyte = fileService.GetFile(NY.Framework.Infrastructure.Constants.MinistryLogoPath, entity.logo);
                }
                return Json(vm);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Ministry>());
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            CommandResult<Ministry> result = new CommandResult<Ministry>();
            if (Authorize(AuthorizeAction.DELETE))
            {
                Ministry entity = repo.Get(id);
                if (entity != null)
                {
                    result = service.Delete(entity);

                    if (result.Success)
                    {
                        AuditLog(nameof(AuditAction.DELETE), nameof(ProgramCodeEnum.MINISTRY));
                    }
                }
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Ministry>());
            }
        }
    }
}
