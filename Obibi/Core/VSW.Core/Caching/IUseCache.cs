using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public interface IUseCache
    {
        bool UseCache { get; set; }

        string CachePrefix { get; set; }

        int CacheTimeoutInMinutes { get; set; }
    }
}
