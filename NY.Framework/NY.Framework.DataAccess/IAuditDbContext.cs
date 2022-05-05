using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.DataAccess
{
    public interface IAuditDbContext : IDisposable
    {

        DatabaseFacade GetDatabase();



    }
}
