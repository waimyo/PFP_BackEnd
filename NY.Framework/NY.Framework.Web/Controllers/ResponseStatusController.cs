using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseStatusController : BaseController
    {
        ResponseStatusMapper mapper;
        IResponseStatusRepository repo;
        public ResponseStatusController(IResponseStatusRepository _repo) : base(typeof(ResponseStatusController), ProgramCodeEnum.RESPONSE_STATS)
        {
            mapper = new ResponseStatusMapper();
            this.repo = _repo;
        }


        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<ResponseStatus> vmlist = GetAllData();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.RESPONSE_STATS));
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<ResponseStatus>());
            }
        }

        public JQueryDataTablePagedResult<ResponseStatus> GetAllData()
        {
            NY.Framework.Infrastructure.JqueryDataTableQueryOptions<ResponseStatus> queryoption = GetJQueryDataTableQueryOptions<ResponseStatus>();
            var vm = MapSearchData();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption, vm);
            NY.Framework.Infrastructure.Pagination.JQueryDataTablePagedResult<ResponseStatus> list = repo.GetProcedurePagedResults(queryoption, "exec SP_Response_Status {0},{1},{2}", new object[] { vm.ministry_id, vm.fromdate, vm.todate });
          //  JQueryDataTablePagedResult<ResponseStatusListViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return list;
        }

        private ResponseStatusSearchViewModel MapSearchData()
        {
            ResponseStatusSearchViewModel model = new ResponseStatusSearchViewModel();
          

            model.fromdate = Request.Query["fromdate"].ToString();
            model.todate = Request.Query["todate"].ToString();
            string ministry_id = Request.Query["ministry_id"].ToString();
            if (!string.IsNullOrEmpty(ministry_id))
            {
                model.ministry_id = Convert.ToInt32(ministry_id);
            }

          
            return model;
        }



        [HttpGet]
        [Route("ExcelExport/")]
        public IActionResult ExcelExport()
        {
            if (Authorize(AuthorizeAction.VIEW))
            { 
              //  JQueryDataTablePagedResult<ResponseStatus> vmlist = GetAllData();

            const string contentType = "application/octet-stream";
            HttpContext.Response.ContentType = contentType;
            HttpContext.Response.Headers.Add("attachment", "Content-Disposition");

            Infrastructure.Utilities.NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 13);
            excel.AddHeader("Response Stats");

            excel.AddColumn("No", typeof(string), NPOIExcelColumnWidth.S2);
            excel.AddColumn("Account", typeof(string), NPOIExcelColumnWidth.M2);
            excel.AddColumn("Name", typeof(string), NPOIExcelColumnWidth.M2);
            excel.AddColumn("Role", typeof(string), NPOIExcelColumnWidth.M2);
            excel.AddColumn("Send", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("Received", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("Response Rate", typeof(string), NPOIExcelColumnWidth.M1);
          
            int count = 0;
                var vm = MapSearchData();
                List<ResponseStatus> data = new List<ResponseStatus>();
                data = repo.getResponseStats(vm.ministry_id, vm.fromdate, vm.todate);

                int send_total = 0;
                int receive_total = 0;
           

                if(data.Count > 0)
                {
                    foreach(var result in data)
                    {
                        if(result.sms_send_count > 0)
                        {
                            send_total += result.sms_send_count;
                        }
                        if (result.sms_receive_count > 0)
                        {
                            receive_total += result.sms_receive_count;
                        }
                      
                    }

                    ResponseStatus rs = new ResponseStatus();
                    rs.name = "Total";
                    if(send_total > 0)
                    {
                        rs.sms_send_count = send_total;
                    }
                    if(receive_total > 0)
                    {
                        rs.sms_receive_count = receive_total;
                    }

                    if (send_total > 0 && receive_total > 0)
                    {
                        rs.response_rate = Math.Round((double)receive_total /send_total * 100);
                    }
                    data.Add(rs);
                }

                foreach (var res in data)
            {
                count++;
                excel.AddRow();
                excel.SetData(0, count);
                excel.SetData(1, res.name);
                excel.SetData(2, res.username);
                excel.SetData(3, res.role_name);
                excel.SetData(4, res.sms_send_count);
                excel.SetData(5, res.sms_receive_count);
                excel.SetData(6, res.response_rate+"%");
                
            }

            byte[] bytes = excel.Generate();
            var fileContentResult = new FileContentResult
            (bytes, contentType)
            {
                FileDownloadName = "Excel.xls"
            };

                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.RESPONSE_STATS));
                return fileContentResult;
        }
            else
            {
                return Json(GetAccessDeniedJsonResult<ResponseStatus>());
    }
}

    }
}
