

using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using NY.Framework.Model.Rules.CategoryRules;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.CategoriesCommands
{
   public class CategoriesCreateOrUpdateCommand:Command<Categories,int,Categories,ICategoriesRepository>
    {
        public CategoriesCreateOrUpdateCommand(IUnitOfWork uom, ICategoriesRepository catrepo, Categories categories) : base(uom, catrepo, categories)
        {

        }

        protected override List<IBusinessRule> GetRules()
        {
            List<IBusinessRule> rules = new List<IBusinessRule>();
            rules.Add(new CategoryNameMustBeUnique(repo,entity));
            return rules;
        }

        protected override CommandResult<Categories> PerformAction(CommandResult<Categories> result)
        {
            repo.Save(entity);
            result.Success = true;
            result.Messages.Add(Constants.SAVE_SUCCESS_MESSAGE);
            //result.Result.Add(entity);
            return result;
        }
    }
}
