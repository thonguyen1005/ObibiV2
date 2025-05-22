using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public interface ITwoLevelCache : ICache
    {
        /// <summary>
        /// Cache Provider for Remote Level
        /// </summary>
        IDistributedCache RemoteCache { get; }

        /// <summary>
        /// Cache Provider for Local Level
        /// </summary>
        ILocalCache LocalCache { get; }

    }
}
