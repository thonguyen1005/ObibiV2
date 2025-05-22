using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public class TypeInfo : ITypeInfo
    {
        public string Name { get; private set; }

        public string FullName { get; private set; }

        [JsonIgnore]
        public Type Type { get; private set; }

        [JsonIgnore]
        public TypePropertyCollection Properties { get { return TypeManager.GetPropertiesFromCached(Type); } }

        public TypeInfo(Type t)
        {
            Name = t.Name;
            FullName = t.FullName;
            Type = t;
        }

        public TypeInfo()
        {

        }
    }
}
