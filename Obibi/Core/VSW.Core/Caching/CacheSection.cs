using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Caching
{
    public class CacheSection
    {
        public string Section { get; private set; }

        public CacheSection(string section)
        {
            Section = section;
        }
    }
}
