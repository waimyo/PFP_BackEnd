using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.SmsCommands
{
    public class SmsCreateOrUpdateCommand : Command<Sms, int, Sms, ISmsRepository>
    {

        public SmsCreateOrUpdateCommand(IUnitOfWork uom,ISmsRepository smsRepo,Sms sms) 
            : base(uom,smsRepo,sms)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            return rules;
        }

        protected override CommandResult<Sms> PerformAction(CommandResult<Sms> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Result.Add(entity);
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
