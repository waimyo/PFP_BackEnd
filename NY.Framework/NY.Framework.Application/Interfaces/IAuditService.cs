using NY.Framework.Infrastructure.Commands;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Model;
using NY.Framework.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Application.Interfaces
{
    public interface IAuditService : IService<Model.Entities.Audit, int>
    {
        CommandResult<Model.Entities.Audit> Create(Model.Entities.Audit audit);
    }

    //public interface IAuditService
    //{
    //    void Log(Infrastructure.Entities.Audit audit);
    //}
}
