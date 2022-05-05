using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities.Home;
using NY.Framework.Model.Repositories.Home;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.Home
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsCountByOperatorController : BaseController
    {
        ISmsCountByOperatorRepository smsRepo;

        public SmsCountByOperatorController(ISmsCountByOperatorRepository _smsRepo)
         : base(typeof(SmsCountByOperatorController), Application.ProgramCodeEnum.Dashboard)
        {
            this.smsRepo = _smsRepo;
        }

        [HttpGet]
        [Route("GetSmsCount")]
        public JsonResult GetSmsCount(string fromdate,string todate,int ministry_acc_id, int user_id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
             //   x => x.createduser.id == minaccid || x.createduser.parentid == minaccid
               
                List<SmsCountByOperator> result = new List<SmsCountByOperator>();
                result = smsRepo.getSmsCount(fromdate,todate,ministry_acc_id, user_id);

                //int mpt_sms_in_total = 0;
                //int mpt_sms_out_total = 0;
                //int telenor_sms_in_total = 0;
                //int telenor_sms_out_total = 0;
                //int mytel_sms_in_total = 0;
                //int mytel_sms_out_total = 0;
                //int ooredoo_sms_in_total = 0;
                //int ooredoo_sms_out_total = 0;
                //if (result.Count > 0)
                //{
                //    foreach(var data in result)
                //    {
                //        if (data.mpt_sms_in > 0)
                //        {

                //            mpt_sms_in_total += data.mpt_sms_in;
                //        }
                //        if (data.mpt_sms_out > 0)
                //        {
                //            mpt_sms_out_total += data.mpt_sms_out;
                //        }
                //        if (data.telenor_sms_in > 0)
                //        {
                //           telenor_sms_in_total += data.telenor_sms_in;
                //        }
                //        if (data.telenor_sms_out > 0)
                //        {
                //            telenor_sms_out_total += data.telenor_sms_out;
                //        }

                //        if (data.ooredoo_sms_in > 0)
                //        {
                //            ooredoo_sms_in_total += data.ooredoo_sms_in;
                //        }
                //        if (data.ooredoo_sms_out > 0)
                //        {
                //            ooredoo_sms_out_total += data.ooredoo_sms_out;
                //        }

                //        if (data.mytel_sms_in > 0)
                //        {
                //            mytel_sms_in_total += data.mytel_sms_in;
                //        }
                //        if (data.ooredoo_sms_out > 0)
                //        {
                //            mytel_sms_out_total += data.mytel_sms_out;
                //        }

                //    }
                //    SmsCountByOperator sm = new SmsCountByOperator();
                //    sm.month = "Total";

                //    if (mpt_sms_in_total > 0)
                //    {
                //        sm.mpt_sms_in = mpt_sms_in_total;
                //    }
                //    if (mpt_sms_out_total > 0)
                //    {
                //        sm.mpt_sms_out = mpt_sms_out_total;
                //    }

                //    if (telenor_sms_in_total > 0)
                //    {
                //        sm.telenor_sms_in = telenor_sms_in_total;
                //    }
                //    if (telenor_sms_out_total > 0)
                //    {
                //        sm.telenor_sms_out = telenor_sms_out_total;
                //    }

                //    if (ooredoo_sms_in_total > 0)
                //    {
                //        sm.ooredoo_sms_in = ooredoo_sms_in_total;
                //    }
                //    if (ooredoo_sms_out_total > 0)
                //    {
                //        sm.ooredoo_sms_out = ooredoo_sms_out_total;
                //    }

                //    if (mytel_sms_in_total > 0)
                //    {
                //        sm.mytel_sms_in = mytel_sms_in_total;
                //    }
                //    if (mytel_sms_out_total > 0)
                //    {
                //        sm.mytel_sms_out = mytel_sms_out_total;
                //    }
                //    result.Add(sm);
                //}
               AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.Dashboard));
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<SmsCountByOperator>());
            }
        }

        [HttpGet]
        [Route("ExcelExport")]
        public IActionResult ExcelExport(string fromdate,string todate, int ministry_id, int user_id)
        {
            if (Authorize(AuthorizeAction.PRINT))
            {
                //string y = year;
                const string contentType = "application/octet-stream";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("attachment", "Content-Disposition");

                NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 12);

                excel.AddHeader("Telecom Operator တစ်ခုချင်းစီအလိုက် SMS အရေအတွက် ");
            
                string[] lst = {"No",  "Month",   "MPT", "",  "Ooredoo", "", "Telenor", "", "Mytel" };
                excel.AddMultipleHeader(lst);

              
                excel.AddColumn("", typeof(string), NPOIExcelColumnWidth.S4);
                excel.AddColumn("", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("Incoming", typeof(string), NPOIExcelColumnWidth.S4);
                excel.AddColumn("Outgoing", typeof(string), NPOIExcelColumnWidth.S4);

                excel.AddColumn("Incoming", typeof(string), NPOIExcelColumnWidth.S4);
                excel.AddColumn("Outgoing", typeof(string), NPOIExcelColumnWidth.S4);

                excel.AddColumn("Incoming", typeof(string), NPOIExcelColumnWidth.S4);
                excel.AddColumn("Outgoing", typeof(string), NPOIExcelColumnWidth.S4);

                excel.AddColumn("Incoming", typeof(string), NPOIExcelColumnWidth.S4);
                excel.AddColumn("Outgoing", typeof(string), NPOIExcelColumnWidth.S4);

                int no = 0;
                List<SmsCountByOperator> result = new List<SmsCountByOperator>();
                result = smsRepo.getSmsCount(fromdate,todate, ministry_id, user_id);
                int mpt_sms_in_total = 0;
                int mpt_sms_out_total = 0;
                int telenor_sms_in_total = 0;
                int telenor_sms_out_total = 0;
                int mytel_sms_in_total = 0;
                int mytel_sms_out_total = 0;
                int ooredoo_sms_in_total = 0;
                int ooredoo_sms_out_total = 0;
                if (result.Count > 0)
                {
                    foreach (var data in result)
                    {
                        if (data.mpt_sms_in > 0)
                        {

                            mpt_sms_in_total += data.mpt_sms_in;
                        }
                        if (data.mpt_sms_out > 0)
                        {
                            mpt_sms_out_total += data.mpt_sms_out;
                        }
                        if (data.telenor_sms_in > 0)
                        {
                            telenor_sms_in_total += data.telenor_sms_in;
                        }
                        if (data.telenor_sms_out > 0)
                        {
                            telenor_sms_out_total += data.telenor_sms_out;
                        }

                        if (data.ooredoo_sms_in > 0)
                        {
                            ooredoo_sms_in_total += data.ooredoo_sms_in;
                        }
                        if (data.ooredoo_sms_out > 0)
                        {
                            ooredoo_sms_out_total += data.ooredoo_sms_out;
                        }

                        if (data.mytel_sms_in > 0)
                        {
                            mytel_sms_in_total += data.mytel_sms_in;
                        }
                        if (data.ooredoo_sms_out > 0)
                        {
                            mytel_sms_out_total += data.mytel_sms_out;
                        }

                    }
                    SmsCountByOperator sm = new SmsCountByOperator();
                    sm.month = "Total";

                    if (mpt_sms_in_total > 0)
                    {
                        sm.mpt_sms_in = mpt_sms_in_total;
                    }
                    if (mpt_sms_out_total > 0)
                    {
                        sm.mpt_sms_out = mpt_sms_out_total;
                    }

                    if (telenor_sms_in_total > 0)
                    {
                        sm.telenor_sms_in = telenor_sms_in_total;
                    }
                    if (telenor_sms_out_total > 0)
                    {
                        sm.telenor_sms_out = telenor_sms_out_total;
                    }

                    if (ooredoo_sms_in_total > 0)
                    {
                        sm.ooredoo_sms_in = ooredoo_sms_in_total;
                    }
                    if (ooredoo_sms_out_total > 0)
                    {
                        sm.ooredoo_sms_out = ooredoo_sms_out_total;
                    }

                    if (mytel_sms_in_total > 0)
                    {
                        sm.mytel_sms_in = mytel_sms_in_total;
                    }
                    if (mytel_sms_out_total > 0)
                    {
                        sm.mytel_sms_out = mytel_sms_out_total;
                    }
                    result.Add(sm);
                }
                foreach (var res in result)
                {
                    
                    no++;
                    excel.AddRow();
                   excel.SetData(0, no);
                    excel.SetData(1, res.month);
                    excel.SetData(2, res.mpt_sms_in);
                    excel.SetData(3, res.mpt_sms_out);
                    excel.SetData(4, res.ooredoo_sms_in);
                    excel.SetData(5, res.ooredoo_sms_out);
                    excel.SetData(6, res.telenor_sms_in);
                    excel.SetData(7, res.telenor_sms_out);
                    excel.SetData(8, res.mytel_sms_in);
                    excel.SetData(9, res.mytel_sms_out);


                }
                
                byte[] bytes = excel.Generate();
                var fileContentResult = new FileContentResult
                (bytes, contentType)
                {
                    FileDownloadName = "Excel.xls"
                };
                AuditLog(nameof(AuditAction.PRINT), nameof(ProgramCodeEnum.Dashboard));
                return fileContentResult;
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<SmsCountByOperator>());
            }
        }
    }

}
