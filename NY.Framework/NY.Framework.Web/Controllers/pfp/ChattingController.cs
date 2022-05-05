using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NY.Framework.Web.Controllers.pfp
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChattingController : BaseController
    {
        ChattingMapper mapper;
        
        IChattingService inoutService;
        IChattingFileService chattingfileService;
        IChattingParticipantService participantService;
        IFileSystemService fileService;
        IView_ChattingListRepository chatlistRepo;
        IChattingParticipantRepository participantRepo;
        IView_GetReplyDatabyChattingIdRepository replybychattingidRepo;
        IChattingFileRepository chattingfileRepo;

        public ChattingController(IChattingService _inoutService,
            IChattingFileService _chattingfileService,            
            IChattingParticipantService _participantService,
            IFileSystemService _fileService,
            IView_ChattingListRepository _chatlistRepo,
            IChattingParticipantRepository _participantRepo,
            IView_GetReplyDatabyChattingIdRepository _replybychattingidRepo,
            IChattingFileRepository _chattingfileRepo) 
            : base(typeof(ChattingController), ProgramCodeEnum.INBOX_OUTBOX)
        {
            this.mapper = new ChattingMapper();            
            this.inoutService = _inoutService;
            this.chattingfileService = _chattingfileService;
            this.participantService = _participantService;
            this.fileService = _fileService;
            this.chatlistRepo = _chatlistRepo;
            this.participantRepo = _participantRepo;
            this.replybychattingidRepo = _replybychattingidRepo;
            this.chattingfileRepo = _chattingfileRepo;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<ChattingListViewModel> list = GetAllData();
                return Json(list);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<View_ChattingList>());
            }
        }

        public JQueryDataTablePagedResult<ChattingListViewModel> GetAllData()
        {
            JqueryDataTableQueryOptions<View_ChattingList> queryoption = GetJQueryDataTableQueryOptions<View_ChattingList>();

            int LoginRoleId = GetLoggedInRoleId();
            if (LoginRoleId == 3)// CPU Account
            {
                queryoption.FilterBy = x => x.reply_chatting_id==0 && x.created_by.Equals(GetLoggedInId());
            }
            if (LoginRoleId == 4) // DEO Account
            {
                queryoption.FilterBy = x => x.reply_chatting_id == 0 && x.receiver.Equals(GetLoggedInId());
            }
            ChattingListViewModel model = new ChattingListViewModel();
            model.fromdate = Request.Query["fromdate"].ToString();
            model.todate = Request.Query["todate"].ToString();

            queryoption = mapper.PrepareQueryOptionForRepository(queryoption,model);
            List<View_ChattingList> list = chatlistRepo.GetListWithFilter(queryoption);
            JQueryDataTablePagedResult<ChattingListViewModel> vmlist = new JQueryDataTablePagedResult<ChattingListViewModel>();
            //vmlist.recordsFiltered = list.recordsFiltered;
            //vmlist.recordsTotal = list.recordsTotal;
            if (list != null)
            {
                var chatlist = list.GroupBy(x => x.chat_id).ToList();
                vmlist = mapper.MapModelToListViewModel(chatlist, replybychattingidRepo, GetLoggedInId());
            }
            
            return vmlist;
        }

        [HttpPost]
        //[Consumes("multipart/form-data")]
        public IActionResult SaveOrUpdate(ChattingViewModel vm)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    CommandResult<Chatting> result = new CommandResult<Chatting>();

                    #region Save Main Chatting

                    Chatting entity = new Chatting();
                    entity.description = vm.description;
                    entity.created_date = DateTime.Now;
                    entity.created_by = GetLoggedInId();
                    entity.ismain = vm.ismain;
                    result = inoutService.CreateOrUpdateCommand(entity);

                    #endregion

                    if (result.Success)
                    {
                        #region Save Chatting For Receiver
                        foreach(int x in vm.receiverarr)
                        {
                            ChattingParticipant participant = new ChattingParticipant();
                            participant.sender = result.Result[0].created_by;
                            participant.receiver = Convert.ToInt32(x);
                            participant.isread = false;
                            if (vm.ismain)
                            {
                                participant.chatting_id = result.Result[0].ID;
                                participant.reply_chatting_id = 0;
                            }
                            else
                            {
                                participant.reply_chatting_id = result.Result[0].ID;
                                participant.chatting_id = vm.mainchatting_id;
                            }
                            participantService.CreateOrUpdate(participant);
                        }
                        #endregion                        
                    }
                    return Json(new { result, chatid= result.Result[0].ID});
                }
                catch (Exception ex)
                {
                    return Json(GetServerJsonResult<Chatting>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Chatting>());
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult SaveAttachFile([FromForm]IFormCollection vm)
        {
            #region Save Attach Files

            if (vm.Files != null)
            {
                CommandResult<ChattingFile> fileResult = new CommandResult<ChattingFile>();
                ChattingFile chattingFile = new ChattingFile();
                var cid = Request.Query["chatid"].ToString();
                
                for (int i = 0; i < vm.Files.Count; i++)
                {
                    if (vm.Files[i].FileName != null)
                    {
                        Guid guId = Guid.NewGuid();
                        string filetype = Path.GetExtension(vm.Files[i].FileName);
                        chattingFile = new ChattingFile();
                        chattingFile.file_name = guId.ToString() + filetype;
                        if (!string.IsNullOrEmpty(cid))
                        {
                            chattingFile.chatting_id = Convert.ToInt32(cid);
                        }
                        //chattingFile.chatting_id = vm.mainchatting_id;
                        chattingFile.file_path = Constants.AttachFile + chattingFile.file_name;
                        fileResult = chattingfileService.CreateOrUpdate(chattingFile);
                        if (fileResult.Success)
                        {
                            fileService.SaveFile(vm.Files[i], Constants.AttachFile, chattingFile.file_name);
                        }
                    }
                }
                
            }

            #endregion
            return Json(true);
        }

        [HttpPost]
        [Route("SaveFiles")]
        [Consumes("multipart/form-data")]
        public IActionResult SaveFile(IFormCollection file)
        {
            return Json(true);
        }

        [HttpGet]
        [Route("GetDetail/")]
        public ActionResult GetDetail()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                int chat_id = Convert.ToInt32(Request.Query["chat_id"]);
                List<ChattingListViewModel> list = GetDetailData(chat_id);
                return Json(list);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Chatting>());
            }
        }

        public List<ChattingListViewModel> GetDetailData(int chat_id)
        {
            User user = GetLoggedInUser();
            List<ChattingListViewModel> result = new List<ChattingListViewModel>();

            #region Get Main Inbound
            var bychatid = chatlistRepo.GetByChattingId(chat_id);
            ChattingListViewModel mainvm = new ChattingListViewModel();
            foreach (var data in bychatid)
            {
                mainvm.description = data.description;
                mainvm.created_date= string.Format("{0:dd-MM-yyyy  hh-mm-ss tt}", data.created_date);
                mainvm.sender = data.sendername;
                if (!string.IsNullOrEmpty(mainvm.receiver)) { mainvm.receiver += ", " + data.receivername; }
                else { mainvm.receiver += data.receivername; }
            }
            List<ChattingFile> filelist= chattingfileRepo.GetByChattingId(chat_id);
            //mainvm.File = 
            foreach(var file in filelist)
            {
                if (System.IO.File.Exists(file.file_path))
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(file.file_path);
                    file.file_path = Convert.ToBase64String(imageArray);
                }
            }
            mainvm.File = filelist;
            result.Add(mainvm);
            #endregion

            #region Get Reply Inbound/Outbound
            List<View_GetReplyDatabyChattingId> List = new List<View_GetReplyDatabyChattingId>();
            if (user.role_id == 3)
            {
                List = replybychattingidRepo.GetByChattingId(chat_id, user.ID);
            }
            else if (user.role_id == 4)
            {
                List = replybychattingidRepo.GetByChattingId(chat_id, user.ID, user.parent_id);
            }
            int count = List.Where(x => x.sender != user.ID).Count();
            var dataList= List.GroupBy(x => x.reply_chatting_id);
            foreach(var data in dataList)
            {
                ChattingListViewModel replyvm = new ChattingListViewModel();
                if (data.Count() > 1)
                {
                    foreach (var aa in data)
                    {                        
                        replyvm.description = aa.description;
                        replyvm.created_date = string.Format("{0:dd-MM-yyyy  hh-mm-ss tt}", aa.created_date);
                        replyvm.sender = aa.sendername;
                        if (!string.IsNullOrEmpty(replyvm.receiver)) { replyvm.receiver += ", " + aa.receivername; }
                        else { replyvm.receiver += aa.receivername; }                   
                    }
                    //replyvm.File= chattingfileRepo.GetByChattingId(data.Key);
                    filelist = new List<ChattingFile>();
                    filelist = chattingfileRepo.GetByChattingId(data.Key);
                    foreach (var file in filelist)
                    {
                        if (System.IO.File.Exists(file.file_path))
                        {
                            byte[] imageArray = System.IO.File.ReadAllBytes(file.file_path);
                            file.file_path = Convert.ToBase64String(imageArray);
                        }
                    }
                    replyvm.File = filelist;
                    result.Add(replyvm);
                }
                else
                {
                    foreach (var aa in data)
                    {
                        replyvm.description = aa.description;
                        replyvm.created_date = string.Format("{0:dd-MM-yyyy  hh-mm-ss tt}", aa.created_date);
                        replyvm.sender = aa.sendername;
                        if (!string.IsNullOrEmpty(replyvm.receiver)) { replyvm.receiver += ", " + aa.receivername; }
                        else { replyvm.receiver += aa.receivername; }
                        //replyvm.receiver += aa.receivername + ",";
                        //replyvm.File = chattingfileRepo.GetByChattingId(data.Key);
                        filelist = new List<ChattingFile>();
                        filelist = chattingfileRepo.GetByChattingId(data.Key);
                        foreach (var file in filelist)
                        {
                            if (System.IO.File.Exists(file.file_path))
                            {
                                byte[] imageArray = System.IO.File.ReadAllBytes(file.file_path);
                                file.file_path = Convert.ToBase64String(imageArray);
                            }
                        }
                        replyvm.File = filelist;
                        result.Add(replyvm);
                    }
                }                
            }
            #endregion

            #region Update chatting_participant table
            UpdateStatus(chat_id, user.ID);
            #endregion

            return result;
        }

        private void UpdateStatus(int chat_id, int loginid)
        {
            var list = participantRepo.GetByChattingId(chat_id, loginid);
            foreach (var participant in list)
            {
                participant.isread = true;
                participantService.CreateOrUpdate(participant);
            }
        }

    }
}
