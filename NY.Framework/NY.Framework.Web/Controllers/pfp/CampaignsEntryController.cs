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
using NY.Framework.Infrastructure.Repositories;
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
        ISmsRepository smsRepository;
        
        
        RestSharpClient restsharpclient = new RestSharpClient();
        SmsCounter smsCounter;


        public CampaignsEntryController(ICampaignService _campaignService,
            IGroupMemberRepository _groupMemberRepo, IGroupRepository _groupRepo,
        ICampaignsRepository _campaignRepo, ISmsService _smsService,ISmsRepository _smsRepository
            )
            : base(typeof(CampaignsEntryController), ProgramCodeEnum.CAMPAIGN_ENTRY)
        {
            mapper = new CampaignMapper();
            campaignService = _campaignService;
            groupMemberRepo = _groupMemberRepo;
            groupRepo = _groupRepo;
            campaignRepo = _campaignRepo;
            smsService = _smsService;
            smsRepository = _smsRepository;
            smsMapper = new SmsMapper();
            logger = new Logger(typeof(CampaignsEntryController));
            smsCounter = new SmsCounter();
        }

        [AllowAnonymous]
        [HttpGet, HttpPost]
        [Route("SendSmsMptAndTelenorWithAllParams")]
        public string SendSmsMptAndTelenorWithAllParams()
        {
            try
            {
                MobileRegularExpression regularExpression = new MobileRegularExpression();
                string id = Request.Query["id"].FirstOrDefault();
                string id_smsc = Convert.ToInt32(Request.Query["id_smsc"].FirstOrDefault()).ToString();
                string message_status = Request.Query["message_status"].FirstOrDefault();
                string level = Request.Query["level"].FirstOrDefault();
                string connector = Request.Query["connector"].FirstOrDefault();

                string subdate1 = Request.Query["subdate"].ToString();
                string subdate = Request.Query["subdate"].FirstOrDefault();

                string donedate = Request.Query["donedate"].FirstOrDefault();
                string sub = Convert.ToInt32(Request.Query["sub"].FirstOrDefault()).ToString();
                string dlvrd = Convert.ToInt32(Request.Query["dlvrd"].FirstOrDefault()).ToString();
                
                string text = Request.Query["text"].FirstOrDefault();
                string Direction = Request.Query["Direction"].FirstOrDefault();
                string Operator = Request.Query["Operator"].FirstOrDefault();

                string err = Request.Query["err"].FirstOrDefault();
                int err1 = 0;
                if (!string.IsNullOrEmpty(err))
                {
                    err1 = Convert.ToInt32(err);
                }
                

                logger.LogDebug("message status = " + message_status);
                logger.LogDebug("Inside CallBackFunction id= " + id + 
                    " and error = " + err + " - " + err1 + 
                    " and id smsc = " + id_smsc +  
                    " and message status = " + message_status +
                    " and level = " + level +
                    " and connector = " + connector +
                    " and subdate = " + subdate + " - " + subdate1 +
                    " and donedate = " + donedate +
                    " and sub = " + sub +
                    " and dlvrd = " + dlvrd +
                    " and err = " + err +
                    " and text = " + text + 
                    " and Direction = " + Direction + 
                    " and Operator = " + Operator 
                );
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }


            return "ACK/Jasmin";
        }
        [AllowAnonymous]
        [HttpGet, HttpPost]
        [Route("SendSmsOoredoo")]
        public string SendSmsOoredoo([FromForm] SmsEntryViewModel vm)
        {
            try
            {
                //logger.LogDebug("Inside Ooredoo sms " + vm);
                //var forms = "";
                //foreach (var f in Request.Form)
                //{
                    
                //    forms += f.Key + "   " + f.Value + " >>> ";
                    
                //}
                //logger.LogDebug("Inside Ooredoo " + "form " + forms + " Custom ID" + Request.Form["customID"]);

                #region SaveSms
                logger.LogInfo("SendSmsOoredoo : Status " + Request.Form["status"] + " CustomID " + Request.Form["customID"]);
                int customID = Convert.ToInt32(Request.Form["customID"]);
                CommandResult<Sms> smsresult = new CommandResult<Sms>();
                Sms sms = new Sms();
                if (customID > 0)
                {
                    logger.LogInfo("Get SMS By Id = " + customID);
                    sms = smsRepository.Get(customID);
                }
                if (sms != null)
                {
                    logger.LogInfo("SendSmsOoredoo : " + Request.Form["status"]);
                    string message_status = "";
                    if (Request.Form["status"] == "D")
                    {
                        sms.Sms_Sent_Status = "Success";
                    }
                    else
                    {
                        sms.Sms_Sent_Status = "Fail";
                    }
                    
                    logger.LogInfo("Saving Campaign SMS.......... sms status = " + sms.Sms_Sent_Status);
                    smsresult = smsService.CreateOrUpdateCommand(sms);
                }

                #endregion SaveSms  

            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
            return "ACK/Jasmin";
        }

        //[AllowAnonymous]
        //[HttpGet, HttpPost]
        //[Route("SendSmsMptAndTelenorWithQueryString")]
        //public string SendSmsMptAndTelenorWithQueryString([FromForm] SmsEntryViewModel vm)
        //{
        //    try
        //    {
        //        logger.LogDebug("Inside Ooredoo SMS ID: " + vm.id);
        //        var forms = "";
        //        foreach(var f in Request.Form) {
        //            forms += f.Key + "   " + f.Value + " >>> ";

        //            } 
                
        //        string body = "";
        //        using(System.IO.StreamReader stream = new System.IO.StreamReader(Request.Body))
        //        {
        //            body = stream.ReadToEnd();
                    
        //        }

        //        var headers = "";

        //        foreach(var h in Request.Headers)
        //        {
        //            headers += h.Key + "..." + h.Value;
        //        }
        //        logger.LogDebug("Inside SendSmsMptAndTelenor "+ "form " + forms + " method: " + Request.Method.ToString() + "  headers: " + headers + " body: " + body + Request.QueryString.ToString());
        //        //logger.LogDebug("Inside SendSmsMptAndTelenor " + vm.Operator + vm.DataInfoId + vm.CampaignId + vm.SmsText);
        //        //#region SaveSms
        //        //CommandResult<Sms> smsresult = new CommandResult<Sms>();
        //        //Sms sms = new Sms();

        //        //sms.IsDeleted = false;

        //        //string[] arr = vm.Operator.Split('?');
        //        //sms.Operator = arr[0];
        //        //string message_status = arr[1];

        //        //sms.Direction = 1;
        //        //sms.Sms_Code_Id = vm.SmsCodeId;
        //        //logger.LogDebug("operator" + sms.Operator + " status " + message_status + "cid" + sms.Sms_Code_Id);
        //        //if (vm.DataInfoId != 0)
        //        //{
        //        //    sms.DataInfo_Id = vm.DataInfoId;
        //        //}
        //        //sms.Sms_Time =DateTime.Now;
        //        //sms.Sms_Text = vm.SmsText;
        //        //if (vm.CampaignId != 0)
        //        //{
        //        //    sms.Campaign_Id = vm.CampaignId;
        //        //}
        //        //sms.Message_Type =(int)MessageType.Main_Feedback_SMS;


        //        //smsCounter.GetSmsCount(sms);
        //        ////smscounter.getcount(sms)
        //        //sms.CreatedBy = GetLoggedInId();
        //        //sms.CreatedDate = DateTime.Now;

        //        //if (message_status.Contains("ESME_ROK"))
        //        //{
        //        //    sms.Sms_Sent_Status = "Success";
        //        //}
        //        //else
        //        //{
        //        //    sms.Sms_Sent_Status = "Fail";
        //        //}
        //        //logger.LogInfo("Saving Campaign SMS..........");
        //        //smsresult = smsService.CreateOrUpdateCommand(sms);
        //        //#endregion SaveSms                   
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Log(ex);
        //    }


        //    return "ACK/Jasmin";
        //}
        
        
        [AllowAnonymous]
        [HttpGet, HttpPost]
        [Route("SendSmsMptAndTelenor")]
        public string SendSmsMptAndTelenor([FromQuery] SmsEntryViewModel vm)
        {
            try
            {
                //logger.LogDebug("Inside SendSmsMptAndTelenor " + Request.QueryString.ToString());
                logger.LogDebug("Inside SendSmsMptAndTelenor: SMS ID " + vm.id + vm.Operator);
                #region SaveSms
                CommandResult<Sms> smsresult = new CommandResult<Sms>();
                Sms sms = new Sms();
                if (vm.id > 0)
                {
                    logger.LogInfo("Get SMS By Id = " + vm.id);
                    sms = smsRepository.Get(vm.id);
                }
                if (sms != null)
                {
                    logger.LogInfo("SendSmsMptAndTelenor : " + vm.Operator);
                    string message_status = "";
                    if (vm.Operator.Contains("?"))
                    {                        
                        string[] arr = vm.Operator.Split('?');
                        message_status = arr[1];
                        if (message_status.Contains("ESME_ROK"))
                        {
                            sms.Sms_Sent_Status = "Success";
                        }
                        else
                        {
                            sms.Sms_Sent_Status = "Fail";
                        }
                    }
                    else
                    {
                        //sms.Operator = vm.Operator;
                        sms.Sms_Sent_Status = "Fail";
                    }                    
                    logger.LogInfo("Saving Campaign SMS.......... sms status = "+sms.Sms_Sent_Status);
                    smsresult = smsService.CreateOrUpdateCommand(sms);
                }
                
                #endregion SaveSms                   
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

                                #region SaveSms
                                if (smsvm.Operator != "mytel1111")
                                {
                                    CommandResult<Sms> smsresult = new CommandResult<Sms>();
                                    Sms sms = new Sms();
                                    sms = smsMapper.MapEntryViewModelToModel(sms, smsvm);
                                    smsCounter.GetSmsCount(sms);
                                    //smscounter.getcount(sms)
                                    sms.CreatedBy = createby.ID;
                                    sms.CreatedDate = DateTime.Now;
                                    sms.IsDeleted = false;
                                    
                                    if(smsvm.Operator == "mytel1111")
                                    {
                                        sms.Sms_Sent_Status = "Success";
                                    }
                                    else
                                    {
                                        sms.Sms_Sent_Status = "Pending";
                                    }
                                    
                                    logger.LogInfo("Saving Campaign SMS..........");
                                    smsresult = smsService.CreateOrUpdateCommand(sms);

                                    #region SendMessage
                                    logger.LogInfo("Sending Campaign SMS..........");
                                    smsvm.id = smsresult.Result[0].ID;
                                    IRestResponse restResponse = restsharpclient.SendSms(smsvm, m.data.mobile, campaignvm.SmsShortCodeText, formattedmessage);
                                    #endregion SendMessage
                                }
                                #endregion SaveSms 
                                else
                                {
                                    #region SendMessage
                                    logger.LogInfo("Sending Campaign SMS..........");
                                    //smsvm.id = smsresult.Result[0].ID;
                                    IRestResponse restResponse = restsharpclient.SendSms(smsvm, m.data.mobile, campaignvm.SmsShortCodeText, formattedmessage);
                                    #endregion SendMessage

                                    CommandResult<Sms> smsresult = new CommandResult<Sms>();
                                    Sms sms = new Sms();
                                    sms = smsMapper.MapEntryViewModelToModel(sms, smsvm);
                                    smsCounter.GetSmsCount(sms);
                                    //smscounter.getcount(sms)
                                    sms.CreatedBy = createby.ID;
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
                                }


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
                    if(mobile.Length <= 4)
                    {
                        mobilelist.Add(mobile);
                    }
                    else
                    {
                        var first = mobile.Substring(0, 4);
                        var last = mobile.Last().ToString();
                        var star = "";
                        for (int i = 0; i < mobile.Length - 5; i++)
                        {
                            star += "*";
                        }
                        mobilelist.Add(first + star + last);
                    }
                    
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
