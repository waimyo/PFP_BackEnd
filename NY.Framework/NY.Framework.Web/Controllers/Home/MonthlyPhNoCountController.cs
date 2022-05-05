using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NY.Framework.Application;
using NY.Framework.Infrastructure.Enumerations;
using NY.Framework.Model.Entities.Home;
using NY.Framework.Model.Repositories.Home;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NY.Framework.Web.Controllers.Home
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyPhNoCountController : BaseController
    {
        IMonthlyPhNoCountRepository phnocountRepo;

        public MonthlyPhNoCountController(IMonthlyPhNoCountRepository _phnocountRepo)
         : base(typeof(MonthlyPhNoCountController), Application.ProgramCodeEnum.Dashboard)
        {
            this.phnocountRepo = _phnocountRepo;
        }


        [HttpGet]
        [Route("GetMonthlyPhNoCount")]
        public JsonResult GetMonthlyPhNoCount(string fromdate,string todate,int ministry_id,int user_id)
        {
           
            if (Authorize(AuthorizeAction.VIEW))
            {
                var phnolsit = new List<MonthlyPhnoCount>();
                if (ministry_id == 0)
                {
                    List<MonthlyPhnoCount> phnolist = phnocountRepo.getMonthlyPhnoCount(fromdate, todate, ministry_id, user_id);
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
                    foreach (var res in phnolist)
                    {
                        yearmonthlist.Add(res.yearmonths);

                    }

                    //check for all yearmonth,if yearmonth is not contain in databse list,add (count=0,yearmonth="2021-05") to database list
                    for (int i = 0; i < filterdates.Count; i++)
                    {

                        foreach (var res in phnolist)
                        {

                            if (!yearmonthlist.Contains(filterdates[i]))
                            {
                                MonthlyPhnoCount phnoobj = new MonthlyPhnoCount();
                                phnoobj.counts = 0;
                                phnoobj.yearmonths = filterdates[i];
                                phnolist.Add(phnoobj);
                                break;
                            }
                        }
                    }
                    phnolist = phnolist.OrderBy(c => c.yearmonths).ToList();
                    if (phnolist.Count > 12)
                    {
                        int k = phnolist.Count;
                        while (k != 12)
                        {
                            phnolist.RemoveAt(0);
                            k--;
                        }
                    }
                    List<string> yearmonths = new List<string>();
                    List<int> counts = new List<int>();
                    foreach (var res in phnolist)
                    {
                        yearmonths.Add(res.yearmonths);
                        counts.Add(res.counts);
                    }
                    return Json(new { yearmonths, counts, filterdates });
                }
                else
                {
                 List < MonthlyPhnoCount > phnolist = phnocountRepo.getMonthlyPhnoCount(fromdate, todate, userRepo.Get(ministry_id).ministry_id, user_id);
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
                    foreach (var res in phnolist)
                    {
                        yearmonthlist.Add(res.yearmonths);

                    }

                    //check for all yearmonth,if yearmonth is not contain in databse list,add (count=0,yearmonth="2021-05") to database list
                    for (int i = 0; i < filterdates.Count; i++)
                    {

                        foreach (var res in phnolist)
                        {

                            if (!yearmonthlist.Contains(filterdates[i]))
                            {
                                MonthlyPhnoCount phnoobj = new MonthlyPhnoCount();
                                phnoobj.counts = 0;
                                phnoobj.yearmonths = filterdates[i];
                                phnolist.Add(phnoobj);
                                break;
                            }
                        }
                    }
                    phnolist = phnolist.OrderBy(c => c.yearmonths).ToList();
                    if (phnolist.Count > 12)
                    {
                        int k = phnolist.Count;
                        while (k != 12)
                        {
                            phnolist.RemoveAt(0);
                            k--;
                        }
                    }
                    List<string> yearmonths = new List<string>();
                    List<int> counts = new List<int>();
                    foreach (var res in phnolist)
                    {
                        yearmonths.Add(res.yearmonths);
                        counts.Add(res.counts);
                    }
                    return Json(new { yearmonths, counts, filterdates });
                }

               
            
                   
                }
      
            else
            {
                return Json(GetAccessDeniedJsonResult<MonthlyPhnoCount>());
            }
        }

    }
}
