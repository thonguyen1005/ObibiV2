using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace VNI.Core.Services.Caching
{
    public static class RedisHelper
    {
        internal static ConnectionMultiplexer GetConnection(string connString)
        {

            try
            {
                ConnectionMultiplexer _redis = null;
                var cfg = ConfigurationOptions.Parse(connString);
                cfg.ConnectTimeout = 10 * 1000; // 10s
                _redis = ConnectionMultiplexer.Connect(cfg);
                return _redis;
            }
            catch (Exception)
            {
              throw;
            }
        }
    }
}
