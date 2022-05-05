using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Infrastructure.Utilities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Entities.Home;
using NY.Framework.Model.Repositories.Home;
using NY.Framework.Web.Mappers.Home;
using NY.Framework.Web.Models.Home;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.Home
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseController
    {
        IMonthlyCampaignCountRepository repo;
        ITotalCampaignCountRepository torepo;
        ISmsRepository srepo;
        INumberOfPeopleSentAndReceivedSMSRepository senrepo;
        IMonthlyNumberOfPeopleSentAndReceivedSMSRepository monthrepo;
        ITotalCorruptionCountsRepository corruRepo;
        IMonthlyNumberOfCorruptionReceivedSMSRepository monthcoruRepo;
        MonthlyCampaignCountMapper mapper;
        public HomeController(IMonthlyCampaignCountRepository repo, ITotalCampaignCountRepository torepo, ISmsRepository srepo, INumberOfPeopleSentAndReceivedSMSRepository senrepo, IMonthlyNumberOfPeopleSentAndReceivedSMSRepository monthrepo,
        ITotalCorruptionCountsRepository corruRepo,
        IMonthlyNumberOfCorruptionReceivedSMSRepository monthcoruRepo
        )
        : base(typeof(HomeController), Application.ProgramCodeEnum.Dashboard)
        {
            this.repo = repo;
            this.torepo = torepo;
            this.srepo = srepo;
            this.senrepo = senrepo;
            this.monthrepo = monthrepo;
            this.corruRepo = corruRepo;
            this.monthcoruRepo = monthcoruRepo;
            mapper = new MonthlyCampaignCountMapper();
        }
        // GET: api/<controller>
        [HttpGet("MonthlyCampaignCounts")]
        public JsonResult MonthlyCampaignCounts(string fromdate, string todate, int ministry_id, int user_id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                List<MonthlyCampaignCount> camplist = repo.GetMonthlyCount(fromdate, todate, ministry_id, user_id);
                //add 12 months in filterdates list
                List<string> filterdates = new List<string>();
                DateTime to_date = Convert.ToDateTime(todate);
                DateTime from_date = Convert.ToDateTime(fromdate);
                int toyear = to_date.Year;
                int month = Convert.ToInt32(to_date.Month);
                int fromyear = from_date.Year;
                int frommonth = Convert.ToInt32(from_date.Month);
                if (fromyear == toyear)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";

                        if (i.ToString().Length == 1)
                        {
                            tempmonth = "0" + i.ToString();
                        }
                        else
                        {
                            tempmonth = i.ToString();
                        }
                        filterdates.Add(string.Concat(fromyear + "-" + tempmonth));

                    }
                }
                else
                {

                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        if (i >= frommonth)
                        {
                            if (i.ToString().Length == 1)
                            {
                                tempmonth = "0" + i.ToString();
                            }
                            else
                            {
                                tempmonth = i.ToString();
                            }
                            filterdates.Add(string.Concat(fromyear + "-" + tempmonth));
                        }
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        var year = toyear - 1;
                        //if (i <= month)
                        //{
                        if (i.ToString().Length == 1)
                        {
                            tempmonth = "0" + i.ToString();
                        }
                        else
                        {
                            tempmonth = i.ToString();
                        }
                        filterdates.Add(string.Concat(year + "-" + tempmonth));
                        //}
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        if (i <= month)
                        {
                            if (i.ToString().Length == 1)
                            {
                                tempmonth = "0" + i.ToString();
                            }
                            else
                            {
                                tempmonth = i.ToString();
                            }
                            filterdates.Add(string.Concat(toyear + "-" + tempmonth));
                        }
                    }

                }
                if (filterdates.Count > 12)
                {
                    int k = filterdates.Count;
                    while (k != 12)
                    {
                        filterdates.RemoveAt(0);
                        k--;
                    }
                }

                //generate yearmonthlist from database
                List<string> yearmonthlist = new List<string>();
                foreach (var res in camplist)
                {
                    yearmonthlist.Add(res.yearmonths);

                }

                //check for all yearmonth,if yearmonth is not contain in databse list,add (count=0,yearmonth="2021-05") to database list
                for (int i = 0; i < filterdates.Count; i++)
                {

                    foreach (var res in camplist)
                    {

                        if (!yearmonthlist.Contains(filterdates[i]))
                        {
                            MonthlyCampaignCount campobj = new MonthlyCampaignCount();
                            campobj.counts = 0;
                            campobj.yearmonths = filterdates[i];
                            camplist.Add(campobj);
                            break;
                        }
                    }
                }
                camplist = camplist.OrderBy(c => c.yearmonths).ToList();
                if (camplist.Count > 12)
                {
                    int k = camplist.Count;
                    while (k != 12)
                    {
                        camplist.RemoveAt(0);
                        k--;
                    }
                }
                List<string> yearmonths = new List<string>();
                List<int> counts = new List<int>();
                foreach (var res in camplist)
                {
                    yearmonths.Add(res.yearmonths);
                    counts.Add(res.counts);
                }

                return Json(new { yearmonths, counts, filterdates });
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<MonthlyCampaignCount>());
            }
        }


        [HttpGet("GetAllForTotalCampaignCounts")]
        public JsonResult GetAllForTotalCampaignCounts(string fromdate, string todate, int ministry_id, int user_id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {

                TotalCampaignCountViewModel model = new TotalCampaignCountViewModel();
                TotalCampaignCounts total = torepo.GetTotalCounts(fromdate, todate, ministry_id, user_id);
                if (total != null)
                {
                    model.counts = total.counts;
                }
                return Json(model);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<TotalCampaignCounts>());
            }
        }
        [HttpGet("NumberOfPeopleSentAndReceivedSMS")]
        public JsonResult NumberOfPeopleSentAndReceivedSMS(string fromdate, string todate, int ministry_id, int user_id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {

                TotalCampaignCountViewModel model = new TotalCampaignCountViewModel();
                NumberOfPeopleSentAndReceivedSMS total = senrepo.GetSentAndReceivedCounts(fromdate, todate, ministry_id, user_id);

                if (total != null)
                {
                    model.receivedcounts = total.receivedcounts;
                    model.sentcounts = total.sentcounts;
                }

                return Json(model);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<NumberOfPeopleSentAndReceivedSMS>());
            }
        }

        [HttpGet("MonthlyNumberOfPeopleSentAndReceivedSMSCount")]
        public JsonResult MonthlyNumberOfPeopleSentAndReceivedSMSCount(string fromdate, string todate, int ministry_id, int user_id)
        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                List<MonthlyNumberOfPeopleSentAndReceivedSMS> total = monthrepo.GetMonthlySentAndReceivedCounts(fromdate, todate, ministry_id, user_id);

                List<string> yearmonths = new List<string>();
                List<int> receivedcountlist = new List<int>();
                foreach (var res in total)
                {
                    yearmonths.Add(res.months);
                    receivedcountlist.Add(res.receivedcounts);
                }
                List<string> FilterDate = new List<string>();

                DateTime to_date = Convert.ToDateTime(todate);
                DateTime from_date = Convert.ToDateTime(fromdate);
                int toyear = to_date.Year;
                int month = Convert.ToInt32(to_date.Month);
                int fromyear = from_date.Year;
                int frommonth = Convert.ToInt32(from_date.Month);
                if (fromyear == toyear)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";

                        if (i.ToString().Length == 1)
                        {
                            tempmonth = "0" + i.ToString();
                        }
                        else
                        {
                            tempmonth = i.ToString();
                        }
                        FilterDate.Add(string.Concat(fromyear + "-" + tempmonth));

                    }
                }
                else
                {

                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        if (i >= frommonth)
                        {
                            if (i.ToString().Length == 1)
                            {
                                tempmonth = "0" + i.ToString();
                            }
                            else
                            {
                                tempmonth = i.ToString();
                            }
                            FilterDate.Add(string.Concat(fromyear + "-" + tempmonth));
                        }
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        var year = toyear - 1;
                        //if (i <= month)
                        //{
                        if (i.ToString().Length == 1)
                        {
                            tempmonth = "0" + i.ToString();
                        }
                        else
                        {
                            tempmonth = i.ToString();
                        }
                        FilterDate.Add(string.Concat(year + "-" + tempmonth));
                        //}
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        if (i <= month)
                        {
                            if (i.ToString().Length == 1)
                            {
                                tempmonth = "0" + i.ToString();
                            }
                            else
                            {
                                tempmonth = i.ToString();
                            }
                            FilterDate.Add(string.Concat(toyear + "-" + tempmonth));
                        }
                    }
                }
                if (FilterDate.Count > 12)
                {
                    int k = FilterDate.Count;
                    while (k != 12)
                    {
                        FilterDate.RemoveAt(0);
                        k--;
                    }
                }
                for (int i = 0; i < FilterDate.Count; i++)
                {
                    foreach (var res in total)
                    {
                        if (!yearmonths.Contains(FilterDate[i]))
                        {
                            MonthlyNumberOfPeopleSentAndReceivedSMS model = new MonthlyNumberOfPeopleSentAndReceivedSMS();
                            model.sentcounts = 0;
                            model.receivedcounts = 0;
                            model.months = FilterDate[i];
                            total.Add(model);
                            break;
                        }
                    }
                }
                total = total.OrderBy(c => c.months).ToList();
                if (total.Count > 12)
                {
                    int k = total.Count;
                    while (k != 12)
                    {
                        total.RemoveAt(0);
                        k--;
                    }
                }
                List<string> months = new List<string>();
                List<int> sentcounts = new List<int>();
                List<int> receivedfornopersent = new List<int>();
                List<double> receivedcounts = new List<double>();
                foreach (var res in total)
                {
                    months.Add(res.months);
                    sentcounts.Add(res.sentcounts);
                    receivedfornopersent.Add(res.receivedcounts);
                    /***updated at 25-Nov-2021 by soelae***/
                    double receivePercentCount=0;
                    if (res.receivedcounts == 0)
                    {
                        receivedcounts.Add(receivePercentCount);
                    }
                    else
                    {
                        double receivePercent = ((double)res.receivedcounts / (double)res.sentcounts) * 100;
                        receivePercentCount = Math.Round(receivePercent);
                        receivedcounts.Add(receivePercentCount);
                    }                  
                    /***updated at 25-Nov-2021 by soelae***/

                }

                return Json(new { months, sentcounts, receivedcounts, FilterDate,receivedfornopersent });
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<MonthlyNumberOfPeopleSentAndReceivedSMS>());
            }
        }
        [HttpGet("TotalCorruptionCounts")]
        public JsonResult TotalCorruptionCounts(string fromdate, string todate, int ministry_id, int user_id)

        {
            if (Authorize(AuthorizeAction.VIEW))
            {

                TotalCampaignCountViewModel model = new TotalCampaignCountViewModel();
                TotalCorruptionCounts total = corruRepo.GetCorruptionCounts(fromdate, todate, ministry_id, user_id);
                if (total != null)
                {
                    model.corruptioncounts = total.corruptioncounts;
                }

                return Json(model);
            }
            else
            {
                return Json(GetAccessDeniedJsonResult<TotalCorruptionCounts>());
            }
        }
        [HttpGet("MonthlyCorruptionReceivedCounts")]
        public JsonResult MonthlyCorruptionReceivedCounts(string fromdate, string todate, int ministry_id, int user_id)

        {
            if (Authorize(AuthorizeAction.VIEW))
            {
                List<MonthlyNumberOfCorruptionReceivedSMS> Model = monthcoruRepo.GetCorruptionReceivedSMs(fromdate, todate, ministry_id, user_id);
                List<string> yearmonths = new List<string>();
                List<int> countlist = new List<int>();
                foreach (var res in Model)
                {
                    yearmonths.Add(res.months);
                    countlist.Add(res.corruptioncounts);
                }
                List<string> FilterDate = new List<string>();
                DateTime to_date = Convert.ToDateTime(todate);
                DateTime from_date = Convert.ToDateTime(fromdate);
                int toyear = to_date.Year;
                int month = Convert.ToInt32(to_date.Month);
                int fromyear = from_date.Year;
                int frommonth = Convert.ToInt32(from_date.Month);
                if (fromyear == toyear)
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";

                        if (i.ToString().Length == 1)
                        {
                            tempmonth = "0" + i.ToString();
                        }
                        else
                        {
                            tempmonth = i.ToString();
                        }
                        FilterDate.Add(string.Concat(fromyear + "-" + tempmonth));

                    }
                }
                else
                {

                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        if (i >= frommonth)
                        {
                            if (i.ToString().Length == 1)
                            {
                                tempmonth = "0" + i.ToString();
                            }
                            else
                            {
                                tempmonth = i.ToString();
                            }
                            FilterDate.Add(string.Concat(fromyear + "-" + tempmonth));
                        }
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        var year = toyear - 1;
                        //if (i <= month)
                        //{
                        if (i.ToString().Length == 1)
                        {
                            tempmonth = "0" + i.ToString();
                        }
                        else
                        {
                            tempmonth = i.ToString();
                        }
                        FilterDate.Add(string.Concat(year + "-" + tempmonth));
                        //}
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        string tempmonth = "";
                        if (i <= month)
                        {
                            if (i.ToString().Length == 1)
                            {
                                tempmonth = "0" + i.ToString();
                            }
                            else
                            {
                                tempmonth = i.ToString();
                            }
                            FilterDate.Add(string.Concat(toyear + "-" + tempmonth));
                        }
                    }
                }
                if (FilterDate.Count > 12)
                {
                    int k = FilterDate.Count;
                    while (k != 12)
                    {
                        FilterDate.RemoveAt(0);
                        k--;
                    }
                }
                for (int i = 0; i < FilterDate.Count; i++)
                {
                    foreach (var res in Model)
                    {
                        if (!yearmonths.Contains(FilterDate[i]))
                        {
                            MonthlyNumberOfCorruptionReceivedSMS total = new MonthlyNumberOfCorruptionReceivedSMS();
                            total.corruptioncounts = 0;
                            total.months = FilterDate[i];
                            Model.Add(total);
                            break;
                        }
                    }
                }
                Model = Model.OrderBy(c => c.months).ToList();
                if (Model.Count > 12)
                {
                    int k = Model.Count;
                    while (k != 12)
                    {
                        Model.RemoveAt(0);
                        k--;
                    }
                }
                List<string> months = new List<string>();
                List<int> corruptioncounts = new List<int>();
                foreach (var res in Model)
                {
                    months.Add(res.months);
                    corruptioncounts.Add(res.corruptioncounts);
                }
                return Json(new { months, corruptioncounts, FilterDate });

            }
            else
            {
                return Json(GetAccessDeniedJsonResult<TotalCorruptionCounts>());
            }


        }

    }
}




