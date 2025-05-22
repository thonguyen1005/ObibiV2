using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSW.Core.Caching
{
    public abstract class InProcessCache : ILocalCache, ICache, IDisposable
    {
        /// <summary>
        /// The synchronization lock
        /// </summary>
        private object _sync = new object();

        /// <summary>
        /// The dictionary that contains cached items
        /// </summary>
        private Dictionary<string, object> _dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The dictionary that contains expiration dates for keys that added with an expiration
        /// </summary>
        private Dictionary<string, DateTime> _expiration = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

        public InProcessCache()
        {

        }

        public void Set(string key, object value, TimeSpan? expiration = null)
        {
            // need a better implementation for expirations
            lock (_sync)
            {
                _dictionary[key] = value;

                if (expiration != null)
                    _expiration[key] = DateTime.Now.Add(expiration.Value);
                else
                    _expiration.Remove(key);
            }
        }

        public void Clear()
        {
            lock (_sync)
            {
                _dictionary.Clear();
                _expiration.Clear();
            }
        }

        public void Dispose()
        {
            Clear();
        }

        public TItem Get<TItem>(string key)
        {
            lock (_sync)
            {
                object value;
                if (isExpires(key))
                {
                    return default(TItem);
                }

                _dictionary.TryGetValue(key, out value);

                return (TItem)value;
            }
        }

        public TItem Get<TItem>(string key, Func<TItem> loader, TimeSpan? expiration = null)
        {
            var cachedObj = Get<TItem>(key);

            if (cachedObj == null)
            {
                var item = loader();
                if (expiration == null)
                    Set(key, item);
                else
                    Set(key, item, expiration.Value);

                return item;
            }

            return cachedObj;
        }

        public Dictionary<string, TItem> GetMany<TItem>(params string[] keys)
        {
            var rs = new Dictionary<string, TItem>();
            if (keys.IsEmpty())
            {
                return rs;
            }

            foreach (var k in keys)
            {
                if (rs.ContainsKey(k))
                {
                    continue;
                }

                var item = Get<TItem>(k);
                if (item != null)
                {
                    rs.Add(k, item);
                }
            }

            return rs;
        }

        private bool isExpires(string key)
        {
            if (!_dictionary.ContainsKey(key))
            {
                return true;
            }

            var now = DateTime.Now;
            DateTime expires;
            if (this._expiration.TryGetValue(key, out expires) &&
                expires <= now)
            {
                _dictionary.Remove(key);
                _expiration.Remove(key);
                return true;
            }

            return false;
        }

        public bool HasKey(string key)
        {
            lock (_sync)
            {
                return !isExpires(key);
            }
        }

        public void Remove(string key)
        {
            lock (_sync)
            {
                _dictionary.Remove(key);
                _expiration.Remove(key);
            }
        }

        public void Removes(params string[] keys)
        {
            lock (_sync)
            {
                foreach (var k in keys)
                {
                    _dictionary.Remove(k);
                    _expiration.Remove(k);
                }
            }
        }

        public void RemoveWithPrefix(string prefix)
        {
            var lstKeys = _dictionary.Where(x => x.Key.StartsWith(prefix)).Select(x => x.Key).ToArray();
            if (lstKeys.IsNotEmpty())
            {
                Removes(lstKeys);
            }
        }

        public Dictionary<string, TItem> Search<TItem>(string keyPattern)
        {
            var dicResult = new Dictionary<string, TItem>();
            lock (_sync)
            {
                var lstKeys = _dictionary.Where(x => x.Key.StartsWith(keyPattern.Replace("*", "")))
                    .Select(x => x.Key).ToList();
                if (lstKeys.IsNotEmpty())
                {
                    lstKeys.ForEach(x =>
                    {
                        var v = Get<TItem>(x);
                        if (v != null)
                        {
                            dicResult.Add(x, v);
                        }
                    });
                }
            }
            return dicResult;
        }
    }
}
