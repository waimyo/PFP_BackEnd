using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : BaseController
    {
        IPermissionRepository pRepo;
        //PermissionMapper mapper;

        public MenuController(IPermissionRepository _pRepo)
            : base(typeof(MenuController), Application.ProgramCodeEnum.Dashboard)
        {
            this.pRepo = _pRepo;
            //mapper = new PermissionMapper();
        }

        [HttpGet]
        [Route("GetAllMenuPermission/")]
        public JsonResult GetAllMenuPermission()
        {
            User u = GetLoggedInUser();
            //u.role_id = 1;
            List<Permission> permissionlists = new List<Permission>();
            List<MenuPermissionViewModel> models = new List<MenuPermissionViewModel>();
            if (u != null)
            {
                permissionlists = pRepo.GetPermissions(u.role_id);
                foreach (var p in permissionlists)
                {
                    if (p.program.parent == 0)
                    {
                        MenuPermissionViewModel model = new MenuPermissionViewModel();
                        model.children = new List<SubMenuViewModel>();
                        model.program_name = p.program.program_name;
                        model.href = p.program.href;
                        model.icon = p.program.icon;
                        model.view = Convert.ToInt32(p.View);
                        model.saveorUpdate = Convert.ToInt32(p.CreateOrUpdate);
                        model.delete = Convert.ToInt32(p.Delete);
                        model.print = Convert.ToInt32(p.Print);
                        model.text = p.program.name;

                        List<Permission> childs = permissionlists.Where(c => c.program.parent == p.program.ID && c.program.status.Equals(true)).ToList();
                        if (childs.Count > 0)
                        {
                            foreach (var ch in childs)
                            {
                                SubMenuViewModel childMenu = new SubMenuViewModel();
                                childMenu.program_name = ch.program.program_name;
                                childMenu.href = ch.program.href;
                                childMenu.text = ch.program.name;
                                childMenu.icon = ch.program.icon;
                                childMenu.view = Convert.ToInt32(ch.View);
                                childMenu.saveorUpdate = Convert.ToInt32(ch.CreateOrUpdate);
                                childMenu.delete = Convert.ToInt32(ch.Delete);
                                childMenu.print = Convert.ToInt32(ch.Print);
                                model.children.Add(childMenu);
                            }
                        }
                        models.Add(model);
                    }
                }
            }
            return Json(models);
        }
    }
}
