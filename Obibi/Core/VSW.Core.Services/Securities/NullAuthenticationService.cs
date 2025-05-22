using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services
{
    public class NullAuthenticationService : IAuthenticationService
    {
        public Result<AuthenticationResult> Verify(string aUserName, string aPassword, string appCode = null)
        {
            return Result.Ok(new AuthenticationResult());
        }
    }
}
