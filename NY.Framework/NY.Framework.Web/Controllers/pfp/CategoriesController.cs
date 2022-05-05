using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
   // [Authorize]
    [Route("api/[controller]")]    
    [ApiController]
    public class CategoriesController : BaseController
    {
              
            ICategoriesRepository repo;
            ICategoriesService serv;
            CategoriesMapper mapper;
        
        public CategoriesController(ICategoriesRepository repo, ICategoriesService serv       
 )
         : base(typeof(CategoriesController), Application.ProgramCodeEnum.CATEGORY)
        {
            this.repo = repo;
            this.serv = serv;
            mapper = new CategoriesMapper();
        }


        // GET: api/<controller>
        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                
               JQueryDataTablePagedResult<CategoriesViewModel> vmlist = GetAllData();
                AuditLog(nameof(AuditAction.QUERY), nameof(ProgramCodeEnum.CATEGORY));
                return Json( vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Categories>());
            }
        }
        public JQueryDataTablePagedResult<CategoriesViewModel> GetAllData()
        {
           
            JqueryDataTableQueryOptions<Categories> queryOption = GetJQueryDataTableQueryOptions<Categories>();
            queryOption = mapper.Preparetorepository(queryOption);
           
            if (queryOption.FilterBy == null)
            {
                queryOption.FilterBy = (a => a.IsDeleted == false);
            }
            else
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (a => a.IsDeleted == false));
            }
            JQueryDataTablePagedResult<Categories> list = repo.GetPagedResults(queryOption);
            JQueryDataTablePagedResult<CategoriesViewModel> vmlist = mapper.MapModelToListViewModel(list);
            return vmlist;           

        }       

        // POST api/<controller>
        [HttpPost]
        public IActionResult SaveOrUpdate(CategoriesViewModel viewModel)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    User user = GetLoggedInUser();
                    CommandResult<Categories> result = new CommandResult<Categories>();
                    Categories cat = new Categories();
                    if (viewModel.id > 0)
                    {
                        cat = repo.Get(viewModel.id);
                        cat = mapper.MapEntityToViewModel(cat, viewModel);
                        cat.ModifiedBy = user.ID;
                        cat.ModifiedDate = DateTime.Now;
                        result = serv.CreateOrUpdate(cat);
                        if (result.Success)
                        {
                            AuditLog(nameof(AuditAction.UPDATE), nameof(ProgramCodeEnum.CATEGORY));
                        }
                    }
                    else
                    {
                        cat = mapper.MapEntityToViewModel(cat, viewModel);
                        cat.CreatedBy = user.ID;
                        cat.CreatedDate = DateTime.Now;
                        result = serv.CreateOrUpdate(cat);
                        AuditLog(nameof(AuditAction.ADD), nameof(ProgramCodeEnum.CATEGORY));
                    }
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(GetServerJsonResult<Categories>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Categories>());
            }

        }

        [HttpGet]
        [Route("GetById")]
        public JsonResult GetById(int id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
            Categories cate = repo.Get(id);
            return Json(cate);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Categories>());
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            CommandResult<Categories> result = new CommandResult<Categories>();
            if (Authorize(AuthorizeAction.DELETE))
            {
            Categories categories = repo.Get(id);
            if (categories != null)
            {
                result = serv.Delete(categories);

                if (result.Success)
                {
                    AuditLog(nameof(AuditAction.DELETE), nameof(ProgramCodeEnum.CATEGORY));
                }
            }
            return Json(result);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Categories>());
            }
        }
    }
}