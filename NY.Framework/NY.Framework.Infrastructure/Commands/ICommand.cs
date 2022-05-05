using NY.Framework.Infrastructure.Entities;
using NY.Framework.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Commands
{
    public interface ICommand<TEntity> where TEntity : BaseEntity 
    {
        CommandResult<TEntity> Execute();

        
    }

}
