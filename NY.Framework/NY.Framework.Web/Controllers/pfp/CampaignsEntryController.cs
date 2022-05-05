using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Logging;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;
using RestSharp;
using RestSharp.Authenticators;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.pfp
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsEntryController : BaseController
    {
        ICampaignsRepository campaignRepo;
        IGroupMemberRepository groupMemberRepo;
        CampaignMapper mapper;
        ICampaignService campaignService;
        ISmsService smsService;
        SmsMapper smsMapper;
        IGroupRepository groupRepo;
        Logger logger;

        
        
        RestSharpClient restsharpclient = new RestSharpClient();
        SmsCounter smsCounter;


        public CampaignsEntryController(ICampaignService _campaignService,
            IGroupMemberRepository _groupMemberRepo, IGroupRepository _groupRepo,
        ICampaignsRepository _campaignRepo, ISmsService _smsService
            )
            : base(typeof(CampaignsEntryController), ProgramCodeEnum.CAMPAIGN_ENTRY)
        {
            mapper = new CampaignMapper();
            campaignService = _campaignService;
            groupMemberRepo = _groupMemberRepo;
            groupRepo = _groupRepo;
            campaignRepo = _campaignRepo;
            smsService = _smsService;
            smsMapper = new SmsMapper();
            logger = new Logger(typeof(CampaignsEntryController));
            smsCounter = new SmsCounter();
        }

        [AllowAnonymous]
        [HttpGet, HttpPost]
        [Route("SendSmsMptAndTelenor")]
        public string SendSmsMptAndTelenor([FromQuery] SmsEntryViewModel vm)
        {
            try
            {
                logger.LogDebug("Inside SendSmsMptAndTelenor " + vm.Operator + vm.DataInfoId + vm.CampaignId + vm.SmsText);
                //#region SaveSms
                //CommandResult<Sms> smsresult = new CommandResult<Sms>();
                //Sms sms = new Sms();

                //sms.IsDeleted = false;

                //string[] arr = vm.Operator.Split('?');
                //sms.Operator = arr[0];
                //string message_status = arr[1];

                //sms.Direction = 1;
                //sms.Sms_Code_Id = vm.SmsCodeId;
                //logger.LogDebug("operator" + sms.Operator + " status " + message_status + "cid" + sms.Sms_Code_Id);
                //if (vm.DataInfoId != 0)
                //{
                //    sms.DataInfo_Id = vm.DataInfoId;
                //}
                //sms.Sms_Time =DateTime.Now;
                //sms.Sms_Text = vm.SmsText;
                //if (vm.CampaignId != 0)
                //{
                //    sms.Campaign_Id = vm.CampaignId;
                //}
                //sms.Message_Type =(int)MessageType.Main_Feedback_SMS;


                //smsCounter.GetSmsCount(sms);
                ////smscounter.getcount(sms)
                //sms.CreatedBy = GetLoggedInId();
                //sms.CreatedDate = DateTime.Now;

                //if (message_status.Contains("ESME_ROK"))
                //{
                //    sms.Sms_Sent_Status = "Success";
                //}
                //else
                //{
                //    sms.Sms_Sent_Status = "Fail";
                //}
                //logger.LogInfo("Saving Campaign SMS..........");
                //smsresult = smsService.CreateOrUpdateCommand(sms);
                //#endregion SaveSms                   
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }


            return "ACK/Jasmin";
        }

        //for sms outbound
        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(CampaignEntryViewModel campaignvm)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
            {
                    User createby = GetLoggedInUser();
                    CommandResult<Campaigns> result = new CommandResult<Campaigns>();
                if (campaignvm.Id <= 0)
                {
                    #region SaveCampaign
                    Campaigns campaign = new Campaigns();
                    mapper.MapEntryViewModelToModel(campaign, campaignvm);                   
                    campaign.CreatedBy =createby.ID;
                    campaign.CreatedDate = DateTime.Now;
                    campaign.IsDeleted = false;
                    result = campaignService.CreateOrUpdateCommand(campaign);
                    AuditLog(nameof(AuditAction.ADD), nameof(ProgramCodeEnum.CAMPAIGN_ENTRY));
                    #endregion SaveCampaign

                    if (result.Success)
                    {
                        #region GetMobielList
                        List<GroupMember> memberlist = groupMemberRepo.GetGroupMembersByGroupId(result.Result[0].Group_Id);
                        #endregion GetMobielList

                        foreach (var m in memberlist)
                        {
                            #region MaptoSmsEntryViewModel
                            SmsEntryViewModel smsvm = new SmsEntryViewModel();
                            smsvm.Direction = (int)SmsDirection.Sent;
                            smsvm.SmsCodeId = campaignvm.SmsCodeId;
                            smsvm.DataInfoId = m.datainfo_id;
                                string formattedmessage = campaignvm.SmsMessage.Replace("#name#", m.data.name)
                                                                           .Replace("#ministry#", m.data.Ministry.name)
                                                                            .Replace("#department#", m.data.Department.Name)
                                                                           .Replace("#service#", m.data.Service.name).
                                                                            Replace("#dateofapp#", string.Format("{0:dd-MM-yyyy}", m.data.date_of_application))
                                                                           .Replace("#dateofcomp#", string.Format("{0:dd-MM-yyyy}", m.data.date_of_completion))
                                                                           .Replace("#township#",m.data.LocationTownship.Name);
                            smsvm.SmsText = formattedmessage;
                            smsvm.SmsTime = DateTime.Now;
                            smsvm.CampaignId = result.Result[0].ID;
                            smsvm.MessageType = (int)MessageType.Main_Feedback_SMS;
                            MobileRegularExpression regularExpCls = new MobileRegularExpression();
                            smsvm.Operator = regularExpCls.Checkoperator(m.data.mobile);
                                #endregion MaptoSmsEntryViewModel

                                #region SendMessage
                                logger.LogInfo("Sending Campaign SMS..........");
                                IRestResponse restResponse = restsharpclient.SendSms(smsvm.Operator, m.data.mobile, campaignvm.SmsShortCodeText, formattedmessage);
                                #endregion SendMessage

                                #region SaveSms
                                CommandResult<Sms> smsresult = new CommandResult<Sms>();
                            Sms sms = new Sms();
                            sms = smsMapper.MapEntryViewModelToModel(sms, smsvm);
                            smsCounter.GetSmsCount(sms);
                                //smscounter.getcount(sms)
                            sms.CreatedBy =createby.ID;
                            sms.CreatedDate = DateTime.Now;
                            sms.IsDeleted = false;
                                if (restResponse.StatusCode != HttpStatusCode.OK)
                                {
                                    sms.Sms_Sent_Status = "Fail";
                                }
                                else
                                {
                                    sms.Sms_Sent_Status = "Success";
                                }
                                logger.LogInfo("Saving Campaign SMS..........");
                            smsresult = smsService.CreateOrUpdateCommand(sms);
                            #endregion SaveSms                   

                        }

                    }

                }
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                return Json(GetServerJsonResult<Campaigns>());
            }

            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Campaigns>());
            }
        }


        [HttpPost]
        [Route("CloseCampaign")]
        public IActionResult CloseCampaign(CampaignEntryViewModel campaignvm)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    CommandResult<Campaigns> result = new CommandResult<Campaigns>();
                    if (campaignvm.Id > 0)
                    {
                        Campaigns camp = campaignRepo.Get(campaignvm.Id);
                        if (camp != null)
                        {
                            camp.Status = false;//campaign close
                            camp.End_Time = DateTime.Now;//campaign end time
                            camp.ModifiedDate = DateTime.Now;
                            User user = GetLoggedInUser();
                            camp.ModifiedBy = user.ID;
                            result = campaignService.CreateOrUpdateCommand(camp);

                        }
                    }
                    AuditLog(nameof(AuditAction.UPDATE), nameof(ProgramCodeEnum.CAMPAIGN_ENTRY));
                    return Json(result);
                }
                catch
                {
                    return Json(GetServerJsonResult<Campaigns>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Campaigns>());
            }
        }

        [HttpGet]
        [Route("GetCampaignConfirmList/")]
        public JsonResult GetCampaignConfirmList(int groupid)
        {
            List<GroupMember> groupMembers = groupMemberRepo.GetGroupMembersByGroupId(groupid);
            List<string> phonenumberlist = groupMembers.Select(g => g.data.mobile).ToList();
            List<string> mobilelist = new List<string>();
            foreach(var mobile in phonenumberlist)
            {
                if (!string.IsNullOrEmpty(mobile))
                {
                    var first =mobile.Substring(0, 4);
                    var last = mobile.Last().ToString();
                    var star = "";
                    for (int i = 0; i < mobile.Length - 5; i++)
                    {
                        star += "*";
                    }
                   mobilelist.Add( first + star + last);
                }
            }          
            return Json(mobilelist);
        }

        [HttpGet]
        [Route("GetCampaignByName/")]
        public JsonResult GetCampaignByName(string name)
        {
            Campaigns camp = campaignRepo.FindByName(name);
            if (camp != null)
            {
                string msg = NY.Framework.Infrastructure.Constants.CampaignNameMustBeUnique;
                return Json(new { satisfied = false, message = msg });
            }
            else
            {
                return Json(new { satisfied = true, message = "" });
            }

        }

        [HttpGet]
        [Route("GetGroup")]
        public JsonResult GetGroup()
        {
            List<Groups> grouplist = new List<Groups>();
            User loginuser = GetLoggedInUser();
            //if loginuser is ministry acc
            if (loginuser.Role != null && loginuser.Role.ID == 2)
            {
                grouplist = groupRepo.Get().Where(g => (g.CreatedBy == loginuser.ID ||
                (g.CreatedByUser != null && g.CreatedByUser.parent_id == loginuser.ID))
                && g.IsDeleted == false).ToList();
            }
            //if loginuser is cpu acc
            if (loginuser.Role != null && loginuser.Role.ID == 3)
            {
                grouplist = groupRepo.Get().Where(g => g.CreatedBy == loginuser.ID
                && g.IsDeleted == false).ToList();
            }

            return Json(grouplist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

    }
}
