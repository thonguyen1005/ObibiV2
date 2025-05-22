using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core
{
    public interface IAppSession
    {
        string Id { get; }

        string UserName { get; }
    }
}
