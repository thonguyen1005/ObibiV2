using System.Globalization;
using System.Web;

namespace VSW.Lib.Global
{
    public static class CPResource
    {
        private static string CurrentCode => CultureInfo.CurrentCulture.Name;

        public static string GetValue(string code)
        {
            return GetValue(code, string.Empty);
        }

        public static string GetValue(string code, string defalt)
        {
            var resourceService = new IniResourceService(HttpContext.Current.Server.MapPath("~/" + Core.Web.Setting.Sys_CPDir + "/Views/Lang/" + CurrentCode + ".ini"));
            return resourceService.VSW_Core_GetByCode(code, defalt);
        }
    }
}