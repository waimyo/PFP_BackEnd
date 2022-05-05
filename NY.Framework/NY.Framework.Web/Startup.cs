using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace NY.Framework.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //// This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
           
            /***edited***/
            services.AddCors();
            services.AddMvc()
                .AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            ///*Redis Cache Configuration*/
            //services.AddDistributedMemoryCache();
            ////services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(StackExchange.Redis.ConnectionMultiplexer.Connect(Configuration.GetSection("RedisCacheOptions")["ConnectionString"]));
            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = Configuration.GetSection("RedisCacheOptions")["ConnectionString"];
            //    options.InstanceName = "Mysite:";
            //});

            // Enable cookie authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            //to get HttpContext.Request values 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().AddControllersAsServices();
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

           // configure jwt authentication
           var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //RequestFormSizeLimit
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            var builder = new ContainerBuilder();

            builder.Populate(services);

            string connectionStr = Configuration.GetConnectionString("DefaultConnection");
            Autofac.ModuleRegistrationExtensions.RegisterModule(builder, new NY.Framework.Application.ServiceModule(connectionStr));
            var controllersTypesInAssembly = typeof(Startup).Assembly.GetExportedTypes()
               .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
            builder.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();

            var container = builder.Build();


            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
               //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseCors(policy => policy
              .AllowAnyHeader()
              .AllowAnyMethod()
              .SetIsOriginAllowed((host)=>true)
            //  .WithOrigins("http://localhost:8082")
              .AllowCredentials());
                //app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //var provider = new FileExtensionContentTypeProvider();
            //// Add new mappings
            //provider.Mappings[".vue"] = "text/plain";
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "VueComponents")),
            //    ContentTypeProvider = provider
            //});
            app.UseStaticFiles();

            app.UseAuthentication();
            loggerFactory.AddLog4Net();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "api/{controller=Author}/getall");
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Account}/{action=Login}/{id?}");
            });

        }
    }
}
