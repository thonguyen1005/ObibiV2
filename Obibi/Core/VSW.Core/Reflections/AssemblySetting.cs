using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public class AssemblySetting
    {
        public List<string> Patterns { get; set; }

        public List<string> ExcludePatterns { get; set; }

        public List<string> ResolvePaths { get; set; }
    }
}
