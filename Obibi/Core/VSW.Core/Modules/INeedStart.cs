using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public interface INeedStart
    {
        int Priority { get; }

        void Start();
    }
}
