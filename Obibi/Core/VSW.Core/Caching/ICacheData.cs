using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public interface ICacheData: ICacheData<object>
    {

    }

    public interface ICacheData<TValue> 
    {
        string Key { get; set; }

        TValue Value { get; set; }

        TimeSpan? ExpireTime { get; }

        bool HasExpiredTime { get; }
    }
}
