using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.pfp
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseController
    {
        IDepartmentRepository deptRepo;
        IDepartmentService deptService;
        DepartmentMapper mapper;
        IMinistryRepository minRepo;
        public DepartmentController(IDepartmentService _deptService, IDepartmentRepository _deptRepo
            , IMinistryRepository _minRepo)
            : base(typeof(DepartmentController), ProgramCodeEnum.DEPARTMENT)
        {
            this.deptRepo = _deptRepo;
            this.deptService = _deptService;
            this.minRepo = _minRepo;
            mapper = new DepartmentMapper();
        }

        [HttpGet]
        [Route("GetMinistry/")]
        public JsonResult GetMinistry()
        {
            List<Ministry> list = minRepo.GetMinistryList();
            return Json(list);
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<DepartmentViewModel> vmlist = GetAllData();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.DEPARTMENT));
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Department>());
            }
        }

        public JQueryDataTablePagedResult<DepartmentViewModel> GetAllData()
        {
            NY.Framework.Infrastructure.JqueryDataTableQueryOptions<Department> queryoption = GetJQueryDataTableQueryOptions<Department>();
            queryoption = mapper.PrepareQueryOptionForRepositoryForMin(queryoption,Search());

            if (queryoption.FilterBy == null)
            {
                queryoption.FilterBy = (a => a.IsDeleted == false);
            }
            else
            {
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, (a => a.IsDeleted == false));
            }

            NY.Framework.Infrastructure.Pagination.JQueryDataTablePagedResult<Department> list = deptRepo.GetPagedResults(queryoption);
            JQueryDataTablePagedResult<DepartmentViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return vmlist;
        }
        private DepartmentViewModel Search()
        {
            DepartmentViewModel model = new DepartmentViewModel();
            string ministry_id = Request.Query["ministry_id"].ToString();
            if (!string.IsNullOrEmpty(ministry_id))
            {
                model.ministry_id = Convert.ToInt32(ministry_id);
            }
            return model;
        }

        [HttpPost]
        public IActionResult SaveOrUpdate(DepartmentViewModel entrymodel)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    User user = GetLoggedInUser();
                    CommandResult<Department> result = new CommandResult<Department>();
                    Department dept = new Department();
                    if (entrymodel.id > 0)
                    {
                        dept = deptRepo.Get(entrymodel.id);
                        dept = mapper.MapModelToEntity(dept, entrymodel);
                        dept.ModifiedBy = user.ID;
                        dept.ModifiedDate = DateTime.Now;
                        result = deptService.CreateOrUpdate(dept);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.UPDATE), nameof(ProgramCodeEnum.DEPARTMENT));
                        }
                    }
                    else
                    {
                        dept = mapper.MapModelToEntity(dept, entrymodel);
                        dept.CreatedBy = user.ID;
                        dept.CreatedDate = DateTime.Now;
                        result = deptService.CreateOrUpdate(dept);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.ADD), nameof(ProgramCodeEnum.DEPARTMENT));
                        }
                    }
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(GetServerJsonResult<Department>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Department>());
            }
        }
        [HttpGet]
        [Route("ExcelExport")]
        public IActionResult ExcelExport()
        {
            if (Authorize(AuthorizeAction.PRINT))
            {
                JQueryDataTablePagedResult<DepartmentViewModel> vmlist = GetAllData();
                vmlist.data = vmlist.data.OrderBy(d => d.ministry_name).ThenBy(d => d.name).ToList();
                const string contentType = "application/octet-stream";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("attachment", "Content-Disposition");
                NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 12);
                excel.AddHeader("Department");
                excel.AddColumn("No", typeof(string));
                excel.AddColumn("PARENT MINISTRY", typeof(string), NPOIExcelColumnWidth.M5);
                excel.AddColumn("DEPARTMENT NAME", typeof(string), NPOIExcelColumnWidth.M2);
                int count = 0;
                ConvertToMyanmarNumber convertcls = new ConvertToMyanmarNumber();
                foreach (var res in vmlist.data)
                {
                    count++;
                    excel.AddRow();
                    excel.SetData(0, convertcls.ConvertToMyanmarNo(count.ToString()));
                    excel.SetData(1, res.ministry_name);
                    excel.SetData(2, res.name);
                }
                byte[] bytes = excel.Generate();

                var fileContentResult = new FileContentResult
                (bytes, contentType)
                {
                    FileDownloadName = "Excel.xls"
                };

                AuditLog(nameof(AuditAction.PRINT), nameof(ProgramCodeEnum.DEPARTMENT));
                return fileContentResult;
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Department>());
            }
        }


        [HttpGet]
        [Route("GetById")]
        public JsonResult GetById(int id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                Department dept = deptRepo.Get(id);
                return Json(dept);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Department>());
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            CommandResult<Department> result = new CommandResult<Department>();
            if (Authorize(AuthorizeAction.DELETE))
            {
                Department dept = deptRepo.Get(id);
                if (dept != null)
                {
                    result = deptService.Delete(dept);

                    if (result.Success)
                    {
                        AuditLog(nameof(AuditAction.DELETE), nameof(ProgramCodeEnum.DEPARTMENT));
                    }
                }
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Department>());
            }
        }
    }
}
