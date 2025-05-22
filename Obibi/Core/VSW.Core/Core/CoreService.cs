using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using VSW.Core.Caching;

namespace VSW.Core
{
    public static class CoreService
    {
        public static bool UseDependency { get => ServiceProvider != null; }

        public static void CheckDependencyMode()
        {
            if (!UseDependency)
            {
                throw new Exception("Cann't create object because Application isn't use Dependency Inject");
            }
        }

        public static IConfiguration Configuration { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }

        public static IServiceCollection ServiceCollection { get; set; }


        private static IRemotedCache _distributedCache;
        public static IRemotedCache DistributedCache { get => GetService(_distributedCache); }

        private static ILocalCache _localcache;
        public static ILocalCache LocalCache { get => GetService(_localcache); }


        private static IMapper _mapper;
        public static IMapper Mapper { get => GetService(_mapper); }


        public static void InitMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public static void InitCache(IRemotedCache cacheProvider)
        {
            _distributedCache = cacheProvider;
        }

        public static TSetting GetConfigWithSection<TSetting>(string sectionKey = "")
        {
            var type = typeof(TSetting);
            return Configuration.GetConfigWithSection<TSetting>(sectionKey);
        }

        private static T GetService<T>(T service) where T : class
        {
            if (service != null)
            {
                return service;
            }

            if (UseDependency)
            {
                return ServiceProvider.GetService<T>();
            }

            return null;
        }
    }
}
