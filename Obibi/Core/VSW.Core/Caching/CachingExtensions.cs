using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSW.Core.Caching
{
    public static class CachingExtensions
    {
        public const string TOKEN = ":";
        public const string SECTION_SYSTEM = "system";
        public const string ALL_KEY = "All";
        public const string CODE_KEY = "Code";

        public static readonly CacheSection System = new CacheSection(SECTION_SYSTEM);

        public static string CombineKey(this CacheSection section, params string[] lstElement)
        {
            return section.BuildKey(lstElement).Key;
        }

        public static string CombineKey(params string[] lstElement)
        {
            var lstData = lstElement.Where(x => x.IsNotEmpty()).ToArray();

            return lstData.Length == 1 ? lstData[0] : lstData.Join(TOKEN);
        }

        public static CacheKey BuildKey(this CacheSection section, params string[] keys)
        {
            return new CacheKey(section, keys);
        }

        public static CacheKey BuildCodeKey(this CacheKey key)
        {
            return key.AddKey(CODE_KEY);
        }

        /// <summary>
        /// Combine Key "All"
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string CombineToAll(this CacheSection section, string key)
        {
            return CombineKey(section.Section, key, ALL_KEY);
        }

        public static string CombineCode(this CacheSection section, string key, string code)
        {
            return CombineKey(section.Section, key, CODE_KEY, code);
        }

        public static string CombineCode(this CacheKey parentKey, string code)
        {
            return CombineKey(parentKey.BuildCodeKey().Key, code);
        }


        public static void Set(this IDistributedCache cache, string key, object value, CacheOptions options)
        {
            if (value == null)
            {
                return;
            }

            var json = value.ToJson();

            if (options != null)
                cache.SetString(key, json, ConvertOptions(options));
            else
                cache.SetString(key, json);
        }


        private static DistributedCacheEntryOptions ConvertOptions(CacheOptions options)
        {
            if (options == null)
            {
                return null;
            }
            else
            {
                return new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = options.AbsoluteExpiration,
                    SlidingExpiration = options.SlidingExpiration,
                    AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
                };
            }
        }

        public static void Set(this IDistributedCache cache, string key, object value, TimeSpan? expiration)
        {

            var options = expiration == null ? null : new CacheOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            Set(cache, key, value, options);
        }

        public static object Get(this IDistributedCache cache, string key, Type targetType)
        {
            var json = cache.GetString(key);
            if (json.IsEmpty())
            {
                return null;
            }

            return JsonHelper.Parse(json, targetType);
        }

        public static TValue Get<TValue>(this IDistributedCache cache, string key)
        {
            var json = cache.GetString(key);
            if (json.IsEmpty())
            {
                return default;
            }

            return JsonHelper.Parse<TValue>(json);
        }

        public static TValue Get<TValue>(this IDistributedCache cache, string key, Func<string, TValue> loader = null, CacheOptions loaderOptions = null)
        {
            var json = cache.GetString(key);
            if (json == null)
            {
                if (loader != null)
                {
                    var value = loader(key);
                    if (value != null)
                    {
                        cache.Set(key, value, loaderOptions);
                    }

                    return value;
                }

                return default;
            }

            return JsonHelper.Parse<TValue>(json);
        }


        public static TimeSpan? GetTimeout(this IUseCache setting)
        {
            if (setting.CacheTimeoutInMinutes <= 0)
            {
                return null;
            }

            return TimeSpan.FromMinutes(setting.CacheTimeoutInMinutes);
        }

        public static CacheOptions GetEntryOptions(this IUseCache setting)
        {
            if (setting.CacheTimeoutInMinutes <= 0)
            {
                return null;
            }

            return new CacheOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(setting.CacheTimeoutInMinutes)
            };
        }
    }
}
