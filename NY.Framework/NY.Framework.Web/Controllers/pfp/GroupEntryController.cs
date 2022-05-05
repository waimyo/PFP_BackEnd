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
    public class GroupEntryController : BaseController
    {
        IGroupRepository gpRepo;
        IGroupService gpService;
        GroupMapper mapper;
        IView_DataInfoRepository dataRepo;
        IGroupMemberRepository gpmemberRepo;
        IGroupMemberService gpmemberService;
        IView_GroupListRepository glistRepo;
        DataMapper dmapper;

        ISP_GroupMemberListRepository gmlistRepo;

        public GroupEntryController(IGroupService _gpService, IGroupRepository _gpRepo, IView_DataInfoRepository _dataRepo,
            IGroupMemberRepository _gpmemberRepo, IGroupMemberService _gpmemberService,
            IView_GroupListRepository _glistRepo, ISP_GroupMemberListRepository _gmlistRepo)
            : base(typeof(GroupController), ProgramCodeEnum.MOBILE_GROUP_ENTRY)
        {
            this.gpRepo = _gpRepo;
            this.gpService = _gpService;
            mapper = new GroupMapper();
            this.dataRepo = _dataRepo;
            this.gpmemberRepo = _gpmemberRepo;
            this.gpmemberService = _gpmemberService;
            this.gmlistRepo = _gmlistRepo;
            this.glistRepo = _glistRepo;
            dmapper = new DataMapper();
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<DataListViewModel> vmlist = GetAllData();
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Groups>());
            }
        }

        public JQueryDataTablePagedResult<DataListViewModel> GetAllData()
        {
            NY.Framework.Infrastructure.JqueryDataTableQueryOptions<View_DataInfo> queryoption = GetJQueryDataTableQueryOptions<View_DataInfo>();
            GroupListViewModel search = MapSearchData();
            User user = GetLoggedInUser();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption, search, user.ID);
            queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, (a => a.ministry_id.Equals(user.ministry_id)));
            NY.Framework.Infrastructure.Pagination.JQueryDataTablePagedResult<View_DataInfo> list = dataRepo.GetPagedResults(queryoption);
            JQueryDataTablePagedResult<DataListViewModel> vmlist = dmapper.MapModelToListViewModel(list);
            return vmlist;
        }

        public GroupListViewModel MapSearchData()
        {
            GroupListViewModel model = new GroupListViewModel();
            model.name = Request.Query["name"].ToString();
            var department = Request.Query["department_id"].ToString();
            if (!string.IsNullOrEmpty(department))
            {
               
                    model.department_id = Convert.ToInt32(department);
            }
            var service = Request.Query["service_id"].ToString();
            if (!string.IsNullOrEmpty(service))
            {
                    model.service_id = Convert.ToInt32(service);
            }
            model.fromdate = Request.Query["fromdate"].ToString();
            model.todate = Request.Query["todate"].ToString();

            var uploadedby = Request.Query["uploadedby"].ToString();
            if (!string.IsNullOrEmpty(uploadedby))
            {
                
                    model.uploadedby = Convert.ToInt32(uploadedby);
            }

            return model;
        }


        [HttpPost]
        public IActionResult SaveOrUpdate(GroupListViewModel entrymodel)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    //int uid = GetLoggedInId();
                    User user = GetLoggedInUser();
                    CommandResult<Groups> result = new CommandResult<Groups>();
                    Groups gp = new Groups();
                    gp = mapper.MapModelToEntity(gp, entrymodel);
                    gp.CreatedBy = user.ID;
                    gp.CreatedDate = DateTime.Now;
                    result = gpService.CreateOrUpdate(gp);

                    if (result.Success == true)
                    {
                        NY.Framework.Infrastructure.JqueryDataTableQueryOptions<View_DataInfo> queryoption = GetJQueryDataTableQueryOptions<View_DataInfo>();
                        //int minid = GetMinistryId();
                        queryoption = mapper.PrepareQueryOptionForRepository(queryoption, entrymodel, user.ID);
                        queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, (a => a.ministry_id.Equals(user.ministry_id)));
                        List<View_DataInfo> datalist = dataRepo.GetListWithFilter(queryoption);
                        //List<Data> datalist = dataRepo.FindByDataList(minid, Convert.ToInt32(entrymodel.department_id), entrymodel.department, Convert.ToInt32(entrymodel.service_id), entrymodel.service, entrymodel.fromdate.ToString(), entrymodel.todate, Convert.ToInt32(entrymodel.uploadedby), entrymodel.uploadedbyname);
                        CommandResult<GroupMember> memberresult = new CommandResult<GroupMember>();
                        List<GroupMember> lists = mapper.MapViewModelToEntity(datalist, gp.ID, user.ID);
                        memberresult = gpmemberService.CreateOrUpdateList(lists);
                        AuditLog(nameof(AuditAction.ADD), nameof(ProgramCodeEnum.MOBILE_GROUP_ENTRY));
                        //}
                    }




                    return Json(result);
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
