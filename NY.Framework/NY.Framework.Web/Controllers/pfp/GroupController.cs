using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
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
    public class GroupController : BaseController
    {
        IGroupRepository gpRepo;
        IGroupService gpService;
        GroupMapper mapper;
        IDataRepository dataRepo;
        IGroupMemberRepository gpmemberRepo;
        IGroupMemberService gpmemberService;
        IView_GroupListRepository glistRepo;

        ISP_GroupMemberListRepository gmlistRepo;

        public GroupController(IGroupService _gpService, IGroupRepository _gpRepo, IDataRepository _dataRepo,
            IGroupMemberRepository _gpmemberRepo, IGroupMemberService _gpmemberService,
            IView_GroupListRepository _glistRepo, ISP_GroupMemberListRepository _gmlistRepo)
            : base(typeof(GroupController), ProgramCodeEnum.MOBILE_GROUP_LIST)
        {
            this.gpRepo = _gpRepo;
            this.gpService = _gpService;
            mapper = new GroupMapper();
            this.dataRepo = _dataRepo;
            this.gpmemberRepo = _gpmemberRepo;
            this.gpmemberService = _gpmemberService;
            this.gmlistRepo = _gmlistRepo;
            this.glistRepo = _glistRepo;
        }

       
        [HttpGet]
        [Route("GetAllGroup")]
        public JsonResult GetAllGroup()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {


                JQueryDataTablePagedResult<View_GroupList> vmlist = GetAllDataGroup();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.MOBILE_GROUP_LIST));
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Groups>());
            }
        }

        public JQueryDataTablePagedResult<View_GroupList> GetAllDataGroup()
        {
            JqueryDataTableQueryOptions<View_GroupList> queryoption = GetJQueryDataTableQueryOptions<View_GroupList>();
            GroupSearchViewModel search = MapSearchDataGroup();
            queryoption = mapper.PrepareQueryOptionForRepositoryGroup(queryoption, search);
            User loginuser = GetLoggedInUser();
            //if login_user is not supreadmin,filter list by login_user id
            //if (loginuser.role_id != 1)
            //{
            //    queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy,
            //        c => c.user_id == loginuser.ID || c.parent_id.Equals(loginuser.ID));
            //}
            if (loginuser.role_id == 3)
            {
                queryoption.FilterBy1 = x => x.user_id.Equals(loginuser.ID);
                queryoption.FilterBy1 = LinqExpressionHelper.AppendOr(queryoption.FilterBy1, x => x.parent_id.Equals(loginuser.ID));
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, queryoption.FilterBy1);
            }
            if (loginuser.role_id == 2)
            {
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, x => x.ministry_id.Equals(loginuser.ministry_id));
            }
            JQueryDataTablePagedResult<View_GroupList> list = glistRepo.GetPagedResults(queryoption);
           
            return list;
        }

        public GroupSearchViewModel MapSearchDataGroup()
        {
            GroupSearchViewModel model = new GroupSearchViewModel();
            var ministry = Request.Query["ministry_id"].ToString();
            if (!string.IsNullOrEmpty(ministry))
            {
                
                    model.ministry_id = Convert.ToInt32(ministry);
               
            }
            model.user_id = Convert.ToInt32(Request.Query["sender_group_id"].ToString());
            
            model.fromdate = Request.Query["fromdate"].ToString();
            model.todate = Request.Query["todate"].ToString();
            model.group_name = Request.Query["group_name"].ToString();
            var gpmember = Request.Query["group_member"].ToString();
            if (!string.IsNullOrEmpty(gpmember))
            {
                model.group_member = Convert.ToInt32(gpmember);
            }

            return model;
        }


        [HttpGet]
        [Route("GetById")]
        public JsonResult GetById(int id)
        {
            View_GroupList glist = new View_GroupList();
            glist = glistRepo.Get(id);
            GroupSearchViewModel vm = mapper.MapEntityToViewModel(glist);
            return Json(vm);
        }





        [HttpGet]
        [Route("GetAllGroupMember")]
        public JsonResult GetAllGroupMember()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                //NY.Framework.Infrastructure.JqueryDataTableQueryOptions<Data> queryoption = GetJQueryDataTableQueryOptions<Data>();
                GroupMemberListViewModel model = new GroupMemberListViewModel();
                var gid = Request.Query["group_id"].ToString();
                if (!string.IsNullOrEmpty(gid))
                {
                    model.group_id = Convert.ToInt32(gid);
                }
                //var vm = SearchData();
                JqueryDataTableQueryOptions<SP_GroupMemberList> queryOptions = GetJQueryDataTableQueryOptions<SP_GroupMemberList>();
                queryOptions = mapper.PrepareQueryOptionForRepositoryGroupMember(queryOptions);

                JQueryDataTablePagedResult<SP_GroupMemberList> list = gmlistRepo.GetProcedurePagedResults(queryOptions, "exec SP_GroupMemberList {0}", new object[] { model.group_id });
                JQueryDataTablePagedResult<GroupMemberListViewModel> vmlist = mapper.MapModelToListViewModelGroupMember(list);

                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Groups>());
            }
        }


        //[HttpDelete("{id}")]
        //public JsonResult Delete(int id)
        //{
        //    CommandResult<GroupMember> result = new CommandResult<GroupMember>();
        //    if (Authorize(AuthorizeAction.DELETE))
        //    {
        //        GroupMember gm = gpmemberRepo.Get(id);
        //        if (gm != null)
        //        {
        //            result = gpmemberService.Delete(gm);

        //            if (result.Success)
        //            {
        //                //AuditLog(AuditAction.DELETE, Constants.PCodeFor_DEPARTMENT_REGISTRATION, d.ToAuditString());
        //            }
        //        }
        //        return Json(result);
        //    }
        //    else
        //    {
        //        return Json(GetAccessDeniedJsonResult<Groups>());
        //    }
        //}

        //[HttpDelete("{id}")]
        [HttpPost]
        [Route("DeleteSelectedRow")]
        public JsonResult Delete(GroupMemberListViewModel vm)
        {
            CommandResult<GroupMember> result = new CommandResult<GroupMember>();
            if (Authorize(AuthorizeAction.DELETE))
            {
                if(vm.selectrows != null)
                {
                    foreach(var select in vm.selectrows)
                    {
                        GroupMember gm = gpmemberRepo.Get(select.Id);
                        if (gm != null)
                        {
                            result = gpmemberService.Delete(gm);

                            if (result.Success)
                            {
                                //AuditLog(AuditAction.DELETE, Constants.PCodeFor_DEPARTMENT_REGISTRATION, d.ToAuditString());
                            }
                        }
                    }
                }
                
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Groups>());
            }
        }

        [HttpPost]
        [Route("InsertDataForData")]
        public IActionResult SaveOrUpdateMember(GroupMemberInsertViewModel entrymodel)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                CommandResult<GroupMember> returnresult = new CommandResult<GroupMember>();
                try
                {
                    int uid = GetLoggedInId();
                    CommandResult<GroupMember> result = new CommandResult<GroupMember>();
                    List<GroupMember> lists = mapper.MapViewModelToEntityMember(entrymodel);
                    //List<CirculationBorrow> lists = new List<CirculationBorrow>();
                    foreach (var l in lists)
                    {
                        l.CreatedBy = uid;
                        l.CreatedDate = DateTime.Now;
                        result = gpmemberService.CreateOrUpdate(l);
                        if (result.Success)
                        {
                            returnresult.Success = true;
                            returnresult.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
                        }
                    }

                    if (returnresult.Success)
                    {
                        return Json(returnresult);
                    }
                    else
                    {
                        return Json(result);
                    }
                    
                   
                }
                catch (Exception ex)
                {
                    return Json(GetServerJsonResult<Groups>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Groups>());
            }
        }


    }
}
