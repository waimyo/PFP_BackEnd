using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Logging;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;
using RestSharp;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using static NY.Framework.Web.Models.SmsInboundViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.pfp
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsInboundController : BaseController
    {
        ISmsRepository smsRepo;
        IDataRepository dataInfoRepo;
        ISmsShortCodeRepository smsShortCodeRepo;
        ISmsService smsService;
        SmsMapper smsMapper;
        RestSharpClient restsharpclient;
        Logger logger;
        SmsCounter smsCounter;

        public SmsInboundController(ISmsRepository _smsRepo,
            ISmsShortCodeRepository _smsShortCodeRepo,
            ISmsService _smsService,
            IDataRepository _dataInfoRepo) : base(typeof(SmsInboundController), ProgramCodeEnum.SENDING_SMS)
        {
            smsRepo = _smsRepo;
            dataInfoRepo = _dataInfoRepo;
            smsShortCodeRepo = _smsShortCodeRepo;
            smsService = _smsService;
            smsMapper = new SmsMapper();
            restsharpclient = new RestSharpClient();
            logger = new Logger(typeof(SmsInboundController));
            smsCounter = new SmsCounter();
        }

        // POST api/<controller>
        [HttpPost]
        [Route("ReceiveSms")]
        public string ReceiveMptOrTelenorSms([FromForm]SmsInboundViewModel vm)
        {
            try
            {
                /***convert '959' format to '09' format for mpt and telenor***/
                vm.From = string.Concat("0", vm.From.Remove(0, 2));

                MobileRegularExpression regularExpCls = new MobileRegularExpression();
                vm.Origin_Connector = regularExpCls.Checkoperator(vm.From);

                /*check string is unicode*/
                if (!string.IsNullOrEmpty(vm.Coding)&&vm.Coding.Equals("\b"))
                {
                    /***Decode hex string to unicode***/
                    UnicodeTools unicodetool = new UnicodeTools();
                    vm.Content = unicodetool.Decode(vm.Content);
                }
              
                 string result = SaveSmsAndSendClosingMessage(vm);
                // return "ACK/Jasmin";
                return result;
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                return "ACK/Jasmin";
            }
        }

        [HttpGet]
        [Route("ReceiveOoredooSms")]
        public IActionResult ReceiveOoredooSms([FromQuery]OoredooInboundViewModel vm)
        {

            try
            {
                SmsInboundViewModel smsvm = new SmsInboundViewModel();
                /***convert '959' format to '09' format for mpt and telenor***/
                smsvm.From = string.Concat("0", vm.Sender.Remove(0, 2));               
                smsvm.To = vm.InNumber;
                smsvm.Content = vm.Content;
                MobileRegularExpression regularExpCls = new MobileRegularExpression();
                smsvm.Origin_Connector = regularExpCls.Checkoperator(smsvm.From);

                string result = SaveSmsAndSendClosingMessage(smsvm);
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                return Json(GetServerJsonResult<Sms>());
            }
        }

        [HttpGet]
        [Route("ReceiveMytelSms")]
        public IActionResult ReceiveMytelSms([FromQuery]MytelInboundViewModel vm)
        {
            try
            {
                SmsInboundViewModel smsvm = new SmsInboundViewModel();
                /***convert '959' format to '09' format for mpt and telenor***/
                smsvm.From = string.Concat("0", vm.SourceAddr.Remove(0, 2));
                smsvm.To = vm.DestAddr;
                smsvm.Content = vm.Content;
                MobileRegularExpression regularExpCls = new MobileRegularExpression();
                smsvm.Origin_Connector = regularExpCls.Checkoperator(smsvm.From);

                string result = SaveSmsAndSendClosingMessage(smsvm);
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                return Json(GetServerJsonResult<Sms>());
            }
        }

        [HttpPost]
        [Route("ReceiveOoredooSms")]
        public IActionResult ReceiveOoredooSmsForHttpPost([FromForm]OoredooInboundViewModel vm)
        {

            try
            {
                SmsInboundViewModel smsvm = new SmsInboundViewModel();
                /***convert '959' format to '09' format for mpt and telenor***/
                smsvm.From = string.Concat("0", vm.Sender.Remove(0, 2));          
                smsvm.To = vm.InNumber;
                smsvm.Content = vm.Content;
                MobileRegularExpression regularExpCls = new MobileRegularExpression();
                smsvm.Origin_Connector = regularExpCls.Checkoperator(smsvm.From);

                string result = SaveSmsAndSendClosingMessage(smsvm);
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                return Json(GetServerJsonResult<Sms>());
            }
        }

        [HttpPost]
        [Route("ReceiveMytelSms")]
        public IActionResult ReceiveMytelSmsForHttpPost([FromBody]MytelInboundViewModel vm)
        {
            try
            {
                SmsInboundViewModel smsvm = new SmsInboundViewModel();
                /***convert '959' format to '09' format for mpt and telenor***/             
                smsvm.From = string.Concat("0", vm.SourceAddr.Remove(0, 2));
                smsvm.To = vm.DestAddr;
                smsvm.Content = vm.Content;
                MobileRegularExpression regularExpCls = new MobileRegularExpression();
                smsvm.Origin_Connector = regularExpCls.Checkoperator(smsvm.From);

                string result = SaveSmsAndSendClosingMessage(smsvm);
                return Json(result);
            }
            catch (Exception ex)
            {
                logger.Log(ex);
                return Json(GetServerJsonResult<Sms>());
            }
        }

        public string SaveSmsAndSendClosingMessage(SmsInboundViewModel vm)
        {
            try
            {
                CommandResult<Sms> inbound_smsresult = new CommandResult<Sms>();

                #region GetCampaignBySenderMobileNo
                Sms campaignSmsInfo = smsRepo.GetCampaignBySenderMobileNumber(vm.From);
                #endregion GetCampaignBySenderMobileNo

                #region GetDataInfoId
                Data datainfo = new Data();
                if (campaignSmsInfo != null)
                {
                    datainfo = campaignSmsInfo.DataInfo;
                }
                else
                {
                    datainfo = dataInfoRepo.GetDataInfoByMobileNo(vm.From);
                }
                
                #endregion GetDataInfoId

                #region GetSmsShortCodeId
                SmsShortCode smsshortcode = smsShortCodeRepo.Get().Where(s => s.Sms_Code.Equals(vm.To)).FirstOrDefault();
                #endregion GetSmsShortCodeId

                #region MaptoSmsEntryViewModel
                SmsEntryViewModel smsvm = new SmsEntryViewModel();
                smsvm.DataInfoId = datainfo != null ? datainfo.ID : 0;
                smsvm.SmsCodeId = smsshortcode.ID;
                smsvm.SmsTime = DateTime.Now;
                smsvm.Operator = vm.Origin_Connector;
                //smsvm.Operator = Checkoperator(m.data.mobile);  

                //if sender's mobile number not included in  campaign,
                //insert record into sms table  with "Number not found in database" reason.
                if (campaignSmsInfo == null)
                {
                    smsvm.CampaignId = 0;
                    smsvm.MessageType = (int)MessageType.Invalid;
                    smsvm.Reason = "Number not found in database.";
                }
                //if sender's mobile number   included in  campaign,
                else
                {
                    smsvm.CampaignId = Convert.ToInt32(campaignSmsInfo.Campaign_Id);
                    //If campaign Opens
                    if (campaignSmsInfo.Campaign.Status)
                    {
                        smsvm.MessageType = (int)MessageType.Valid_Reply;
                    }
                    //if campaign was closed
                    else
                    {
                        smsvm.MessageType = (int)MessageType.Invalid_Because_Campaign_Has_been_Closed;
                        smsvm.Reason = "Campaign was closed when message arrived";
                    }
                }
                #endregion MaptoSmsEntryViewModel

                logger.LogInfo("Saving Inbound SMS**********"+" from "+vm.From+" to "+vm.To+" content "+vm.Content);
                #region Save_InboundSms          
                Sms sms = new Sms();
                smsvm.Direction = (int)SmsDirection.Received;
                smsvm.SmsText = vm.Content;
                sms = smsMapper.MapEntryViewModelToModel(sms, smsvm);
                //User user = GetLoggedInUser();
                smsCounter.GetSmsCount(sms);
                sms.CreatedBy = campaignSmsInfo != null ? campaignSmsInfo.CreatedBy : 0;
                sms.CreatedDate = DateTime.Now;
                sms.IsDeleted = false;
                sms.Sms_Sent_Status = "Success";
                inbound_smsresult = smsService.CreateOrUpdateCommand(sms);
                #endregion Save_InboundSms                                        

                #region SendClosingMessage
                if (inbound_smsresult.Success && campaignSmsInfo != null && campaignSmsInfo.Campaign.Status)
                {
                    logger.LogInfo("Sending Closing SMS**********");
                    //when reply message has saved to sms table,send closing message back.
                    IRestResponse restResponse = restsharpclient.SendSms(smsvm.Operator, vm.From, vm.To, campaignSmsInfo.Campaign.Closing_Message);

                    //after sending closing message, save closing_message info to sms table
                    #region SaveClosingMessage
                    Sms closing_sms = new Sms();
                    smsvm.Direction = (int)SmsDirection.Sent;
                    smsvm.MessageType = (int)MessageType.Closing_Message;
                    smsvm.SmsText = campaignSmsInfo.Campaign.Closing_Message;
                    closing_sms = smsMapper.MapEntryViewModelToModel(closing_sms, smsvm);
                    //User user = GetLoggedInUser();
                    smsCounter.GetSmsCount(closing_sms);
                    closing_sms.CreatedBy = campaignSmsInfo != null ? campaignSmsInfo.CreatedBy : 0;
                    closing_sms.CreatedDate = DateTime.Now;
                    closing_sms.IsDeleted = false;
                    if (restResponse.StatusCode != HttpStatusCode.OK)
                    {
                        closing_sms.Sms_Sent_Status = "Fail";
                    }
                    else
                    {
                        closing_sms.Sms_Sent_Status = "Success";
                    }
                    logger.LogInfo("Saving Closing Sms**********");
                    inbound_smsresult = smsService.CreateOrUpdateCommand(closing_sms);

                    #endregion SaveClosingMessage

                }
                #endregion SendClosingMessage
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
          

            return "ACK/Jasmin";
        }



    }
}
