using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.SmsCommands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
    public class SmsService : ServiceBase<Sms, int, ISmsRepository>, ISmsService
    {

        public SmsService(ISmsRepository smsRepo,IUnitOfWork uom,IDbContext dbContext) 
            :base(typeof(SmsService),smsRepo,uom,dbContext)
        {

        }      

        public CommandResult<Sms> CreateOrUpdateCommand(Sms sms)
        {
            SmsCreateOrUpdateCommand cmd = new SmsCreateOrUpdateCommand(uom,repo,sms);
            return cmd.Execute();
        }
    }
}
