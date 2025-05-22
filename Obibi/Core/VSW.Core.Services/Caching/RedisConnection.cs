using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace VNI.Core.Services.Caching
{
    public class RedisConnection : IDisposable
    {
        public IDatabase RedisDb { get; private set; }
        public IServer RedisServer { get; private set; }
        public string ConnectionString { get; private set; }

        public string InstanceName { get; private set; }

        internal string GetKey(string key)
        {
            if (string.IsNullOrEmpty(InstanceName))
            {
                return key;
            }

            return InstanceName + key;
        }

        private RedisCacheOptions _options;

        public RedisConnection(IOptions<RedisCacheOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            ConnectionString = _options.Configuration;
            InstanceName = _options.InstanceName;
        }

        internal Result EnsureConnection()
        {
            if (RedisDb != null)
            {
                return Result.Ok();
            }

            var _redis = RedisHelper.GetConnection(ConnectionString);

            if (_redis == null)
            {
                return Result.Error("Cannot connect to redis");
            }

            var addrs = ConnectionString.Split(',');
            RedisServer = _redis.GetServer(addrs[0]);
            RedisDb = _redis.GetDatabase();

            return Result.Ok();
        }

        public void Dispose()
        {
            RedisDb = null;
            RedisServer = null;
        }
    }
}
