using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Pagination;
using NY.Framework.Model.Entities.Home;
using NY.Framework.Web.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NY.Framework.Web.Mappers.Home
{
    public class MonthlyCampaignCountMapper
    {
        public JqueryDataTableQueryOptions<MonthlyCampaignCount> Preparetorepository(JqueryDataTableQueryOptions<MonthlyCampaignCount> queryoption)
        {
            if (!string.IsNullOrEmpty(queryoption.SearchValue))
            {
               // queryoption.FilterBy = (c => c.c.Contains(queryoption.SearchValue));
            }
            return queryoption;
        }
       
    }
}