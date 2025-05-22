using Elastic.Apm.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSW.Core.Services;
using VSW.Core.Services.Tracing;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using VSW.Core;

namespace VSW.Website
{
    public static class WebAppExtensions
    {
        public static IApplicationBuilder UseTracing(this IApplicationBuilder builder, IConfiguration configuration)
        {
            var settings = configuration.GetConfigWithSection<TracingSettings>();
            if (settings == null || !settings.Enabled)
            {
                return builder;
            }

            return builder.UseElasticApm(configuration);
        }

        public static IApplicationBuilder UseWebAuthorization(this IApplicationBuilder builder)
        {
            return builder;
            //return builder.UseMiddleware<WebAuthorizationMiddleware>();
        }


        public static void CustomCookieOptions(this CookieBuilder builder, AppsSetting setting)
        {
            builder.HttpOnly = true;
            builder.IsEssential = true;
            builder.Name = setting.Code ?? $"{AppDomain.CurrentDomain.FriendlyName}.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";
            //builder.Domain = setting.SharedDomain;
        }

        public static void CustomSessionOptions(this CookieBuilder builder, AppsSetting setting)
        {
            builder.HttpOnly = true;
            builder.IsEssential = true;
            builder.Name = "SESSION" + (setting.Code ?? $"{AppDomain.CurrentDomain.FriendlyName}.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
            //builder.Domain = setting.SharedDomain;
        }

        public static string GetContentPath()
        {
            var env = CoreService.ServiceProvider.GetService<IHostEnvironment>();
            return Path.Combine(env.ContentRootPath, "wwwroot");
        }
    }
}
