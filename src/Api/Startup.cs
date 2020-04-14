using System;
using Api.Utils;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Logic.AppServices;
using Logic.Students;
using Logic.Utils;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Api
{
    public class Startup
    {

        public IConfigurationRoot Configuration { get; private set; }
        public ILifetimeScope AutofacContainer { get; private set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
        }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var config = new Config(3); // Deserialize from appsettings.json
            var commandsConnectionString = new CommandsConnectionString(Configuration["ConnectionString"]);
            var queriesConnectionString = new QueriesConnectionString(Configuration["QueriesConnectionString"]);

            var diBuilder = new ContainerBuilder();
            diBuilder.Populate(services);
            diBuilder.RegisterInstance(config);
            diBuilder.RegisterInstance(commandsConnectionString);
            diBuilder.RegisterInstance(queriesConnectionString);
            diBuilder.RegisterType<SessionFactory>().SingleInstance();
            diBuilder.RegisterType<Messages>().SingleInstance();
            diBuilder.AddMediatR(typeof(DisenrollCommand).Assembly);

            AutofacContainer = diBuilder.Build();
            return new AutofacServiceProvider(AutofacContainer);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
