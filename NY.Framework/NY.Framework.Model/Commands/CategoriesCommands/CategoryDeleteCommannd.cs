using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using NY.Framework.Infrastructure;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Commands.CategoriesCommands
{
    public class CategoryDeleteCommannd : Command<Categories, int, Categories, ICategoriesRepository>
    {
        public CategoryDeleteCommannd(IUnitOfWork uom, ICategoriesRepository catRepo, Categories categories)
            : base(uom, catRepo, categories) { }

        protected override List<IBusinessRule> GetRules()
        {
            return null;
        }

        protected override CommandResult<Categories> PerformAction(CommandResult<Categories> result)
        {
            repo.Remove(entity);
            result.Success = true;
            result.Messages.Add(Constants.DELETE_SUCCESS_MESSAGE);
            return result;
        }
    }
}
