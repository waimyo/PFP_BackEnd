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
    //[Authorize]
    public class AccountController : BaseController
    {
        public AccountController(IUserService userService): base(typeof(AccountController), ProgramCodeEnum.Dashboard)
        {
            //this.UserService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync();
            this.ViewData["ReturnUrl"] = returnUrl;
            if(returnUrl != null)
            {
                return RedirectToAction(nameof(AccountController.Login), "Account");
            }
            return View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model, string returnUrl, bool success)
        //{
        //    logger.LogInfo("Logging in...");
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (ModelState.IsValid)
        //    {
        //        // This doesn't count login failures towards account lockout
        //        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //        var result = UserService.FindByUserNameAndPassword(model.UserName, model.Password);
        //        if (result == null)
        //        {

        //            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //            return View(model);
        //        }
        //        else
        //        {
        //            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        //            identity.AddClaim(new Claim(ClaimTypes.Name, result.Name));
        //            identity.AddClaim(new Claim(ClaimTypes.Role, result.Role.Name));


        //            var principal = new ClaimsPrincipal(identity);
        //            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //            ViewBag.Username = result.Name;
        //            string Message = "Login Successfully";
        //            success = true;
        //            if (Url.IsLocalUrl(returnUrl))
        //            {
        //                return Redirect(returnUrl);
        //            }
        //            else
        //            {
        //                return Json(new { returnUrl = "Home/Index", Message, success });
        //            }
                   
        //            //return RedirectToLocal(returnUrl);
        //        }
                
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private  IActionResult RedirectToLocal(string returnUrl)
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

        #endregion
    }
}
