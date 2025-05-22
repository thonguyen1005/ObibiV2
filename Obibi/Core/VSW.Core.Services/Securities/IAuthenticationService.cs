using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Core.Services
{
    public interface IAuthenticationService
    {
        Result<AuthenticationResult> Verify(string aUserName, string aPassword, string appCode = null);
    }

    public class AuthenticationResult
    {
        public Dictionary<string, object> Properties { get; set; }

        public AuthenticationResult()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}
