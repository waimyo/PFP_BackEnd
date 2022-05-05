using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Entities;
using System.Collections.Generic;

namespace NY.Framework.Model.Repositories
{
    public interface ICategoriesRepository:IReadWriteRepository<Categories,int>
    {
        Categories FindByName(string name);
        List<Categories> GetCategoriesList();
    }
}
