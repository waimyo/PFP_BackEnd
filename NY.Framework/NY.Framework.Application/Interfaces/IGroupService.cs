using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
    public interface IGroupService:IService<Groups,int>
    {
        CommandResult<Groups> CreateOrUpdate(Groups group);
        CommandResult<Groups> Delete(Groups group);
    }
}
