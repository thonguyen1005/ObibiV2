using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public interface INeedShutdown
    {
        int Priority { get; }

        void Shutdown();
    }
}
