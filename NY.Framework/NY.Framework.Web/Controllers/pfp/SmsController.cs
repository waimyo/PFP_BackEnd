using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application.Interfaces;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Web.Mappers;
using NY.Framework.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.pfp
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : BaseController
    {
        ISmsRepository repo;
        ISmsService serv;
        SmsMapper mapper;
        ICategoriesRepository catRepo;

        public SmsController(ISmsRepository repo, ISmsService serv, ICategoriesRepository catRepo
 )
         : base(typeof(SmsController), Application.ProgramCodeEnum.UNCATEGORIZED_SMS)
        {
            this.repo = repo;
            this.serv = serv;
            this.catRepo = catRepo;
            mapper = new SmsMapper();
        }


        // GET: api/<controller>
        [HttpGet]
        public JsonResult GetAll()
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<SmsViewModel> vmlist = GetAllData();
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Sms>());
            }
        }
        public JQueryDataTablePagedResult<SmsViewModel> GetAllData()
        {

            JqueryDataTableQueryOptions<Sms> queryOption = GetJQueryDataTableQueryOptions<Sms>();
           
            queryOption.FilterBy =  c => c.Direction == (int)SmsDirection.Received && c.Message_Type == (int)MessageType.Valid_Reply && (c.Category_Id == null || c.Category_Id == 0);
            queryOption = GetQueryOption(queryOption);
            queryOption = mapper.Preparetorepository(queryOption);
            JQueryDataTablePagedResult<Sms> list = repo.GetPagedResults(queryOption);
            List<Categories> lst = catRepo.GetCategoriesList();
            int cst = lst.First().ID;
            JQueryDataTablePagedResult<SmsViewModel> vmlist = mapper.MapModelToListViewModel(list,cst);
            return vmlist;
        }

        [HttpGet("GetAllForCategorized")]
        public JsonResult GetAllForCategorized()

        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                JQueryDataTablePagedResult<SmsViewModel> vmlist = GetAllDataForCategorized();
                return Json(vmlist);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Sms>());
            }
        }
        public JQueryDataTablePagedResult<SmsViewModel> GetAllDataForCategorized()
        {

            JqueryDataTableQueryOptions<Sms> queryOption = GetJQueryDataTableQueryOptions<Sms>();
            queryOption = mapper.Preparetorepository(queryOption);
            queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy,
            c => c.Direction == (int)SmsDirection.Received && c.Message_Type == (int)MessageType.Valid_Reply && (c.Category_Id != null || c.Category_Id > 0));
            queryOption = GetQueryOption(queryOption);
            JQueryDataTablePagedResult<Sms> list = repo.GetPagedResults(queryOption);
            JQueryDataTablePagedResult<SmsViewModel> vmlist = mapper.MapModelToListViewModelForCategorized(list);
            return vmlist;
        }

        private JqueryDataTableQueryOptions<Sms> GetQueryOption(JqueryDataTableQueryOptions<Sms> queryoption)
        {
            User loginuser = GetLoggedInUser();            
            if (loginuser.role_id == 3)
            {
                queryoption.FilterBy1 = x => x.CreatedBy.Equals(loginuser.ID);
                queryoption.FilterBy1 = LinqExpressionHelper.AppendOr(queryoption.FilterBy1, x => x.CreatedByUser.parent_id.Equals(loginuser.ID));
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, queryoption.FilterBy1);
            }
            if (loginuser.role_id == 2)
            {
                queryoption.FilterBy = LinqExpressionHelper.AppendAnd(queryoption.FilterBy, x => x.CreatedByUser.ministry_id.Equals(loginuser.ministry_id));
            }
            return queryoption;
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult SaveOrUpdate(SmsViewModel viewModel)
        {
            if (Authorize(AuthorizeAction.CREATEORUPDATE))
            {
                try
                {
                    CommandResult<Sms> result = new CommandResult<Sms>();
                    Sms sms = new Sms();
                    if (viewModel.selectrows != null)
                    {
                        foreach (var rows in viewModel.selectrows)
                        {
                            sms = repo.Get(rows.id);
                            if (sms != null)
                            {
                                if (viewModel.category_id > 0)
                                {
                                    sms.Category_Id = viewModel.category_id;
                                    sms.Categorized_Time = DateTime.Now;
                                    sms.Categorized_by = GetLoggedInId();
                                }                                
                                sms.ModifiedBy = GetLoggedInId();
                                sms.ModifiedDate = DateTime.Now;
                                result = serv.CreateOrUpdateCommand(sms);
                            }
                        }

                    }
                    if(viewModel.id>0)
                    {
                        sms = repo.Get(viewModel.id);
                        if (sms != null)
                        {
                            sms.Category_Id = null;
                            sms.Categorized_Time = DateTime.Now;
                            sms.Categorized_by = GetLoggedInId();
                        }
                        sms.ModifiedBy = GetLoggedInId();
                        sms.ModifiedDate = DateTime.Now;
                        result = serv.CreateOrUpdateCommand(sms);
                    }
                    return Json(result);


                    //CommandResult<Sms> result = new CommandResult<Sms>();
                    //Sms sms = new Sms();
                    //if (viewModel.id > 0)
                    //{
                    //    sms = repo.Get(viewModel.id);
                    //    if (sms != null)
                    //    {
                    //        if (viewModel.category_id > 0)
                    //        {
                    //            sms.Category_Id = viewModel.category_id;
                    //            sms.Categorized_Time = DateTime.Now;
                    //            sms.Categorized_by = GetLoggedInId();
                    //        }
                    //        else
                    //        {
                    //            sms.Category_Id = null;
                    //            sms.Categorized_Time = DateTime.Now;
                    //            sms.Categorized_by = GetLoggedInId();
                    //        }

                    //        sms.ModifiedBy = GetLoggedInId();
                    //        sms.ModifiedDate = DateTime.Now;
                    //        result = serv.CreateOrUpdateCommand(sms);
                    //    }
                    //}

                    //return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(GetServerJsonResult<Sms>());
                }
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<Sms>());
            }

        }
    }
}
