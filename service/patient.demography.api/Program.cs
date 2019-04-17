using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using System;
using System.IO;

namespace patient.demography.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            IConfigurationRoot config = null;

            var app = WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
            {
                IHostingEnvironment env = context.HostingEnvironment;

                builder.AddEnvironmentVariables();
                config = builder.Build();

                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config = builder.Build();
                var connectionstring = config.GetSection("ConnectionStrings:appconnection").Value;

                configurelogging(connectionstring);
                var logger = NLog.LogManager.GetCurrentClassLogger();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Trace);
            })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseNLog()
            .UseStartup<Startup>();

            return app;
        }

        public static void configurelogging(string databasestring)
        {
            var config = new LoggingConfiguration();

            var dbTarget = new DatabaseTarget();
            dbTarget.ConnectionString = databasestring;

            dbTarget.CommandText =
        @"INSERT INTO dbo.[log]
        ([application],
        [logged],
        [level],
        [message],
        [logger],
        [callsite],
        [exception],
        [url],
        [requestipaddress],
        [action],
        [controller],
        [requesthost],
        [requestmethod],
        [querystring],
        [referrer],
        [useragent],
        [authenticated],
        [useridentity],
        [userauthtype])
        VALUES      ( @application,
        @logged,
        @level,
        @message,
        @logger,
        @callsite,
        @exception,
        @url,
        @requestipaddress,
        @action,
        @controller,
        @requesthost,
        @requestmethod,
        @querystring,
        @referrer,
        @useragent,
        @authenticated,
        @useridentity,
        @userauthtype);";

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@application", new NLog.Layouts.SimpleLayout("Patient Demography")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@logged", new NLog.Layouts.SimpleLayout("${date}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new NLog.Layouts.SimpleLayout("${level}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", new NLog.Layouts.SimpleLayout("${message}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new NLog.Layouts.SimpleLayout("${logger}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@callSite", new NLog.Layouts.SimpleLayout("${callsite:className=true:fileName=true:includeSourcePath=true:methodName=true}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new NLog.Layouts.SimpleLayout("${exception:format=@:innerFormat=@:maxInnerExceptionLevel=10:innerExceptionSeparator= --> :separator= |--| :exceptionDataSeparator=string}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@url", new NLog.Layouts.SimpleLayout("${aspnet-request-url}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@requestipaddress", new NLog.Layouts.SimpleLayout("${aspnet-request-IP}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@action", new NLog.Layouts.SimpleLayout("${aspnet-MVC-Action}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@controller", new NLog.Layouts.SimpleLayout("${aspnet-MVC-Controller}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@requesthost", new NLog.Layouts.SimpleLayout("${aspnet-request-Host}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@requestmethod", new NLog.Layouts.SimpleLayout("${aspnet-request-Method}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@querystring", new NLog.Layouts.SimpleLayout("${aspnet-request-QueryString}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@referrer", new NLog.Layouts.SimpleLayout("${aspnet-request-Referrer}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@useragent", new NLog.Layouts.SimpleLayout("${aspnet-request-UserAgent}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@authenticated", new NLog.Layouts.SimpleLayout("${aspnet-User-isAuthenticated}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@useridentity", new NLog.Layouts.SimpleLayout("${aspnet-User-Identity}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@userauthtype", new NLog.Layouts.SimpleLayout("${aspnet-User-AuthType}")));
            // <logger name="*" minlevel="Error" maxlevel="Error" final="true" writeTo="database" />
            config.AddTarget("database", dbTarget);
            var rule = new LoggingRule("*", NLog.LogLevel.Error, dbTarget);

            config.LoggingRules.Add(rule);
            NLog.Common.InternalLogger.LogLevel = NLog.LogLevel.Info;
            NLog.Common.InternalLogger.LogFile = $"c:\\temp\\patient\\nlog.txt";

            NLog.Web.NLogBuilder.ConfigureNLog(config);
        }
    }
}