using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Application.Services;
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
    public class ServiceController : BaseController
    {
        IServiceRepository repo;
        IMinistryRepository minRepo;
        IDepartmentRepository deptRepo;
        IServiceService serv;
        ServiceMapper mapper;

        public ServiceController(IServiceRepository repo, IServiceService serv, IMinistryRepository minRepo,
        IDepartmentRepository deptRepo
 )
         : base(typeof(ServiceController), Application.ProgramCodeEnum.SERVICES)
        {
            this.repo = repo;
            this.serv = serv;
            this.minRepo = minRepo;
            this.deptRepo = deptRepo;
            mapper = new ServiceMapper();
        }



        // GET: api/<controller>
        [HttpGet]
        public JsonResult GetAll()

        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<ServicesViewModel> vmlist = GetAllData();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.SERVICES));
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Service>());
            }
        }
        public JQueryDataTablePagedResult<ServicesViewModel> GetAllData()
        {
            JqueryDataTableQueryOptions<Service> queryOption = GetJQueryDataTableQueryOptions<Service>();
           
            queryOption = mapper.PreparetorepositoryForService(queryOption, Search());
            if (queryOption.FilterBy == null)
            {
                queryOption.FilterBy = (a => a.IsDeleted == false);
            }
            else
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (a => a.IsDeleted == false));
            }

            JQueryDataTablePagedResult<Service> list = repo.GetPagedResults(queryOption);
            JQueryDataTablePagedResult<ServicesViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return vmlist;

        }
        private ServicesViewModel Search()
        {
            ServicesViewModel model = new ServicesViewModel();
            string ministry_id = Request.Query["ministry_id"].ToString();
            if (!string.IsNullOrEmpty(ministry_id))
            {
                model.ministry_id = Convert.ToInt32(ministry_id);
            }
            return model;
        }
        
        // POST api/<controller>
        [HttpPost]
        public IActionResult SaveOrUpdate(ServicesViewModel viewModel)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    User user = GetLoggedInUser();
                    CommandResult<Service> result = new CommandResult<Service>();
                    Service serve = new Service();
                    if (viewModel.id > 0)
                    {
                        serve = repo.Get(viewModel.id);
                        serve = mapper.MapEntityToViewModel(serve, viewModel);
                        serve.ModifiedBy = user.ID;
                        serve.ModifiedDate = DateTime.Now;
                        result = serv.CreateOrUpdate(serve);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.UPDATE), nameof(ProgramCodeEnum.SERVICES));
                        }
                    }
                    else
                    {
                        serve = mapper.MapEntityToViewModel(serve, viewModel);
                        serve.CreatedBy = user.ID;
                        serve.CreatedDate = DateTime.Now;
                        result = serv.CreateOrUpdate(serve);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.ADD), nameof(ProgramCodeEnum.SERVICES));
                        }
                    }
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(GetServerJsonResult<Service>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Service>());
            }
        }


        [HttpGet]
        [Route("GetById")]
        public JsonResult GetById(int id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                Service cate = repo.Get(id);
                ServicesViewModel vm = mapper.MapEntityToViewModel(cate);
                return Json(vm);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Service>());
            }
        }
        [HttpGet]
        [Route("ExcelExport")]
        public IActionResult ExcelExport()
        {
            if (Authorize(AuthorizeAction.PRINT))
            {
                JQueryDataTablePagedResult<ServicesViewModel> vmlist = GetAllData();
                vmlist.data=vmlist.data.OrderBy(d=>d.ministry_name).ThenBy(d=>d.department_name).ThenBy(d=>d.name).ToList();
               // vmlist.data=vmlist.data.OrderBy(d =>d.department_name).ToList();
                const string contentType = "application/octet-stream";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("attachment", "Content-Disposition");
                NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 12);
                excel.AddHeader("Service");
                excel.AddColumn("No", typeof(string));
                excel.AddColumn("PARENT MINISTRY", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("PARENT DEPARTMENT", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("NAME", typeof(string), NPOIExcelColumnWidth.M3);
                int count = 0;
                ConvertToMyanmarNumber convertcls = new ConvertToMyanmarNumber();
                foreach (var res in vmlist.data)
                {
                    count++;
                    excel.AddRow();
                    excel.SetData(0, convertcls.ConvertToMyanmarNo(count.ToString()));
                    excel.SetData(1, res.ministry_name);
                    excel.SetData(2, res.department_name);
                    excel.SetData(3, res.name);
                }
                byte[] bytes = excel.Generate();
                
                var fileContentResult = new FileContentResult
                (bytes, contentType)
                {
                    FileDownloadName = "Excel.xls"
                };

                AuditLog(nameof(AuditAction.PRINT), nameof(ProgramCodeEnum.SERVICES));
                return fileContentResult;
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Service>());
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            CommandResult<Service> result = new CommandResult<Service>();
            if (Authorize(AuthorizeAction.DELETE))
            {
                Service services = repo.Get(id);
                if (services != null)
                {
                    result = serv.Delete(services);

                    if (result.Success)
                    {
                        AuditLog(nameof(AuditAction.DELETE), nameof(ProgramCodeEnum.SERVICES));
                    }
                }
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Service>());
            }
        }
    }
}

