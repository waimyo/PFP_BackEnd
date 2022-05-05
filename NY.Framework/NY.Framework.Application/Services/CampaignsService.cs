using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.CampaignsCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class CampaignsService : ServiceBase<Campaigns, int, ICampaignsRepository>, ICampaignService
    {

        public CampaignsService(ICampaignsRepository campaignRepo,IUnitOfWork uom,IDbContext dbContext) 
            : base(typeof(CampaignsService),campaignRepo,uom,dbContext)
        {

        }
        public CommandResult<Campaigns> CreateOrUpdateCommand(Campaigns campaign)
        {
            CampaignsCreateOrUpdateCommand cmd = new CampaignsCreateOrUpdateCommand(uom,repo,campaign);            
            return ExecuteCommand(cmd) ;
        }
    }
}
