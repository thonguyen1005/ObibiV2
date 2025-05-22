using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public class CacheKey
    {
        public string Key { get; private set; }

        public CacheKey(string section, params string[] keys)
        {
            var lst = new List<string> { section };
            if (keys.IsNotEmpty())
            {
                lst.AddRange(keys);

            }
            Key = CachingExtensions.CombineKey(lst.ToArray());
        }

        public CacheKey(CacheSection section, params string[] keys) : this(section.Section, keys)
        {

        }

        public CacheKey AddKey(string key)
        {
            Key += CachingExtensions.TOKEN + key;
            return this;
        }
    }
}
