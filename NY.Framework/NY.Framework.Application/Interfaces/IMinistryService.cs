using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface IMinistryService: IService<Ministry,int>
    {
        CommandResult<Ministry> CreateOrUpdate(Ministry entity);
        CommandResult<Ministry> Delete(Ministry entity);
    }
}
