using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace patient.demography.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionStrings>(appSettingsSection);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddDebug();
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });
            services.AddMvc(config =>
            {
                /* config.RespectBrowserAcceptHeader = true;
                 config.ReturnHttpNotAcceptable = true;
                 config.OutputFormatters.Add(new XMLOutputFormatter());
                 config.InputFormatters.Add(new XMLInputFormatter());*/
            }).AddXmlDataContractSerializerFormatters().AddXmlSerializerFormatters()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.Use(async (httpContext, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception e)
                {
                }
            });
            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}