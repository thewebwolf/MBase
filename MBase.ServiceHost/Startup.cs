using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MBase.MBase.ServiceHost.Controllers;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.IO;
using MBase.MemberService;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MBase.ServiceHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private IConfiguration Configuration { get; }
        private ILoggerFactory LoggerFactory { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LoggerFactory = new LoggerFactory();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.AddDebug();
            });
            services.AddHealthChecks();

            var service = new Service();
            services.AddSingleton<IService>(service);

            foreach (var command in service.Commands)
            {
                services.AddScoped(command.GetType());
            }

            services.AddControllers().AddApplicationPart(Assembly.Load(ControllerBuilder.CreateControllerCode(service)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"My {service.GetType().FullName} Service.", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Microservices");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/health", new HealthCheckOptions { Predicate = check => check.Tags.Contains("ready") });
        }


    }
}
