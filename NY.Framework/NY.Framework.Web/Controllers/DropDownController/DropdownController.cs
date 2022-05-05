using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.DropDownController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownController : BaseController
    {
        IDepartmentRepository deptRepo;
        IMinistryRepository minRepo;
        IServiceRepository servRepo;
        ILocationRepository locationRepo;
        ISmsShortCodeRepository smsShortCodeRepo;
        IGroupRepository groupRepo;
        IDataRepository dataRepo;
        IUserRepository userRepo;
        ICategoriesRepository catRepo;
        IView_ParticipantByChattingRepository participantbychattingRepo;

        public DropDownController(IDepartmentRepository deptRepo,
            IMinistryRepository minRepo, IServiceRepository servRepo, 
            ILocationRepository locationRepo, IDataRepository _dataRepo,
            IUserRepository _userRepo, ISmsShortCodeRepository _smsShortCodeRepo,
            IGroupRepository _groupRepo,ICategoriesRepository catRepo,
            IView_ParticipantByChattingRepository _participantbychattingRepo)
           : base(typeof(DropDownController), Application.ProgramCodeEnum.CATEGORY)
        {
            this.deptRepo = deptRepo;
            this.minRepo = minRepo;
            this.servRepo = servRepo;
            this.locationRepo = locationRepo;
            smsShortCodeRepo = _smsShortCodeRepo;
            groupRepo = _groupRepo;
            this.dataRepo = _dataRepo;
            this.userRepo = _userRepo;
            this.catRepo = catRepo;
            this.participantbychattingRepo = _participantbychattingRepo;
        }             
            
        [HttpGet]
        [Route("GetMinistry/")]
        public JsonResult GetMinistry()
        {
            List<Ministry> list = minRepo.GetMinistryList();
            return Json(list);
        }

        [HttpGet]
        [Route("GetMinistryAccountNoTraining/")] //GetMinistryNoTraining
        public JsonResult GetMinistryNoTraining()
        {
            //List<Ministry> list = minRepo.GetMinistryList().Where(m => m.istraining == false).ToList();
            List<User> list = userRepo.GetNoTrainingMinistryuser();
            return Json(list);
        }

        [HttpGet]
        [Route("GetMinistryAccount/")]
        public JsonResult GetMinistryAccount()
        {
            List<User> list = userRepo.GetMinistryuser();
            return Json(list);
        }

        [HttpGet]
        [Route("GetDepartment/")]
        public JsonResult GetDepartment(int ministryid)
        {
            List<Department> list = deptRepo.GetDepartment(ministryid);
            return Json(list);
        }

        [HttpGet]
        [Route("GetService/")]
        public JsonResult GetService(int deptid)
        {
            List<Service> list = servRepo.GetService(deptid);
            return Json(list);
        }

        [HttpGet]
        [Route("GetAllStateDivision")]
        public JsonResult GetAllStateDivision()
        {
            List<Location> statedivlist = locationRepo.GetAllStateDivision();
            return Json(statedivlist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        
        [HttpGet]
        [Route("GetAllDistrict")]
        public JsonResult GetAllDistrict(int statedivid)
        {
            Location loc = new Location();
            //loc.Name = "Unknown";
            //loc.ID = (int)LocationType.UNKNOWN;
            List<Location>  districtlist = locationRepo.GetAllDistrictByStateDivisonId(statedivid);
            //districtlist.Add(loc);
            return Json(districtlist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        [Route("GetAllTownship")]
        public JsonResult GetAllTownship(int districtid)
        {
            List<Location> townshiplist = locationRepo.GetAllTownshipByDistrictId(districtid);
            return Json(townshiplist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        [Route("GetAllTownshipByMinistry")]
        public JsonResult GetAllTownshipByMinistry(int ministry_id)
        {
            List<Location> townshiplist = locationRepo.GetAllTownshipByMinistryId(ministry_id);
            return Json(townshiplist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        //[HttpGet]
        //[Route("GetAllCPUAccount/")]
        //public JsonResult GetAllCPU(int ministry_id)
        //{
        //    List<User> list = userRepo.GetUserByMinistry(ministry_id);
        //    return Json(list);
        //}

        [HttpGet]
        [Route("GetSmsShortCode")]
        public JsonResult GetSmsShortCode()
        {
            List<SmsShortCode> smscodelist = smsShortCodeRepo.Get();
            return Json(smscodelist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        [Route("GetGroup")]
        public JsonResult GetGroup()
        {
            List<Groups> grouplist = groupRepo.Get().Where(g=>g.IsDeleted==false).ToList();
            return Json(grouplist, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        [HttpGet]
        [Route("GetCreatedDateList/")]
        public JsonResult GetCreatedDateList(int ministryid)
        {
            List<Data> list = dataRepo.GetCreatedDate(ministryid);

            List<DataListViewModel> vmlist = new List<DataListViewModel>();
            foreach (var d in list)
            {
                DataListViewModel vm = new DataListViewModel();
                vm.created_date = string.Format("{0:dd/MM/yyyy}", d.CreatedDate);
                vmlist.Add(vm);
            }
            return Json(vmlist);
            //return Json(list);
        }

        [HttpGet]
        [Route("GetUserList/")]
        public JsonResult GetUserList(int ministryid)
        {
            List<User> ulist = new List<User>();
            List<Data> dlist = dataRepo.Get().Where(d => d.CreatedBy == ministryid || d.CreatedByUser.parent_id == ministryid).ToList();
            dlist = dlist.GroupBy(p => p.CreatedBy).Select(g => g.First()).ToList();
            foreach (var u in dlist)
            {
                User user = new User();
                user = userRepo.FindById(u.CreatedBy);
                ulist.Add(user);
            }
            
            return Json(ulist);
        }

        [Route("GetCategory/")]
        public JsonResult GetCategory()
        {
            List<Categories> list = catRepo.GetCategoriesList();
            return Json(list);
        }

        [Route("GetDataList/")]
        public JsonResult GetDataList()
        {
            int uid = GetLoggedInId();
            List<Data> list = dataRepo.GetDataList(uid);
            foreach(var l in list)
            {
                l.name = l.name + " (" + l.ID + ")";
            }
            list.Reverse();
            return Json(list);
        }

        [Route("GetAllCPUAccountByMinistry/")]
        public JsonResult GetCPUAccountByMinistry(int ministryid)
        {
            // cpu role id = 3
            List<User> list = userRepo.GetCPUAccountByMinistry(ministryid, 3);
            return Json(list);
        }

        [Route("GetRoleByNotDefault/")]
        public JsonResult GetRoleByNotDefault()
        {
            List<Role> list = roleRepo.GetRoleByNotDefault();
            return Json(list);
        }

        [Route("GetAllDEOAccount/")]
        public JsonResult GetAllDEOAccount()
        {            
            List<User> list = userRepo.GetDEOAccount(GetLoggedInId());
            return Json(list);
        }

        [Route("GetDEOAccountByChatting/")]
        public JsonResult GetDEOAccountByChattingId(int chat_id)
        {
            List<View_ParticipantByChatting> list = participantbychattingRepo.GetDEOAccountByChatting(chat_id);
            return Json(list);
        }

        [Route("GetUserNameCPU/")]
        public JsonResult GetUserNameforCPU(int parent_id)
        {
            string username = "";
            int count = 0;
            username = userRepo.Get(parent_id).username;
            count = userRepo.GetCPUAccountCount(parent_id) + 1;
            username = username + "cpu" + count;
            return Json(username);
        }

        [Route("GetUserNameDEO/")]
        public JsonResult GetUserNameforDEO(int parent_id)
        {
            string username = "";
            int count = 0;
            User user = userRepo.Get(parent_id);            
            count = userRepo.GetCPUAccountCount(parent_id) + 1;
            if (count >= 10)
            {
                username = user.ParentUser.username + "deo" + "c" + Regex.Match(user.username, @"\d+\.*\d*").Value + count;
            }
            else
            {
                username = user.ParentUser.username + "deo" + "c" + Regex.Match(user.username, @"\d+\.*\d*").Value + "0" + count;
            }
            
            return Json(username);
        }
    }
}
