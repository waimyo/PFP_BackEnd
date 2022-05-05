using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Entities;
using NY.Framework.Model.Entities;
using NY.Framework.Model.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
//using NY.Framework.Infrastructure.Repositories;

namespace NY.Framework.DataAccess.Repositories
{
    public class AuditRepository : ReadWriteRepositoryBase<Model.Entities.Audit, int>, Model.Repositories.IAuditRepository
    {
        public AuditRepository(IDbContext context)
            : base(context, new string[] { "" })
        {


        }

    }

  
}
