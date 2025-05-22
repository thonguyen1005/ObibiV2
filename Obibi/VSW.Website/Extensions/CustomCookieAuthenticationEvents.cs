using LinqToDB.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Website
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private SignInManager _signInManager;
        //private IWebSession _session;
        public CustomCookieAuthenticationEvents(SignInManager signInManager)
        {
            _signInManager = signInManager;
            //_session = webSession;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;
            if (!userPrincipal.Identity.IsAuthenticated)
            {
                return;
            }

            var session = context.HttpContext.Session;
            if (session == null || !session.IsAvailable)
            {
                context.RejectPrincipal();
                _signInManager.Logout();
            }
            else if (session.Keys.Count() == 0)
            {
                context.RejectPrincipal();
                _signInManager.Logout();
            }

            //if (_session == null || _session.UserName.IsEmpty() || !_session.IsAvailable)
            //{
            //    context.RejectPrincipal();
            //    _signInManager.Logout();
            //}
        }
    }
}
