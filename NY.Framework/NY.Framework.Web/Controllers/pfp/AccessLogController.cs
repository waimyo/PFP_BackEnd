using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Logging;
using NY.Framework.Infrastructure.Pagination;
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
    public class AccessLogController : BaseController
    {
        IView_AccessLogRepository Repo;
        AccessLogMapper mapper;
        Logger logger;
        public AccessLogController(IView_AccessLogRepository _Repo)
            : base(typeof(AccessLogController), ProgramCodeEnum.ACCESS_LOGS)
        {
            this.Repo = _Repo;
            mapper = new AccessLogMapper();
            logger = new Logger(typeof(AccessLogController));
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                try
                {
                    JQueryDataTablePagedResult<AccessLogViewModel> vmlist = GetAllData();
                    return Json(vmlist);
                }
                catch (Exception ex)
                {
                    logger.Log(ex);
                    return Json(GetAccessDeniedJsonResult<View_AccessLog>());
                }

            }
            else
            {
                return Json(GetAccessDeniedJsonResult<View_AccessLog>());
            }
        }

        public JQueryDataTablePagedResult<AccessLogViewModel> GetAllData()
        {
            AccessLogViewModel model = new AccessLogViewModel();
            model.fromdate = Request.Query["fromdate"].ToString();
            model.todate = Request.Query["todate"].ToString();
            NY.Framework.Infrastructure.JqueryDataTableQueryOptions<View_AccessLog> queryoption = GetJQueryDataTableQueryOptions<View_AccessLog>();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption);

            NY.Framework.Infrastructure.Pagination.JQueryDataTablePagedResult<View_AccessLog> list = Repo.GetProcedurePagedResults(queryoption, "exec SP_AccessLog {0},{1}", new object[] { model.fromdate, model.todate });
            JQueryDataTablePagedResult<AccessLogViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return vmlist;
        }


    }
}
