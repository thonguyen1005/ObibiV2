using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public class CacheOptions
    {
        //
        // Summary:
        //     Gets or sets an absolute expiration date for the cache entry.
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        //
        // Summary:
        //     Gets or sets an absolute expiration time, relative to now.
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        //
        // Summary:
        //     Gets or sets how long a cache entry can be inactive (e.g. not accessed) before
        //     it will be removed. This will not extend the entry lifetime beyond the absolute
        //     expiration (if set).
        public TimeSpan? SlidingExpiration { get; set; }

        public static CacheOptions WithAbsoluteExpiration(DateTimeOffset expiredIn, TimeSpan? slidingExpiration = null)
        {
            var options = new CacheOptions();
            options.AbsoluteExpiration = expiredIn;
            options.SlidingExpiration = slidingExpiration;
            return options;
        }

        public static CacheOptions WithAbsoluteExpirationRelativeToNow(TimeSpan expiredIn, TimeSpan? slidingExpiration = null)
        {
            var options = new CacheOptions();
            options.AbsoluteExpirationRelativeToNow = expiredIn;
            options.SlidingExpiration = slidingExpiration;
            return options;
        }
    }
}
