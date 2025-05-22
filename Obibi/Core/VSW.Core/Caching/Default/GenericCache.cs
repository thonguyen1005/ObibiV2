using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public class GenericCache<TKey, TValue> : IGenericCache<TKey, TValue>
    {
        private ConcurrentDictionary<TKey, TValue> _dicKey = new ConcurrentDictionary<TKey, TValue>();
        protected Func<TKey, TValue> Loader { get; private set; }

        public GenericCache(Func<TKey, TValue> loader = null)
        {
            Loader = loader;
        }

        public bool HasKey(TKey key)
        {
            return _dicKey.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _dicKey.TryAdd(key, value);
        }

        public TValue this[TKey key] { get { return Get(key); } }

        public TValue Get(TKey key)
        {
            if (!HasKey(key))
            {
                if (Loader == null)
                {
                    return default(TValue);
                }

                var value = Loader(key);
                _dicKey.TryAdd(key, value);
            }

            return _dicKey[key];
        }

        public Dictionary<TKey, TValue> GetMany<TItem>(params TKey[] keys)
        {
            var dic = new Dictionary<TKey, TValue>();
            foreach (var k in keys)
            {
                if (HasKey(k) && !dic.ContainsKey(k))
                {
                    dic.Add(k, _dicKey[k]);
                }
            }

            return dic;
        }

        public void Remove(TKey key)
        {
            if (HasKey(key))
            {
                _dicKey.Remove(key, out TValue v);
            }
        }

        public void Removes(params TKey[] keys)
        {
            foreach (var k in keys)
            {
                Remove(k);
            }
        }

        public void Clear()
        {
            _dicKey.Clear();
        }
    }
}
