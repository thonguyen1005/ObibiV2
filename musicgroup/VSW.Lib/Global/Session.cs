using System.Web;

namespace VSW.Lib.Global
{
    public static class Session
    {
        public static bool Exists(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }

        public static void SetValue(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static object GetValue(string key)
        {
            return Exists(key) ? HttpContext.Current.Session[key] : string.Empty;
        }

        public static void Remove(string key)
        {
            if (Exists(key))
                HttpContext.Current.Session[key] = null;
        }
    }
}