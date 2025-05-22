using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public interface ITypeInfo
    {
        string Name { get; }

        string FullName { get; }

        [JsonIgnore]
        Type Type { get; }

        [JsonIgnore]
        TypePropertyCollection Properties { get; }
    }
}
