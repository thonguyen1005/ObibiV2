using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VNI.Core.Caching;

namespace VNI.Core.Services.Caching
{
    public class RemotedRedisCache : RedisCache, IRemotedCache
    {
        private RedisConnection _client;

        public RemotedRedisCache(IOptions<RedisCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
            _client = new RedisConnection(optionsAccessor);
            _client.EnsureConnection();
        }

        public bool HasKey(string key)
        {
            _client.EnsureConnection();
            key = _client.GetKey(key);
            return _client.RedisDb.KeyExists(key);
        }

        private List<RedisKey> SearchRedisKeys(string keyPattern)
        {
            _client.EnsureConnection();
            var key = _client.GetKey(keyPattern);
            return _client.RedisServer.Keys(_client.RedisDb.Database, key, Int32.MaxValue, CommandFlags.None).ToList();
        }

        public IDictionary<string, TData> Search<TData>(string keyPattern)
        {
            var keys = SearchRedisKeys(keyPattern);
            var result = new Dictionary<string, TData>();
            if (keys.IsEmpty())
            {
                return result;
            }

            for (int i = 0; i < keys.Count; i++)
            {
                var k = keys[i];
                var v = this.Get<TData>(k);
                if (v != null)
                {
                    result.Add(k, v);
                }
            }

            return result;
        }

        public IReadOnlyList<string> SearchKeys(string keyPattern)
        {
            var result = new List<string>();
            var keys = SearchRedisKeys(keyPattern);
            if (keys.IsEmpty())
            {
                return result;
            }

            foreach (var k in keys)
            {
                result.Add(k);
            }
            return result;
        }

        public bool Removes(string keyPattern)
        {
            var keys = SearchRedisKeys(keyPattern);
            if (keys.IsEmpty())
            {
                return true;
            }

            _client.RedisDb.KeyDelete(keys.ToArray());
            return true;
        }

        public bool Removes(string[] keys)
        {
            _client.EnsureConnection();
            var lstKey = new List<RedisKey>();
            foreach (var k in keys)
            {
                lstKey.Add(_client.GetKey(k));
            }

            _client.RedisDb.KeyDelete(lstKey.ToArray());
            return true;
        }
    }
}
