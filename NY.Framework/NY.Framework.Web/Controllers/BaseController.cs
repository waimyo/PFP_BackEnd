using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using NY.Framework.Infrastructure.Logging;
using NY.Framework.Model.Repositories;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace NY.Framework.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected Logger logger;
        protected ProgramCodeEnum program;
        private Type type;
        private string pCodeFor_NONE;

        public IAuditService AuditService { get; set; }
        public IUserRepository userRepo { get; set; }
        public IRoleRepository roleRepo { get; set; }
        public IPermissionRepository permissionRepo { get; set; }
        public IMinistryRepository ministryRepo { get; set; }

        protected TType GetRequestParameter<TType>(string key)
        {
            TType val = default(TType);
            try
            {
                if (Request.Query[key].ToString() != null)
                {
                    string tmp = Request.Query[key].FirstOrDefault();
                    val = (TType)Convert.ChangeType(tmp, typeof(TType));
                }
            }
            catch (Exception ex)
            {
                val = default(TType);
            }
            return val;
        }

        protected QueryOptions<TViewModel> GetQueryOptions<TViewModel>() where TViewModel : BaseEntity
        {
            QueryOptions<TViewModel> op = new QueryOptions<TViewModel>();
            string sortOrder = GetRequestParameter<string>("so");
            int pageSize = GetRequestParameter<int>("ps");
            int pageNumber = GetRequestParameter<int>("pn");
            if (pageSize == 0)
            {
                pageSize = 10;
            }
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            //Find Order Column
            var sortColumnDir = GetRequestParameter<string>("order[0][dir]");
            ////e.g sc=a&so=asc&ps=10&pn=1
            int skip = 0;
            try
            {
                if (pageNumber > 0)
                {
                    skip = (pageNumber - 1) * pageSize;
                }
            }
            catch (Exception ex)
            {
            }
            op.fromPage = pageNumber;
            op.fromRecord = skip;
            op.recordPerPage = pageSize;
            return op;
        }

        protected JqueryDataTableQueryOptions<TViewModel> GetOptionForScroll<TViewModel>() where TViewModel : BaseEntity
        {
            JqueryDataTableQueryOptions<TViewModel> op = new JqueryDataTableQueryOptions<TViewModel>();
            // Skiping number of Rows count  
            var start = Request.Query["start"].FirstOrDefault();
            if (!string.IsNullOrEmpty(start))
            {
                op.Start = Convert.ToInt32(start);
            }
            else
            {
                op.Start = 0;
            }
            op.Length = Convert.ToInt32(10);
            op.SearchValue = Request.Query["search"].FirstOrDefault();
            op.SortOrder = SortOrder.ASC;
            op.SortColumnsName = new List<string>();
            op.SortColumnsName.Add("id");
            op.SortBy = new List<Func<TViewModel, object>>();
            return op;
        }

        public NY.Framework.Infrastructure.JqueryDataTableQueryOptions<TEntity> GetJQueryDataTableQueryOptions<TEntity>() where TEntity : BaseEntity
        {
            NY.Framework.Infrastructure.JqueryDataTableQueryOptions<TEntity> queryOption = new NY.Framework.Infrastructure.JqueryDataTableQueryOptions<TEntity>();
            // Request.Query["First"]
            queryOption.Draw = Request.Query["draw"].FirstOrDefault();
            // Skiping number of Rows count  
            var start = Request.Query["start"].FirstOrDefault();
            // Paging Length 10,20  
            var pageSize = Request.Query["length"].FirstOrDefault();

            if (!string.IsNullOrEmpty(pageSize))
            {
                queryOption.Length = Convert.ToInt32(pageSize);
            }
            else
            {
                queryOption.Length = 10;
            }
            if (!string.IsNullOrEmpty(start))
            {
                queryOption.Start = Convert.ToInt32(start);
            }
            else
            {
                queryOption.Start = 0;
            }
            // Sort Column Name  
            var SortColumn = Request.Query["sortBy"].FirstOrDefault();
            queryOption.SortColumnsName = new List<string>();
            queryOption.SortColumnsName.Add(SortColumn);
            // Sort Column Direction ( asc ,desc)  
            var SortColumnDirection = Request.Query["sortOrder"].FirstOrDefault();
            queryOption.SortOrder = SortOrder.ASC;
            if (!string.IsNullOrEmpty(SortColumnDirection))
            {
                if (SortColumnDirection == "desc")
                {
                    queryOption.SortOrder = SortOrder.DESC;
                }
            }
            // Search Value from (Search box)  
            queryOption.SearchValue = Request.Query["search"].FirstOrDefault();
            return queryOption;
        }

        public BaseController(Type type, ProgramCodeEnum programCode)
        {
            logger = new Logger(type);
            this.program = programCode;
        }

        protected BaseController(Type type, string pCodeFor_NONE)
        {
            this.type = type;
            this.pCodeFor_NONE = pCodeFor_NONE;
        }

        protected int GetMinistryId()
        {
            User user = GetLoggedInUser();
            int ministryId = 0;
            if (user != null)
            {
                ministryId = user.ministry_id;
            }
            return ministryId;
        }

        protected bool IsDefaultUser()
        {
            bool isdefault = false;
            User user = GetLoggedInUser();
            if (user != null)
            {
                isdefault = user.Role.isdefault;
            }
            return isdefault;
        }

        protected Ministry MinistryData()
        {
            Ministry ministry = new Ministry();
            int ministryId = GetMinistryId();
            if (ministryId > 0)
            {
                ministry = ministryRepo.Get(ministryId);
            }
            return ministry;
        }

        protected User GetLoggedInUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                string id = User.Identity.Name;
                User u = userRepo.Get(Convert.ToInt32(id));
                if (u != null)
                {
                    return u;
                }
                //var currentDate = DateTime.Now.Date;                
            }
            return null;
        }
        protected int GetLoggedInId()
        {
            User user = GetLoggedInUser();
            int id = 0;
            if (user != null)
            {
                id = user.ID;
            }
            return id;
        }
        protected int GetLoggedInRoleId()
        {
            User user = GetLoggedInUser();
            int id = 0;
            if (user != null)
            {
                id = user.role_id;
            }
            return id;
        }

        public bool Authorize(AuthorizeAction action)
        {
            Role userRole = null;
            if (User.Identity.IsAuthenticated)
            {
                User u = GetLoggedInUser();
                userRole = u.Role;
            }
            else
            {
                userRole = roleRepo.getAnonymousRole();
            }
            if (userRole != null)
            {
                List<Permission> permissionlist = permissionRepo.Get();
                Permission permission = permissionlist.FirstOrDefault(p => p.program.code.Equals((int)program) && p.role_id.Equals(userRole.ID) && (p.CreateOrUpdate == action || p.View.Equals(action) || p.Delete.Equals(action) || p.Print.Equals(action)));
                if (permission != null)
                {
                    return true;
                }
            }
            return false;
        }
        
        protected CommandResult<TEntity> GetAccessDeniedJsonResult<TEntity>() where TEntity : BaseEntity
        {
            CommandResult<TEntity> errorResult = new CommandResult<TEntity>();
            errorResult.Success = false;
            errorResult.ErrorCode = (int)ErrorCodes.ACCESS_DENIED;
            errorResult.Messages.Add(Constants.AccessedDenised);
            return errorResult;
        }

        protected CommandResult<TEntity> GetForbideensonResult<TEntity>() where TEntity : BaseEntity
        {
            CommandResult<TEntity> errorResult = new CommandResult<TEntity>();
            errorResult.Success = false;
            errorResult.ErrorCode = (int)ErrorCodes.FORBIDDEN;
            errorResult.Messages.Add(Constants.Forbideen);
            return errorResult;
        }

        protected CommandResult<TEntity> GetServerJsonResult<TEntity>() where TEntity : BaseEntity
        {
            CommandResult<TEntity> errorResult = new CommandResult<TEntity>();
            errorResult.Success = false;
            errorResult.ErrorCode = (int)ErrorCodes.SERVER_ERROR;
            errorResult.Messages.Add(Constants.ServerError);
            return errorResult;
        }

        #region Helpers

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion Helpers


        #region Audit
        
         protected void AuditLog(string action, string program)
         {
            if (User.Identity.IsAuthenticated)
            {
                //Task.Run(() =>
                //{
                User user = GetLoggedInUser();
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string ip = Request.Headers["X-Real-IP"].FirstOrDefault();
                if (ip == null)
                {
                    ip = "";
                }
                //var ip = System.Net.Dns.GetHostAddressesAsync(System.Net.Dns.GetHostName());
                //var ip = host
                //    .AddressList
                //    .FirstOrDefault(i => i.AddressFamily == AddressFamily.InterNetwork);
                //var ip =HttpContext.Connection.RemoteIpAddress;
                //string hostName = Dns.GetHostName(); // Retrive the Name of HOST
               
                //string ip = Dns.GetHostByName(hostName).AddressList[1].ToString();
                Model.Entities.Audit audit = new Model.Entities.Audit();
                //audit.UserId = id;
                audit.page_accessed = program;
                    audit.role_id = user.role_id;
                    audit.action = action;
                    audit.created_by = user.ID;
                    audit.created_date = DateTime.Now;
                    audit.ip_address = ip;
                    //audit.ip_address = ip.ToString();
                    audit.deleted = false;
                    AuditService.Create(audit);
                //});
            }
        }

        #endregion


    }
}