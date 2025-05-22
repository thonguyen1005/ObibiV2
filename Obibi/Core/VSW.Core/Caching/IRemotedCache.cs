using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public interface IRemotedCache : IDistributedCache
    {
        IReadOnlyList<string> SearchKeys(string keyPattern);

        IDictionary<string, TData> Search<TData>(string keyPattern);

        bool Removes(string keyPattern);

        bool Removes(string[] keys);

        bool HasKey(string key);
    }
}
