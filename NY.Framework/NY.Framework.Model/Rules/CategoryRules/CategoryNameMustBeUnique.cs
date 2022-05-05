using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Model.Rules.CategoryRules
{
   public class CategoryNameMustBeUnique: IBusinessRule
    {
        ICategoriesRepository Repo;
        Categories categories;
        public CategoryNameMustBeUnique(ICategoriesRepository _Repo, Categories _categories)
        {
            this.Repo = _Repo;
            this.categories = _categories;
        }
        public string[] GetRules()
        {
            return new string[] { NY.Framework.Infrastructure.Constants.CategoryNameMustBeUnique };
        }

        public bool IsSatisfied()
        {
            Categories cate = Repo.FindByName(categories.name);
            if (cate != null)
            {
                if (cate.ID != categories.ID)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
