using System;
using VSW.Core.Interface;
using VSW.Core.Web;
using VSW.Lib.CPControllers;
using VSW.Lib.Global;
using VSW.Lib.Models;
using Setting = VSW.Core.Web.Setting;

namespace VSW.Lib.MVC
{
    public class CPViewPage : Core.MVC.CPViewPage
    {
        public Message Message { get; } = new Message();

        public CPUserEntity CurrentUser { get; private set; }
        public Permissions UserPermissions { get; private set; }

        private string _moduleCode;

        public CPViewPage()
        {
            var langCode = Global.Cookies.GetValue("CP.Lang", true);

            //ngon ngu mac dinh neu chua co
            if (langCode?.Length == 0) langCode = "vi-VN";

            CurrentLang = new SysLangEntity { Code = langCode };

            ResourceService = new IniResourceService(Server.MapPath("~/" + Setting.Sys_CPDir + "/Views/Lang/" + langCode + ".ini"));
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (_moduleCode != null && string.Equals(_moduleCode, "login", StringComparison.OrdinalIgnoreCase))
                MasterPageFile = "Views/Shared/Login.Master";
            else
            {
                if (_moduleCode != null && _moduleCode.ToLower().StartsWith("form"))
                    MasterPageFile = "Views/Shared/Form.Master";
                else
                    MasterPageFile = "Views/Shared/Main.Master";

                CurrentUser = CPLogin.CurrentUser;

                if (CurrentUser == null)
                {
                    CPRedirect("Login.aspx?ReturnPath=" + Server.UrlEncode(Request.RawUrl));
                    return;
                }

                if (_moduleCode != null)
                {
                    _moduleCode = _moduleCode.ToLower();

                    UserPermissions = _moduleCode.StartsWith("mod") || _moduleCode.StartsWith("sys")
                        ? CurrentUser.GetPermissionsByModule(_moduleCode.StartsWith("sys")
                            ? "SysAdministrator"
                            : _moduleCode)
                        : new Permissions(31);
                }
                else
                    UserPermissions = new Permissions(31);

                if (UserPermissions == null || UserPermissions != null && !UserPermissions.Any)
                    CurrentModule = AccessDeniedModule();
            }
        }

        public override IModuleInterface DefaultModule()
        {
            return new ModuleInfo
            {
                Code = "Home",
                ModuleType = typeof(HomeController)
            };
        }

        public IModuleInterface AccessDeniedModule()
        {
            return new ModuleInfo
            {
                Code = "AccessDenied",
                ModuleType = typeof(AccessDeniedController)
            };
        }

        public override IModuleInterface FindModule(string moduleCode)
        {
            _moduleCode = moduleCode;

            return new ModuleInfo
            {
                Code = moduleCode,
                ModuleType = Type.GetType("VSW.Lib.CPControllers." + moduleCode + "Controller")
            };
        }

        public void Back(int step)
        {
            JavaScript.Back(step, Page);
        }

        public void Navigate(string url)
        {
            JavaScript.Navigate(url, Page);
        }

        public void Close()
        {
            JavaScript.Close(Page);
        }

        public void Script(string key, string script)
        {
            JavaScript.Script(key, script, Page);
        }

        public void CPRedirectHome()
        {
            Response.Redirect("~/" + Setting.Sys_CPDir + "/Default.aspx");
        }

        public void CPRedirect(string path)
        {
            Response.Redirect("~/" + Setting.Sys_CPDir + "/" + path);
        }

        public void RefreshPage()
        {
            Response.Redirect(Request.RawUrl);
        }

        public void SetLog(string message)
        {
            var log = new CPUserLogEntity
            {
                UserID = CPLogin.UserID,
                IP = HttpRequest.IP,
                Created = DateTime.Now,
                Note = message
            };
            CPUserLogService.Instance.Save(log);
        }

        public void SetMessage(string message)
        {
            Global.Cookies.SetValue("message", Data.Base64Encode(message));
        }
        public void SetMessage_error(string message)
        {
            Global.Cookies.SetValue("message_error", Data.Base64Encode(message));
        }
        #region zebradialog

        public void Alert(string title, string content)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) return;

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "AlertScript", string.Format("<script type=\"text/javascript\">zebra_alert('" + Data.EscapeQuote(title) + "', '" + Data.EscapeQuote(content) + "');</script>"));
        }

        public void Alert(string content)
        {
            Alert("Thông báo !", content);
        }

        public void AlertThenRedirect(string title, string content, string redirect)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(redirect)) return;

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmScript", string.Format("<script type=\"text/javascript\">zebra_infor('" + Data.EscapeQuote(title) + "','" + Data.EscapeQuote(content) + "','" + redirect + "');</script>"));
        }

        public void AlertThenRedirect(string content, string redirect)
        {
            AlertThenRedirect("Thông báo !", content, redirect);
        }

        public void AlertThenRedirect(string content)
        {
            AlertThenRedirect("Thông báo !", content, "/");
        }

        public void Confirm(string title, string content, string redirect)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(redirect))
            {
                return;
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmScript", string.Format("<script type=\"text/javascript\">zebra_confirm('" + Data.EscapeQuote(title) + "','" + Data.EscapeQuote(content) + "','" + redirect + "');</script>"));
        }

        public void Confirm(string content, string redirect)
        {
            Confirm("Thông báo !", content, redirect);
        }

        public void Confirm(string content)
        {
            Confirm("Thông báo !", content, "/");
        }

        #endregion zebradialog
    }
}