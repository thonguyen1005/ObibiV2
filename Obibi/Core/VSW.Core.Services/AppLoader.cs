using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using VSW.Core.IO;
using VSW.Core.Modules;

namespace VSW.Core.Services
{
    public class AppLoader
    {
        public static readonly string DefaultConfigRootPath = "Configs" + FileHelper.PathSeparator + "*.json";

        public const string APP_LOG = "[AppLoader]";

        public AppLoader(string configFiles)
        {
            CoreService.Configuration = InitConfiguration(configFiles);
        }

        public virtual void Run(string[] args)
        {

        }

        public void Shutdown()
        {
            ShutdownModules();
        }


        private void ShutdownModules()
        {
            var lstService = ModuleContainer.GetNeedShutdowns();
            if (lstService.IsNotEmpty())
            {
                foreach (var service in lstService)
                {
                    service.Shutdown();
                }
            }
        }

        protected virtual IConfiguration InitConfiguration(string configFiles)
        {
            var configBuilder = new ConfigurationBuilder()
                   .SetBasePath(FileHelper.GetCurrentDirectory());

            var files = FileHelper.GetFilesByPattern(configFiles, FileHelper.GetCurrentDirectory());
            foreach (var r in files)
            {
                configBuilder.AddJsonFile(r, optional: false, reloadOnChange: true);
            }

            var config = configBuilder.AddEnvironmentVariables().Build();

            return config;
        }
    }
}
