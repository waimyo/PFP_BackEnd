using System;
using System.Collections.Generic;
using System.Linq;
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
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;
using static NY.Framework.Web.Models.CampaignDetailViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.pfp
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsListController : BaseController
    {
        CampaignMapper mapper;
        ICampaignsRepository campaignRepository;
        IGroupMemberRepository groupMemberRepository;
        ISmsRepository smsRepository;
        ICampaignDetailListStoreProcedureRepository campaignDetailListRepository;
        ICampaignService campaignService;
        ICategorizationStatusStoreProcedureRepository categorizationStatusRepository;

        public CampaignsListController(ICampaignsRepository _campaignRepository, ICampaignService _campaignService,
           IGroupMemberRepository _groupMemberRepository, ISmsRepository _smsRepository,
            ICampaignDetailListStoreProcedureRepository _campaignDetailListRepository,
            ICategorizationStatusStoreProcedureRepository _categorizationStatusRepository)
            : base(typeof(CampaignsListController), ProgramCodeEnum.CAMPAIGN_LIST)
        {
            campaignService = _campaignService;
            campaignRepository = _campaignRepository;
            groupMemberRepository = _groupMemberRepository;
            smsRepository = _smsRepository;
            campaignDetailListRepository = _campaignDetailListRepository;
            categorizationStatusRepository = _categorizationStatusRepository;
            mapper = new CampaignMapper();
        }

        //GET: api/<controller>
        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                CampaignListFilterViewModel filtervm = GetFilterForCampaignList();
                JQueryDataTablePagedResult<CampaignListViewModel> vmlist = GetAllData(filtervm);
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.CAMPAIGN_LIST));
                return Json(vmlist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Campaigns>());
            }

        }

        [HttpGet]
        [Route("Detail/")]
        public JsonResult Detail()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                int campid = Convert.ToInt32(Request.Query["id"].FirstOrDefault());
                Campaigns camp = campaignRepository.Get(campid);
                CampaignDetailViewModel detailvm = mapper.MapModelToDetailViewModel(camp);

                #region GetTotalParticipantCountForCampaign
                List<GroupMember> groupmembers = groupMemberRepository.GetGroupMembersByGroupId(camp.Group_Id);
                if (groupmembers != null)
                {
                    detailvm.TotalParticipantCount = groupmembers.Count;
                }
                #endregion GetTotalParticipantCountForCampaign

                #region GetCategorizedResponseCount
                int categorized_responsecount = smsRepository.GetCategorizedResponseCountByCampaignId(campid);
                detailvm.CategorizedResponseCount = categorized_responsecount;
                #endregion GetCategorizedResponseCount

                #region GetUnCategorizedResponseCount
                int uncategorized_responsecount = smsRepository.GetUnCategorizedResponseCountByCampaignId(campid);
                detailvm.UncategorizedResponseCount = uncategorized_responsecount;
                #endregion GetUnCategorizedResponseCount

                #region GetTotalResponseCount
                detailvm.ResponseCount = smsRepository.GetTotalResponseCountByCampaignId(campid);
                #endregion GetTotalResponseCount

                #region GetTotalResponsePercent              
                double responsePercent = ((double)detailvm.ResponseCount / (double)detailvm.TotalParticipantCount) * 100;
                detailvm.ResponsePercent = Math.Round(responsePercent);
                #endregion GetTotalResponsePercent

                #region GetNotReplyCount
                detailvm.NotReplyCount = detailvm.TotalParticipantCount - detailvm.ResponseCount;
                #endregion GetNotReplyCount

                #region GetCategorizationStatus
                List<CategorizationStatusStoreProcedure> categorizationstatuslist = categorizationStatusRepository.GetCategorizationStatusByCampaignId(campid);
                #endregion GetCategorizationStatus

                #region GetCampaignDetailList
                var camplist = GetCampaignDetailList(campid);
                #endregion GetCampaignDetailList
                
                return Json(new { detail = detailvm, camplist, categorizationstatuslist }, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });

            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Campaigns>());
            }
        }

        [HttpGet]
        [Route("ExcelExportForDetailList/")]
        public IActionResult ExcelExportForDetailList()
        {
            if (Authorize(AuthorizeAction.PRINT))
            {
                int id = Convert.ToInt32(Request.Query["id"].FirstOrDefault());
                #region GetRouteParametersFromRequestQuery
                //  BasicInfoListViewModel vm = GetRouteParameters();
                #endregion GetRouteParametersFromRequestQuery
                //vm.MinistryId = GetMinistryId();
                JQueryDataTablePagedResult<CampaignDetailListViewModel> vmlist = GetCampaignDetailList(id);

                const string contentType = "application/octet-stream";
                HttpContext.Response.ContentType = contentType;
                HttpContext.Response.Headers.Add("attachment", "Content-Disposition");

                NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Myanmar3", 13);
                excel.AddHeader("Public Feedback Program");
                excel.AddColumn("စဉ်", typeof(string));
                excel.AddColumn("အမည်", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("မိုဘိုင်းဖုန်းနံပါတ်", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ဌာန/အဖွဲ့အစည်း", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ကျား/မ", typeof(string), NPOIExcelColumnWidth.M1);
                excel.AddColumn("တုန့်ပြန်မှုအမျိုးအစား", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ပြန်စာ", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ပေးပို့ခဲ့သည့် မက်ဆေ့ချ်", typeof(string), NPOIExcelColumnWidth.M3);
                excel.AddColumn("ပြန်စာပေးပို့သည့်အချိန်", typeof(string), NPOIExcelColumnWidth.M3);

                int count = 0;
                ConvertToMyanmarNumber convertcls = new ConvertToMyanmarNumber();
                foreach (var res in vmlist.data)
                {
                    count++;
                    excel.AddRow();
                    excel.SetData(0, convertcls.ConvertToMyanmarNo(count.ToString()));
                    excel.SetData(1, res.Name);
                    excel.SetData(2, res.Mobile);
                    excel.SetData(3, res.DepartmentName);
                    excel.SetData(4, res.Gender);
                    excel.SetData(5, res.CategorizedResponse);
                    excel.SetData(6, res.ResponseMessage);
                    excel.SetData(7, res.SmsMessage);
                    excel.SetData(8, res.ResponseMessageTime);
                }

                byte[] bytes = excel.Generate();
                var fileContentResult = new FileContentResult
                (bytes, contentType)
                {
                    FileDownloadName = "Public Feedback Program.xls"
                };
                AuditLog(nameof(AuditAction.PRINT), nameof(ProgramCodeEnum.CAMPAIGN_LIST));
                return fileContentResult;
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Campaigns>());
            }

        }

        public JQueryDataTablePagedResult<CampaignListViewModel> GetAllData(CampaignListFilterViewModel filtervm)
        {
            JqueryDataTableQueryOptions<Campaigns> queryOption = GetJQueryDataTableQueryOptions<Campaigns>();
            mapper.PrepareQueryOptionForRepository(queryOption, filtervm);
            User loginuser = GetLoggedInUser();
            //if login_user is cpu acc
             if (loginuser.role_id == 3)
            {
                queryOption.FilterBy1 = x => x.CreatedBy.Equals(loginuser.ID);
                queryOption.FilterBy1 = LinqExpressionHelper.AppendOr(queryOption.FilterBy1, x => x.CreatedByUser.parent_id.Equals(loginuser.ID));
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, queryOption.FilterBy1);
            }
             //if login_user is ministry acc
            if (loginuser.role_id == 2)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, x => x.CreatedByUser.ministry_id==loginuser.ministry_id);
            }
            JQueryDataTablePagedResult<Campaigns> list = campaignRepository.GetPagedResults(queryOption);
            JQueryDataTablePagedResult<CampaignListViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return vmlist;
        }

        public JQueryDataTablePagedResult<CampaignDetailListViewModel> GetCampaignDetailList(int campid)
        {
            JqueryDataTableQueryOptions<CampaignDetailListStoreProcedure> queryOption = GetJQueryDataTableQueryOptions<CampaignDetailListStoreProcedure>();
            mapper.PrepareQueryOptionForDetailListRepository(queryOption);
            JQueryDataTablePagedResult<CampaignDetailListStoreProcedure> camplist = campaignDetailListRepository.GetProcedurePagedResultsAsNoTracking(queryOption, "exec CampaignDetailList {0}", new object[] { campid });
            return mapper.MapModelToDetailListViewModel(camplist);
        }

        public CampaignListFilterViewModel GetFilterForCampaignList()
        {
            CampaignListFilterViewModel vm = new CampaignListFilterViewModel();
            vm.Name = Request.Query["Name"].FirstOrDefault();
            string campaignstatus = Request.Query["Status"].FirstOrDefault();
            if (!string.IsNullOrEmpty(campaignstatus))
            {
                vm.Status = Convert.ToBoolean(campaignstatus);
            }
            vm.SmsMessage = Request.Query["SmsMessage"].FirstOrDefault();
            vm.GroupNo = Request.Query["GroupNo"].FirstOrDefault();
            vm.MinistryId = Convert.ToInt32(Request.Query["MinistryId"].FirstOrDefault());
            vm.CreatedUserId = Convert.ToInt32(Request.Query["CreatedUserId"].FirstOrDefault());
            vm.CreatedDateFrom = Request.Query["CreatedDateFrom"].FirstOrDefault();
            vm.CreatedDateTo = Request.Query["CreatedDateTo"].FirstOrDefault();

            return vm;
        }

    }
}
