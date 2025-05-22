using VSW.Lib.Global;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class LoginController : CPController
    {
        public void ActionIndex()
        {
        }

        public void ActionLogin(LoginModel loginModel)
        {
            if (CPLogin.CheckLogin(loginModel.UserName, loginModel.Password))
            {
                CPViewPage.SetLog("Đăng nhập hệ thống.");

                //CPViewPage.CPRedirectHome();

                if (string.IsNullOrEmpty(loginModel.ReturnPath))
                    CPViewPage.CPRedirectHome();
                else
                    CPViewPage.Response.Redirect(CPViewPage.Server.UrlDecode(loginModel.ReturnPath));
            }
            else
            {
                CPViewPage.SetLog("Tên đăng nhập '" + loginModel.UserName + "' không thành công.");

                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("{RS:Login_Error}");
            }
        }

        public void ActionChangeLang(string langCode)
        {
            Cookies.SetValue("CP.Lang", langCode, true);

            CPViewPage.RefreshPage();
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnPath { get; set; }
    }
}