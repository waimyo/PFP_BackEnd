using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Infrastructure;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System.Collections.Generic;
using NY.Framework.Model.Rules.CampaignsRules;

namespace NY.Framework.Model.Commands.CampaignsCommands
{
    public class CampaignsCreateOrUpdateCommand : Command<Campaigns, int, Campaigns, ICampaignsRepository>
    {

        public CampaignsCreateOrUpdateCommand(IUnitOfWork uom,
            ICampaignsRepository campaignRepo,
            Campaigns campaign) 
            : base(uom,campaignRepo,campaign)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new CampaignNameMustBeUnique(repo,entity));
            return rules;
        }

        protected override CommandResult<Campaigns> PerformAction(CommandResult<Campaigns> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            result.Result.Add(entity);
            return result;
        }
    }
}
