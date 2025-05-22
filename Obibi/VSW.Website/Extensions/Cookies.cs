using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Website
{
    public static class Cookies
    {
        public static bool Set(string key, string value, TimeSpan? expiredIn = null)
        {
            var context = HttpRequestExtensions.GetContext();
            var option = new CookieOptions();
            if (expiredIn != null && expiredIn.HasValue)
            {
                option.Expires = DateTime.Now.Add(expiredIn.Value);
            }

            context.Response.Cookies.Append(key, value, option);
            return true;
        }

        public static string Get(string key)
        {
            var context = HttpRequestExtensions.GetContext();
            if (context.Request.Cookies.TryGetValue(key, out string v))
            {
                return v;
            }

            return null;
        }

        public static bool Exist(string key)
        {
            var context = HttpRequestExtensions.GetContext();
            return context.Request.Cookies.ContainsKey(key);
        }

        public static bool Remove(string key)
        {
            var context = HttpRequestExtensions.GetContext();
            context.Response.Cookies.Delete(key);
            return true;
        }
    }
}
