using VSW.Core.Global;
using VSW.Lib.Models;

namespace VSW.Lib.Global
{
    public class WebLogin
    {
        public static ModWebUserEntity CurrentUser
        {
            get
            {
                var webPage = Core.Web.Application.CurrentViewPage;

                var obj = webPage?.PageViewState["Web.CurrentUser"];

                if (obj != null)
                    return (ModWebUserEntity)obj;

                ModWebUserEntity webUser = null;

                if (WebUserID > 0)
                    webUser = ModWebUserService.Instance.GetForLogin(WebUserID);

                if (webUser == null)
                    Logout();

                if (webPage != null)
                    webPage.PageViewState["Web.CurrentUser"] = webUser;

                return webUser;
            }
        }

        public static string Name => CurrentUser.Name;

        public static int WebUserID => Convert.ToInt(Cookies.GetValue("Web.Login", true));

        public static bool IsLogin()
        {
            return CurrentUser != null;
        }

        public static void Logout()
        {
            Cookies.Remove("Web.Login");
            Cookies.Remove("SessionCart");
        }

        public static void SetLogin(int webUserId, bool savepass)
        {
            Cookies.SetValue("Web.Login", webUserId.ToString(), savepass ? 0 : 120, true);
        }
    }
}