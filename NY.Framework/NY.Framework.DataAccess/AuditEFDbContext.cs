using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Proxies;

namespace NY.Framework.DataAccess
{
    public partial class AuditEFDbContext : DbContext, IAuditDbContext
    {
        string ConnectionString = "";

        public AuditEFDbContext(string ConnectionString) : base()
        {
            this.ConnectionString = ConnectionString;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(ConnectionString);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ///modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }




        public DatabaseFacade GetDatabase()
        {
            return base.Database;
        }


    }


}
