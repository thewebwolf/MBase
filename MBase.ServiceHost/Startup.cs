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

namespace MBase.ServiceHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private string ApiTitle;
        public void ConfigureServices(IServiceCollection services)
        {
            var service = new Service();
            services.AddSingleton<IService>(service);

            services.AddControllers().AddApplicationPart(Assembly.Load(ControllerBuilder.CreateControllerCode(service)));

            ApiTitle = $"My {service.GetType().FullName} Service.";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ApiTitle, Version = "v1" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiTitle);
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
