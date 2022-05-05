using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;
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
    public class ServiceViewController : BaseController
    {
        IDepartmentRepository deptRepo;
        IDepartmentService deptService;
        DepartmentMapper mapper;
        IServiceRepository srepo;
        IServiceService serv;
        ServiceMapper smapper;
        public ServiceViewController(IDepartmentService _deptService, IDepartmentRepository _deptRepo,
            IServiceRepository srepo, IServiceService serv
            )
            : base(typeof(ServiceViewController), ProgramCodeEnum.SERVICE_VIEW)
        {
            this.deptRepo = _deptRepo;
            this.deptService = _deptService;
            this.srepo = srepo;
            this.serv = serv;
            mapper = new DepartmentMapper();
            smapper = new ServiceMapper();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<DepartmentViewModel> vmlist = GetAllData();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.SERVICE_VIEW));
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Department>());
            }
        }

        public JQueryDataTablePagedResult<DepartmentViewModel> GetAllData()
        {
            int minid = GetMinistryId();
            NY.Framework.Infrastructure.JqueryDataTableQueryOptions<Department> queryoption = GetJQueryDataTableQueryOptions<Department>();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption);

            queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, (a => a.IsDeleted == false && a.Ministry_Id.Equals(minid)));
            
            NY.Framework.Infrastructure.Pagination.JQueryDataTablePagedResult<Department> list = deptRepo.GetPagedResults(queryoption);
            JQueryDataTablePagedResult<DepartmentViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return vmlist;
        }

        [HttpGet]
        [Route("GetAllService")]
        public JsonResult GetAllService()

        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<ServicesViewModel> vmlist = GetAllDataService();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.SERVICE_VIEW));
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Service>());
            }
        }
        public JQueryDataTablePagedResult<ServicesViewModel> GetAllDataService()
        {
            //int minid = GetMinistryId();
            JqueryDataTableQueryOptions<Service> queryOption = GetJQueryDataTableQueryOptions<Service>();
            queryOption = smapper.Preparetorepository(queryOption);
            if (queryOption.FilterBy == null)
            {
                queryOption.FilterBy = (a => a.IsDeleted == false && a.Department.Ministry_Id == GetMinistryId());
            }
            else
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (a => a.IsDeleted == false && a.Department.Ministry_Id == GetMinistryId()));
            }

            JQueryDataTablePagedResult<Service> list = srepo.GetPagedResults(queryOption);
            JQueryDataTablePagedResult<ServicesViewModel> vmlist = smapper.MapModelToListViewModel(list);
            return vmlist;

        }
    }
}
