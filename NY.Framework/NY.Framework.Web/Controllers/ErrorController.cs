using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;

namespace NY.Framework.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController() : base(typeof(ErrorController), ProgramCodeEnum.Dashboard)
        {

        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            string ExceptionMessage = "";
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature.Error is System.IO.FileNotFoundException)
            {
                ExceptionMessage = "File error thrown";
            }

            //logger.Log(exceptionHandlerPathFeature.Error);

            return View();
        }

        public IActionResult NotFound()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult AccessDenied()
        {
            ViewData["Message"] = NY.Framework.Infrastructure.Constants.AccessedDenised;
           // SetActiveMenu(MainNav.DASHBOARD, SubMenuLevel1.NONE, SubMenuLevel2.NONE, SubMenuLevel3.NONE, 0);
            return View();
        }

        public IActionResult SettingAccessDenied()
        {
            ViewData["Message"] = NY.Framework.Infrastructure.Constants.AccessedDenised;
          //  SetActiveMenu(MainNav.SETTING, SubMenuLevel1.NONE, SubMenuLevel2.NONE, SubMenuLevel3.NONE, 0);
            return View();
        }

        public IActionResult SiteOffline()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        
    }
}
