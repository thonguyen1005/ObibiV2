using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using VSW.Core;
using VSW.Core.Services;

namespace VSW.Website
{
    public class SignInManager
    {
        private IHttpContextAccessor _httpContextAccessor;
        private AppsSetting _appsSetting;
        public SignInManager(IHttpContextAccessor contextAccessor, IOptions<AppsSetting> options)
        {
            _httpContextAccessor = contextAccessor;
            _appsSetting = options.Value;
        }

        private void SetAuthToken(string aUserName, bool aRememberMe, List<string> stringArray)
        {
            var context = _httpContextAccessor.HttpContext;
            var principal = new GenericPrincipal(new GenericIdentity(aUserName, CookieAuthenticationDefaults.AuthenticationScheme), stringArray.ToArray());
            context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = aRememberMe }).Wait();
        }

        public Result Logout()
        {
            var context = _httpContextAccessor.HttpContext;
            context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return Result.Ok();
        }
    }
}
