using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using VSW.Core.Modules;

namespace VSW.Core.Services
{
    public class StartUpBase
    {
        protected IConfiguration Configuration { get; set; }
        public StartUpBase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            GlobalLogger.Current.LogDebug($"{AppLoader.APP_LOG} Configuring Services Application...");

            CoreService.ServiceCollection = services;
            ConfigureCoreServices();
            OnConfigureServices(services);
        }

        protected virtual void ConfigureCoreServices()
        {
            JsonSettings.Init();
            var setting = CoreService.GetConfigWithSection<AssemblySetting>();
            TypeManager.Init(() => setting);
            CoreService.ServiceCollection.AddSingleton(CoreService.ServiceCollection);

            //Register Module
            var lstService = ModuleContainer.GetModules();
            if (lstService.IsNotEmpty())
            {
                foreach (var service in lstService)
                {
                    service.ConfigureServices(CoreService.ServiceCollection);
                }
            }
        }

        public virtual void OnConfigure(bool beginConfigure = true)
        {
            //Config Module
            var lstService = ModuleContainer.GetModules();
            if (lstService.IsNotEmpty())
            {
                foreach (var service in lstService)
                {
                    service.Configure(CoreService.ServiceProvider);
                }
            }

            StartModules();
        }

        private void StartModules()
        {
            GlobalLogger.Current.LogDebug($"{AppLoader.APP_LOG} Starting Modules Application...");

            var lstService = ModuleContainer.GetModules();
            if (lstService.IsNotEmpty())
            {
                foreach (var service in lstService)
                {
                    service.Initialize(CoreService.ServiceProvider);
                }
            }

            var lstServiceStart = ModuleContainer.GetNeedStarts();
            if (lstServiceStart.IsNotEmpty())
            {
                foreach (var service in lstServiceStart)
                {
                    service.Start();
                }
            }

            //Gọi trước để khởi tạo lst shutdown module
            ModuleContainer.GetNeedShutdowns();

            GlobalLogger.Current.LogDebug($"{AppLoader.APP_LOG} Started Application...");
        }

        protected virtual void OnConfigureServices(IServiceCollection services)
        {

        }
    }

    public abstract class StartUpBase<TAppBuilder, THostedEnvironment> : StartUpBase where THostedEnvironment : IHostEnvironment
    {
        protected THostedEnvironment Environments { get; set; }

        public StartUpBase(IConfiguration configuration, THostedEnvironment environment) : base(configuration)
        {
            Environments = environment;
        }

        public void Configure(TAppBuilder app)
        {
            GlobalLogger.Current.LogDebug($"{AppLoader.APP_LOG} Configuring Application...");

            OnConfigure(app);
            OnConfigure(false);
        }

        protected virtual void OnConfigure(TAppBuilder app)
        {
        }
    }
}
