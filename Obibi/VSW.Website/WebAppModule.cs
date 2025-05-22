using VSW.Core;
using VSW.Core.Modules;
using VSW.Website.DataBase.Repositories;
using VSW.Website.DataBase.Services;
using VSW.Website.Interface;

namespace VSW.Website
{
    public class WebAppModule : BaseModule
    {
        public override int Priority => PriorityLevel.Application;

        public override void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<SignInManager>();

            services.AddScoped<IWebSession, WebSession>();
            services.AddScoped<IAppSession>(provider => provider.GetService<IWebSession>());
            //services.AddSingleton<IWebMenuRegister, WebMenuRegister>();
            services.AddScoped<IResourceServiceInterface, ResourceService>();
            services.AddScoped<ISiteServiceInterface, SiteService>();
            services.AddScoped<IPageServiceInterface, PageService>();
            services.AddScoped<ITemplateServiceInterface, TemplateService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
        }

        public override void Configure(IServiceProvider resolver)
        {
            //WebMenuManager.Init();
        }
    }
}
