using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Application
{
    public class ServiceModule : Autofac.Module
    {
        string connectionString = "DefaultConnection";
        public ServiceModule(string connectionString)
        {
            this.connectionString = connectionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new NY.Framework.DataAccess.EFModule(connectionString));
            builder.RegisterModule(new NY.Framework.DataAccess.RepositoryModule());
            //builder.RegisterType<NY.Framework.Application.Services.UserService>().As<NY.Framework.Application.Services.Interfaces.IUserService>().InstancePerLifetimeScope();
            //builder.RegisterType<NY.Framework.Application.Services.CacheService>().As<NY.Framework.Model.Repositories.ICacheRepository>().InstancePerLifetimeScope();
            
            builder.RegisterAssemblyTypes(Assembly.Load("NY.Framework.Application"))
                    .Where(t => t.Name.EndsWith("Service"))
                    .AsImplementedInterfaces().PropertiesAutowired()
                    .InstancePerLifetimeScope();

            
            
        }
    }
}
