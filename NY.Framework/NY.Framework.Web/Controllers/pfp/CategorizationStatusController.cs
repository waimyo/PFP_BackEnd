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

namespace NY.Framework.Web.Controllers.pfp
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorizationStatusController : BaseController
    {
        CategorizationStatusMapper mapper;
        ICategorizationStatusRepository repo;

        public CategorizationStatusController(ICategorizationStatusRepository _repo)
         : base(typeof(CategorizationStatusController), Application.ProgramCodeEnum.CATEGORIZATION_STATS)
        {
            mapper = new CategorizationStatusMapper();
            this.repo = _repo;
        }


        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<CategorizationStatus> vmlist = GetAllData();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.CATEGORIZATION_STATS));
                
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<CategorizationStatus>());
            }
        }

        public JQueryDataTablePagedResult<CategorizationStatus> GetAllData()
        {
            NY.Framework.Infrastructure.JqueryDataTableQueryOptions<CategorizationStatus> queryoption = GetJQueryDataTableQueryOptions<CategorizationStatus>();
            var vm = MapSearchData();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption,vm );
            NY.Framework.Infrastructure.Pagination.JQueryDataTablePagedResult<CategorizationStatus> list = repo.GetProcedurePagedResults(queryoption, "exec SP_Categorization_Status {0},{1},{2}", new object[] { vm.ministry_id, vm.fromdate, vm.todate });
            // JQueryDataTablePagedResult<CategorizationStatusListViewModel> vmlist = mapper.MapModelToListViewModel(list);
          //  List<CategorizationStatus> result = repo.getCategorization(vm.ministry_id, vm.fromdate, vm.todate);

            //int satisfied_t = 0;
            //int dissatisfied_t = 0;
            //int suggestion_t = 0;
            //int appreciation_t = 0;
            //int not_relevant_t = 0;
            //int corruption_t = 0;
            //int other_t = 0;
            //int charge_t = 0;
            //int no_charge_t = 0;
            //int categorized_count_t = 0;
            //int non_categorized_count_t = 0;
            //int? grand_total_t = 0;
            //if(result > 0)
            //{
            //    foreach(var res in result)
            //    {
            //        if(res.satisfied > 0)
            //        {
            //            satisfied_t += res.satisfied;
            //        }
            //    }
            //    CategorizationStatus cs = new CategorizationStatus();
            //    cs.name = "Total";

            //    if (satisfied_t > 0)
            //    {
            //        cs.satisfied = satisfied_t;
            //    }
            //    result.Add(cs);

            //}

            return list;
        }

        private CategorizationStatusSearchViewModel MapSearchData()
        {
            CategorizationStatusSearchViewModel model = new CategorizationStatusSearchViewModel();


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
                //   JQueryDataTablePagedResult<CategorizationStatus> vmlist = GetAllData();
              
                const string contentType = "application/octet-stream";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("attachment", "Content-Disposition");

                Infrastructure.Utilities.NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 13);
                excel.AddHeader("Categorization Stats");

                excel.AddColumn("No", typeof(string));
                excel.AddColumn("Account", typeof(string), NPOIExcelColumnWidth.M2);
                excel.AddColumn("Name", typeof(string), NPOIExcelColumnWidth.M2);
                excel.AddColumn("Role", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Satisfied", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Disatisfied", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Suggestion", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Appreciation", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Not Relevant", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Corruption", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Other", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Charge", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("No Charge", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Categorized", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Uncategorized", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Grand Total", typeof(string), NPOIExcelColumnWidth.M1);

                int count = 0;
                var vm = MapSearchData();
                List<CategorizationStatus> data = new List<CategorizationStatus>();
                data = repo.getCategorization(vm.ministry_id, vm.fromdate, vm.todate);

                int satisfied_total = 0;
                int dissatisfied_total = 0;
                int suggestion_total = 0;
                int appreciation_total = 0;
                int not_relevant_total = 0;
                int corruption_total = 0;
                int other_total = 0;
                int charge_total = 0;
                int no_charge_total = 0;
                int categorized_count_total = 0;
                int non_categorized_count_total = 0;
                int gran_total = 0;

                if (data.Count > 0)
                {
                    foreach(var result in data)
                    {
                        if(result.satisfied > 0)
                        {
                            satisfied_total += result.satisfied;
                        }

                        if (result.dissatisfied > 0)
                        {
                            dissatisfied_total += result.dissatisfied;
                        }

                        if (result.suggestion > 0)
                        {
                            suggestion_total += result.suggestion;
                        }
                        if (result.appreciation > 0)
                        {
                            appreciation_total += result.appreciation;
                        }
                        if (result.not_relevant > 0)
                        {
                            not_relevant_total += result.not_relevant;
                        }
                        if (result.corruption > 0)
                        {
                            corruption_total += result.corruption;
                        }
                        if (result.other > 0)
                        {
                            other_total += result.other;
                        }
                        if (result.charge > 0)
                        {
                            charge_total += result.charge;
                        }
                        if (result.no_charge > 0)
                        {
                            no_charge_total += result.no_charge;
                        }
                        if (result.categorized_count > 0)
                        {
                            categorized_count_total += result.categorized_count;
                        }
                        if (result.non_categorized_count > 0)
                        {
                            non_categorized_count_total += result.non_categorized_count;
                        }
                        if (result.grand_total > 0)
                        {
                            gran_total += result.grand_total;
                        }
                    }

                    CategorizationStatus cs = new CategorizationStatus();
                    cs.name = "Total";
                    if (satisfied_total > 0)
                    {
                       cs.satisfied = satisfied_total;
                    }

                    if (dissatisfied_total > 0)
                    {
                        cs.dissatisfied = dissatisfied_total;
                    }

                    if (suggestion_total > 0)
                    {
                        cs.suggestion = suggestion_total;
                    }
                    if (appreciation_total > 0)
                    {
                        cs.appreciation = appreciation_total;
                    }
                    if (not_relevant_total > 0)
                    {
                        cs.not_relevant = not_relevant_total;
                    }
                    if (corruption_total  > 0)
                    {
                        cs.corruption = corruption_total;
                    }
                    if (other_total > 0)
                    {
                        cs.other = other_total;
                    }
                    if (charge_total > 0)
                    {
                        cs.charge = charge_total;
                    }
                    if (no_charge_total > 0)
                    {
                        cs.no_charge = no_charge_total;
                    }
                    if (categorized_count_total > 0)
                    {
                        cs.categorized_count = categorized_count_total;
                    }
                    if (non_categorized_count_total > 0)
                    {
                        cs.non_categorized_count = non_categorized_count_total;
                    }
                    if (gran_total > 0)
                    {
                        cs.grand_total = gran_total;
                    }
                    data.Add(cs);
                }

                foreach (var res in data)
                {
                    count++;
                    excel.AddRow();
                    excel.SetData(0, count);
                    excel.SetData(1, res.name);
                    excel.SetData(2, res.username);
                    excel.SetData(3, res.role_name);
                    excel.SetData(4, res.satisfied);
                    excel.SetData(5, res.dissatisfied);
                    excel.SetData(6, res.suggestion);
                    excel.SetData(7, res.appreciation);
                    excel.SetData(8, res.not_relevant);
                    excel.SetData(9, res.corruption);
                    excel.SetData(10, res.other);
                    excel.SetData(11, res.charge);
                    excel.SetData(12, res.no_charge);
                    excel.SetData(13, res.categorized_count);
                    excel.SetData(14, res.non_categorized_count);
                    //    excel.SetData(15,  res.categorized_count + res.non_categorized_count+ res.appreciation+ res.not_relevant
                    //   + res.corruption+ res.other+ res.charge + res.no_charge+res.satisfied + res.dissatisfied + res.suggestion );
                    excel.SetData(15, res.grand_total);
                }

                byte[] bytes = excel.Generate();
                var fileContentResult = new FileContentResult
                (bytes, contentType)
                {
                    FileDownloadName = "Excel.xls"
                };

                AuditLog(nameof(AuditAction.PRINT), nameof(ProgramCodeEnum.CATEGORIZATION_STATS));
                return fileContentResult;
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<ResponseStatus>());
            }
        }

        //[HttpGet]
        //[Route("GetCategorization")]
        //public JsonResult GetCategorization(int ministry_id, string fromdate, string todate)
        //{
        //    if (Authorize(AuthorizeAction.VIEW))
        //    {
        //        List<CategorizationStatus> result = new List<CategorizationStatus>();
        //        result = repo.getCategorization(ministry_id, fromdate, todate);

        //        return Json(result);
        //    }
        //    else
        //    {
        //        return Json(GetAccessDeniedJsonResult<CategorizationStatus>());
        //    }
        //}

        //[HttpGet]
        //[Route("ExcelExport")]
        //public IActionResult ExcelExport(int ministry_id, string fromdate, string todate)
        //{
        //    if (Authorize(AuthorizeAction.PRINT))
        //    {
        //        const string contentType = "application/octet-stream";
        //        HttpContext.Response.ContentType = contentType;
        //        HttpContext.Response.Headers.Add("attachment", "Content-Disposition");

        //        NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 12);

        //        excel.AddHeader("Categorization Status");
        //        excel.AddColumn("No", typeof(string));
        //        excel.AddColumn("Account", typeof(string), NPOIExcelColumnWidth.M2);
        //        excel.AddColumn("Name", typeof(string), NPOIExcelColumnWidth.M2);
        //        excel.AddColumn("Role", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Satisfied", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Disatisfied", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Suggestion", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Appreciation", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Not Relevant", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Corruption", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Other", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Charge", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("No Charge", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Categorized", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Uncategorized", typeof(string), NPOIExcelColumnWidth.M1);
        //        excel.AddColumn("Grand Total", typeof(string), NPOIExcelColumnWidth.M1);
        //        int no = 0;

        //        List<CategorizationStatus> data = new List<CategorizationStatus>();
        //        data = repo.getCategorization(ministry_id, fromdate, todate);
                
        //            foreach (var res in data)
        //            {
        //                no++;
        //                excel.AddRow();
        //                excel.SetData(0, no);
        //                excel.SetData(1, res.name);
        //                excel.SetData(2, res.username);
        //                excel.SetData(3, res.role_name);
        //                excel.SetData(4, res.satisfied);
        //                excel.SetData(5, res.dissatisfied);
        //            excel.SetData(6, res.suggestion);
        //            excel.SetData(7, res.appreciation);
        //            excel.SetData(8, res.not_relevant);
        //            excel.SetData(9, res.corruption);
        //            excel.SetData(10, res.other);
        //            excel.SetData(11, res.charge);
        //            excel.SetData(12, res.no_charge);
        //            excel.SetData(13, res.categorized_count);
        //            excel.SetData(14, res.non_categorized_count);
        //            excel.SetData(15, res.categorized_count+res.non_categorized_count);
        //        }

                

        //        byte[] bytes = excel.Generate();
        //        var fileContentResult = new FileContentResult
        //        (bytes, contentType)
        //        {
        //            FileDownloadName = "Excel.xls"
        //        };
        //     //   AuditLog(AuditAction.PRINT, Constants.PCodeFor_TotalReportByReportingEntity);
        //        return fileContentResult;
        //    }
        //    else
        //    {
        //        return Json(GetAccessDeniedJsonResult<CategorizationStatus>());
        //    }
        //}


    }
}
