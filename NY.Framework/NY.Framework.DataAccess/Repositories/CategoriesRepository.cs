using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.DataAccess.Repositories
{
   public class CategoriesRepository:ReadWriteRepositoryBase<Categories,int>,ICategoriesRepository
    {
        public CategoriesRepository(IDbContext context) : base(context, new string[] { }) { }

        public Categories FindByName(string name)
        {
            return CustomQuery().Where(c => c.name.Equals(name)).FirstOrDefault();
        }       

        List<Categories> ICategoriesRepository.GetCategoriesList()
        {
            return CustomQuery().Where(s => s.IsDeleted.Equals(false)).ToList();
        }
    }  

   
}
