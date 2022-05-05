using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
   public interface ICategoriesService:IService<Categories,int>
    {
        CommandResult<Categories> CreateOrUpdate(Categories categories);
        CommandResult<Categories> Delete(Categories categories);
    }
}
