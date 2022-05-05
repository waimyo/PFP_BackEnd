using NY.Framework.Infrastructure.Commands;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NY.Framework.Application.Interfaces
{
  public interface IServiceService:IService<Service,int>
    {
        CommandResult<Service> CreateOrUpdate(Service services);
        CommandResult<Service> Delete(Service services);
    }
}
