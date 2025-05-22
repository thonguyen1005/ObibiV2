using System;
using System.Web;

namespace VSW.Lib.Global
{
    public static class Cookies
    {
        private static string SiteID => Core.Web.Setting.Sys_SiteID + "_";

        public static bool Exists(string Key)
        {
            Key = SiteID + Key;

            return HttpContext.Current.Request.Cookies[Key] != null;
        }

        public static void SetValue(string key, string value, int minutes, bool secure)
        {
            key = SiteID + key;

            var IP = HttpContext.Current.Request.UserHostAddress;

            if (IP != "127.0.0.1" && Core.Web.Setting.Mod_DomainCookies != string.Empty)
                HttpContext.Current.Response.Cookies[key].Domain = Core.Web.Setting.Mod_DomainCookies;

            if (secure)
                HttpContext.Current.Response.Cookies[key].Value = Core.Global.CryptoString.Encrypt(IP + "_VSW_" + key + value);
            else
                HttpContext.Current.Response.Cookies[key].Value = value;

            if (minutes > 0)
                HttpContext.Current.Response.Cookies[key].Expires = System.DateTime.Now.AddMinutes(minutes);
        }

        public static void SetValue(string key, string value, int minutes)
        {
            SetValue(key, value, minutes, false);
        }

        public static void SetValue(string key, string value, bool secure)
        {
            SetValue(key, value, 0, secure);
        }

        public static void SetValue(string key, string value)
        {
            SetValue(key, value, 0, false);
        }

        public static string GetValue(string key, bool secure)
        {
            if (!Exists(key))
                return string.Empty;

            key = SiteID + key;

            if (!secure) return HttpContext.Current.Request.Cookies[key].Value;

            var IP = HttpContext.Current.Request.UserHostAddress;

            var decrypt = Core.Global.CryptoString.Decrypt(HttpContext.Current.Request.Cookies[key].Value).Replace(IP + "_VSW_" + key, string.Empty);

            if (decrypt.IndexOf("_VSW_" + key, StringComparison.Ordinal) <= -1) return decrypt;

            Remove(key);

            return string.Empty;
        }

        public static string GetValue(string key)
        {
            return GetValue(key, false);
        }

        public static void Remove(string key)
        {
            if (!Exists(key)) return;

            key = SiteID + key;

            //ip
            var IP = HttpContext.Current.Request.UserHostAddress;

            //local
            if (IP != "127.0.0.1" && Core.Web.Setting.Mod_DomainCookies != string.Empty)
                HttpContext.Current.Response.Cookies[key].Domain = Core.Web.Setting.Mod_DomainCookies;

            //remove
            HttpContext.Current.Response.Cookies[key].Value = string.Empty;
            HttpContext.Current.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
        }
    }
}