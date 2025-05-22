using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VSW.Core.Modules;
using AutoMapper;
using VSW.Core.Caching;

namespace VSW.Core
{
    public class CoreModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureWithSection<AssemblySetting>(Configuration);
            services.AddSingleton<MapperProfile>();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddSingleton<ILocalCache, DefaultLocalCache>();
        }

        public override void Configure(IServiceProvider resolver)
        {

        }
    }
}
