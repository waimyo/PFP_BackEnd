using Autofac;
using NY.Framework.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.DataAccess
{
    public class EFModule : Autofac.Module
    {
        string connectionString = "DefaultConnection";
        public EFModule(string connectionString)
            : base()
        {
            this.connectionString = connectionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            Autofac.NamedParameter para = new NamedParameter("ConnectionString", connectionString);
            builder.RegisterType(typeof(EFDbContext)).As(typeof(IDbContext)).WithParameter(para).InstancePerLifetimeScope();
            builder.RegisterType(typeof(AuditEFDbContext)).As(typeof(IAuditDbContext)).WithParameter(para).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();

            
        }

    }
}
