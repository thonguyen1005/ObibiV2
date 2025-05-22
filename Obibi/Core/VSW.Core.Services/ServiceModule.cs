using Elastic.Apm.Api;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSW.Core.Caching;
using VSW.Core.Modules;
using VSW.Core.Services.Tracing;
using VSW.Core.Services.Tracing.Default;
using VSW.Core.Services.Tracing.ElasticApm;

namespace VSW.Core.Services
{
    public class ServiceModule : BaseModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureWithSection<AppsSetting>(Configuration);
            services.ConfigureWithSection<DatasourcesSetting>(Configuration);
            //services.ConfigureWithSection<StoragesSetting>(Configuration);

            //Register Redis
            //var dbItem = DatasourceExtensions.GetRedisSetting();

            //var param = dbItem.Item1.Parameters["InstanceName"];
            //var instanceName = param.Value;

            //services.Configure<RedisCacheOptions>(options =>
            //{
            //    options.Configuration = dbItem.Item2.FirstOrDefault().Value;
            //    options.InstanceName = instanceName + CachingExtensions.TOKEN;
            //});

            //services.AddSingleton<IRemotedCache, RemotedRedisCache>();
            //services.AddSingleton<IDistributedCache, RemotedRedisCache>();

            services.AddSingleton<ISqlDataHandle, SqlConvertHandleData>();
            services.AddSingleton<SqlConvertHandleData>();

            services.AddTransient(typeof(IWorkingContext<>), typeof(WorkingContext<>));

            services.AddSingleton<IAuthenticationService, NullAuthenticationService>();
            services.AddSingleton<IPermissionService, NullPermissionService>();
            //services.AddTransient<IExcelDocument, EPPlusExcelDocument>();
            //End Register Redis


            //Register Tracing ElasticApm
            var settings = CoreService.GetConfigWithSection<TracingSettings>();
            if (settings != null && settings.Enabled)
            {
                services.AddSingleton<Tracing.ITracer, ElasticApmTracer>();
            }
            else
            {
                services.AddSingleton<Tracing.ITracer, DefaultTracer>();
            }
            //End Register Tracing ElasticApm

        }

        public override void Initialize(IServiceProvider resolver)
        {
            //ExcelHelper.Init();
        }
    }
}
