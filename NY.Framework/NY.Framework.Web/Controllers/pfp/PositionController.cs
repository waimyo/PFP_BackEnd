using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NY.Framework.Application;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;

namespace NY.Framework.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionController : BaseController
    {
        public PositionController(): base(typeof(PositionController), ProgramCodeEnum.Dashboard)
        {
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            return null;
            //return Json(GetAccessDeniedJsonResult<Department>());
        }
    }
}
