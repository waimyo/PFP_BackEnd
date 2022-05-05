using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules.CampaignsRules
{
    public class CampaignNameMustBeUnique : IBusinessRule
    {
        ICampaignsRepository campaignsRepository;
        Campaigns campaigns;

        public CampaignNameMustBeUnique(ICampaignsRepository _campaignsRepository
            ,Campaigns _campaigns)
        {
            campaignsRepository = _campaignsRepository;
            campaigns = _campaigns;
        }


        public string[] GetRules()
        {
            string message =Constants.CampaignNameMustBeUnique;
            return new string[] { message };
        }

        public bool IsSatisfied()
        {
            Campaigns existingcampaign=campaignsRepository.FindByName(campaigns.Name);
            if (existingcampaign != null)
            {
                if (existingcampaign.ID != campaigns.ID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
