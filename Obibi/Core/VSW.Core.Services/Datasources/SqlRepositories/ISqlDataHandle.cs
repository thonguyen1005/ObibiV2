using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services
{
    public interface ISqlDataHandle
    {
        void HandleUpdateValue(object obj);

        void HandleReadValue(object obj);
    }
}
