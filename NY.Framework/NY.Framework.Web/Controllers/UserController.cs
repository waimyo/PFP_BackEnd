using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Logging;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;

namespace NY.Framework.Web.Controllers.Settings
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]    
    public class UserController : BaseController
    {
        UserMapper mapper;
        IUserService userService;
        readonly AppSettings _appSettings;
        Logger logger;

        public UserController(IUserRepository _userRepository, IUserService _userService, IOptions<AppSettings> appSettings)
            : base(typeof(UserController), Application.ProgramCodeEnum.ACCOUNT)
        {
            this.userRepo = _userRepository;
            this.userService = _userService;
            mapper = new UserMapper();
            _appSettings = appSettings.Value;
            logger = new Logger(typeof(UserController));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<UserListViewModel> models = GetAllData();

                User user = GetLoggedInUser();
                JqueryDataTableQueryOptions<User> queryOptions = GetJQueryDataTableQueryOptions<User>();
                queryOptions.FilterBy = u => u.IsDeleted.Equals(false) && u.Role.isdefault.Equals(false) && u.status.Equals(true);
                queryOptions = mapper.PrepareQueryOptionForRepository(queryOptions, MapSearchData());
                if (!IsDefaultUser())
                {
                    queryOptions.FilterBy = LinqExpressionHelper.AppendAnd(queryOptions.FilterBy, u => u.parent_id.Equals(GetLoggedInId()));
                }
                List<User> alluserlist = userRepo.GetListWithFilter(queryOptions);

                int mcount = 0, ccount = 0, dcount = 0;
                if (alluserlist != null)
                {
                    mcount = alluserlist.Where(a => a.role_id == 2).ToList().Count();
                    ccount = alluserlist.Where(a => a.role_id == 3).ToList().Count();
                    dcount = alluserlist.Where(a => a.role_id == 4).ToList().Count();
                }
                
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.USER));
                
                return Json(new { models, mcount, ccount, dcount });
                //return Json(models);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<User>());
            }
        }

        public JQueryDataTablePagedResult<UserListViewModel> GetAllData()
        {            
            User user = GetLoggedInUser();            
            JqueryDataTableQueryOptions<User> queryOptions = GetJQueryDataTableQueryOptions<User>();
            queryOptions.FilterBy = u => u.IsDeleted.Equals(false) && u.Role.isdefault.Equals(false);
            queryOptions = mapper.PrepareQueryOptionForRepository(queryOptions, MapSearchData());
            if (!IsDefaultUser())
            {
                queryOptions.FilterBy = LinqExpressionHelper.AppendAnd(queryOptions.FilterBy, u => u.parent_id.Equals(GetLoggedInId()));
            }
            JQueryDataTablePagedResult<User> list = userRepo.GetPagedResults(queryOptions);
          
            JQueryDataTablePagedResult<UserListViewModel> vmlist = mapper.MapModelToListViewModel(list);
          
                
            
            return vmlist;
        }

        private UserFilterViewModel MapSearchData()
        {
            UserFilterViewModel vm = new UserFilterViewModel();
            vm.role_id = Convert.ToInt32(Request.Query["role_id"]);
            vm.search = Request.Query["search"].ToString();
            vm.fromdate = Request.Query["fromdate"].ToString();
            vm.todate = Request.Query["todate"].ToString();
            string ministry_id = Request.Query["ministry_id"].ToString();
            if (!string.IsNullOrEmpty(ministry_id))
            {
                vm.ministry_id = Convert.ToInt32(ministry_id);
                if (vm.ministry_id > 0)
                {
                    vm.ministry_id = userRepo.Get(vm.ministry_id).ministry_id;
                }
            }
            return vm;
        }


        //[HttpGet]
        //[Route("GetUserName")]
        //public JsonResult GetUserName()
        //{
        //    User u = GetLoggedInUser();
        //    string username = "";
        //    if (u != null)
        //    {
        //        username = u.name;
        //    }
        //    return Json(u);
        //}

        [HttpPost]
        [Route("LogOut")]
        public IActionResult LogOut()
        {
            CommandResult<User> result = new CommandResult<User>();
            result.Success = true;
            AuditLog(nameof(AuditAction.LOGOFF), "Logoff");
            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("CallBackFuntion")]
        public string CallBackFuntion()
        {
            MobileRegularExpression regularExpression = new MobileRegularExpression();
            //return Json(regularExpression.Checkoperator("09423676383"));
         ///   logger.LogDebug("Inside CallBackFunction");
            string id = Request.Query["id"].FirstOrDefault();
            string messagestaus = Request.Query["message_status"].FirstOrDefault();
            string err = Convert.ToInt32(Request.Query["err"].FirstOrDefault()).ToString();
            string id_smsc = Convert.ToInt32(Request.Query["id_smsc"].FirstOrDefault()).ToString();
            string text = Request.Query["text"].FirstOrDefault();
            string Direction = Request.Query["Direction"].FirstOrDefault();
            string Operator = Request.Query["Operator"].FirstOrDefault();
            logger.LogDebug("message status = " + messagestaus);
            logger.LogDebug("Inside CallBackFunction id= " + id +  " and error = " + err + " and id smsc = " + id_smsc + " and text = " + text +
                " and Direction = " + Direction + " and Operator = " + Operator);
            //  ControllerContext.HttpContext.Request.QueryString.key
            //return Json(id);
            return "ACK/Jasmin";
        }


        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    CommandResult<User> result = new CommandResult<User>();
        //    if (Authorize(AuthorizeAction.DELETE))
        //    {
        //        User user = userRepo.FindById(id);
        //        if (user != null)
        //        {
        //            result = userService.Delete(user);
        //        }
        //        return Json(result);
        //    }
        //    else
        //    {
        //        return Json(GetAccessDeniedJsonResult<User>());
        //    }
        //}

        [HttpPost]
        public IActionResult SaveOrUpdate(UserEntryViewModel vm)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                CommandResult<User> result = new CommandResult<User>();
                User user = new User();
                int userid = GetLoggedInId();
                if (vm.ministry_id == 0)
                {
                    vm.ministry_id = userRepo.Get(vm.parent_id).ministry_id;
                }                
                if (vm.Id > 0)
                {
                    user = userRepo.Get(vm.Id);
                    user = mapper.MapModelToEntity(vm, user);
                    user.ModifiedBy = userid;
                    user.ModifiedDate = DateTime.Now;
                    result = userService.CreateOrUpdate(user);
                    if (result.Success)
                    {
                        AuditLog(nameof(AuditAction.UPDATE), nameof(ProgramCodeEnum.USER));
                    }
                }
                else
                {
                    user = mapper.MapModelToEntity(vm, user);
                    user.CreatedBy = userid;
                    user.CreatedDate = DateTime.Now;
                    result = userService.CreateOrUpdate(user);
                    if (result.Success)
                    {
                        AuditLog(nameof(AuditAction.ADD), nameof(ProgramCodeEnum.USER));
                    }
                }
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<User>());
            }
        }

        [HttpPost]
        [Route("resetpassword")]
        public IActionResult ResetPassword(UserEntryViewModel vm)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {                
                CommandResult<User> result = new CommandResult<User>();
                User user = new User();
                if (vm.Id > 0)
                {
                    user = userRepo.Get(vm.Id);
                    user = mapper.MapUserToPasswordReset(vm, user);
                    result = userService.CreateOrUpdate(user);
                }
                //else if (vm.Id > 0 && !string.IsNullOrEmpty(vm.password))
                //{
                //    user = userRepo.Get(vm.Id);
                //    string pwd = PasswordHashHelper.HashPassword(vm.password);
                //    if (user.password == pwd)
                //    {
                //        user = mapper.MapUserToPasswordReset(vm, user);
                //        result = userService.CreateOrUpdate(user);
                //    }
                //    else
                //    {
                //        result.Success = false;
                //        result.Messages.Add("Current Password မှားနေပါသည်။");
                //        //result.Messages[0 = "Current Password မှားနေပါသည်။";
                //    }
                //}
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<User>());
            }
        }

        [HttpPost]
        [Route("suspendpassword")]
        public IActionResult SuspendPassword(UserEntryViewModel vm)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                CommandResult<User> result = new CommandResult<User>();
                User user = new User();
                if (vm.Id > 0)
                {
                    user = userRepo.Get(vm.Id);
                    user.status = vm.status;
                    result = userService.CreateOrUpdate(user);
                }
                return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<User>());
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int userid)
        {
            UserEntryViewModel vm = new UserEntryViewModel();
            if (Authorize(AuthorizeAction.VIEW))
            {
                User user = userRepo.Get(userid);
                if (user != null)
                {
                    vm = mapper.MapEntityToModel(vm, user);
                }
                return Json(vm);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<User>());
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public JsonResult Authenticate([FromBody]User model)
        {
            AuditLog(nameof(AuditAction.LOGIN), nameof(ProgramCodeEnum.ACCOUNT));
            var   user = userRepo.GetUserByUserNameAndPasswordAndMinistryId(model.name, model.password);           
             if (user == null)
            {
                return Json(new { msg = Constants.UserNamePasswordMessage });
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                int hrs = DateTime.UtcNow.Hour - 1;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name,user.ID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(24 - hrs), //DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
                user.password = null;
                UserEntryViewModel vm = new UserEntryViewModel();
                vm.Id = user.ID;
                vm.name = user.name;
                vm.username = user.username;
                vm.isFirstApiCall = true;
                vm.apiCallJwtExpire = tokenDescriptor.Expires.Value;
                vm.Token = user.Token;
                vm.role_id = user.role_id;
                vm.parent_id = user.parent_id;
                vm.ministry_id = user.ministry_id;
                if (user.Ministry != null)
                {
                    //vm.minlogo = user.Ministry.logo;                   
                    vm.minname = user.Ministry.name;
                    string filepath= Constants.MinistryLogoPath + user.Ministry.logo;
                    if (System.IO.File.Exists(filepath))
                    {
                        byte[] imageArray = System.IO.File.ReadAllBytes(filepath);
                        string base64Image = Convert.ToBase64String(imageArray);
                        vm.minlogo = base64Image;
                    }
                 
                }
                AuditLog(nameof(AuditAction.LOGIN), "Login");
                return Json(vm);
            }
        }
    }
}