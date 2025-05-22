using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace VSW.Core
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigureWithSection<TSetting>(this IServiceCollection services, IConfiguration config, string sectionKey = "") where TSetting : class
        {
            IConfiguration section;
            if (sectionKey.IsEmpty())
            {
                section = config.GetSection<TSetting>();
            }
            else
            {
                section = config.GetSection(sectionKey);
            }

            return services.Configure<TSetting>(section);
        }

        public static IConfiguration GetSection<TSetting>(this IConfiguration config)
        {
            return config.GetSection(typeof(TSetting));
        }

        public static string GetSectionKey(Type settingType)
        {
            string sectionKey = settingType.Name;
            if (sectionKey.EndsWith("Settings"))
            {
                sectionKey = sectionKey.Left(sectionKey.Length - "Settings".Length);
            }
            else if (sectionKey.EndsWith("Setting"))
            {
                sectionKey = sectionKey.Left(sectionKey.Length - "Setting".Length);
            }

            return sectionKey;
        }

        public static string GetSectionKey<TSetting>()
        {
            return GetSectionKey(typeof(TSetting));
        }

        public static IConfiguration GetSection(this IConfiguration config, Type settingType)
        {
            string sectionKey = GetSectionKey(settingType);
            return config.GetSection(sectionKey);
        }

        public static TSetting GetConfig<TSetting>(this IConfiguration config)
        {
            return config.Get<TSetting>();
        }

        public static TSetting GetConfigWithSection<TSetting>(this IConfiguration config, string sectionKey = "")
        {
            IConfiguration section;
            if (sectionKey.IsEmpty())
            {
                section = config.GetSection<TSetting>();
            }
            else
            {
                section = config.GetSection(sectionKey);
            }

            return section.GetConfig<TSetting>();
        }

        public static TSetting GetConfig<TSetting>(this IConfiguration config, string key)
        {
            return config.GetValue<TSetting>(key);
        }
    }
}
