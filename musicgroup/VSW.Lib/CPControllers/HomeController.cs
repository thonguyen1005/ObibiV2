using VSW.Lib.Global;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class HomeController : CPController
    {
        public void ActionIndex()
        {
        }

        public void ActionLogout()
        {
            CPViewPage.SetLog("Thoát khỏi hệ thống.");

            CPLogin.Logout();

            CPViewPage.CPRedirect("Login.aspx");
        }
    }
}