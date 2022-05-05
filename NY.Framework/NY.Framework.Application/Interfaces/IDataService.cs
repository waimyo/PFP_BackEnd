using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface IDataService:IService<Data,int>
    {
        CommandResult<Data> CreateOrUpdate(Data entity);
        CommandResult<Data> Delete(Data entity);
    }
}
