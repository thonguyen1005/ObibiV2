using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using VSW.Core;

namespace VSW.Website
{
    public interface IWebSession: IAppSession, ISession
    {

    }
}
