using NY.Framework.Application.Interfaces;
using NY.Framework.DataAccess;
using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Repositories;
using NY.Framework.Model.Commands.CategoriesCommands;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Services
{
   public class CategoriesService:ServiceBase<Categories,int,ICategoriesRepository>,ICategoriesService
    {
        public CategoriesService(IUnitOfWork uom, IDbContext context, ICategoriesRepository catRepo) : base(typeof(CategoriesService), catRepo, uom, context) { }

        public CommandResult<Categories> CreateOrUpdate(Categories categories)
        {
            CategoriesCreateOrUpdateCommand cmd = new CategoriesCreateOrUpdateCommand(uom, repo, categories);
            return ExecuteCommand(cmd);
        }
        public CommandResult<Categories> Delete(Categories categories)
        {
            CategoryDeleteCommannd cmd = new CategoryDeleteCommannd(uom, repo, categories);
            return ExecuteCommand(cmd);
        }
    }
}
