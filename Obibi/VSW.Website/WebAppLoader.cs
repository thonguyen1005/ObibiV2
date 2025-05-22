using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSW.Core.Services;
using Microsoft.AspNetCore.Hosting;
using VSW.Core;

namespace VSW.Website
{
    public class WebAppLoader : AppLoader
    {
        public static readonly string DefaultWebConfigRootPath = "App_Data/" + DefaultConfigRootPath;

        public WebAppLoader(string configFiles) : base(configFiles)
        {
            var logConfig = new LoggerConfiguration()
             .Enrich.FromLogContext()
             .ReadFrom.Configuration(CoreService.Configuration);

            var log = logConfig.CreateLogger().ForContext<WebAppLoader>();

            Log.Logger = log;

            using (var logfactory = new SerilogLoggerFactory(log))
            {
                var _logger = logfactory.CreateLogger(typeof(WebAppLoader).FullName);
                GlobalLogger.Current = new GlobalLogger(_logger);
            }
        }

        public override void Run(string[] args)
        {
            Run<WebStartUp>(args);
        }

        public void Run<TStartup>(string[] args) where TStartup : WebStartUp
        {
            GlobalLogger.Current.LogDebug($"{APP_LOG} Starting Application...");

            var HostInstance = Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseConfiguration(CoreService.Configuration);
                webBuilder.UseStartup<TStartup>();
            })
            .UseSerilog().Build();

            try
            {
                HostInstance.Run();
            }
            finally
            {
                if (GlobalLogger.Current is IDisposable)
                {
                    (GlobalLogger.Current as IDisposable).Dispose();
                }
            }
        }
    }
}
