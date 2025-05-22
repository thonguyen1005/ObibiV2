using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using VSW.Core.Interface;
using VSW.Lib.Global;
using VSW.Lib.Models;

namespace VSW.Lib.MVC
{
    public class ViewPage : Core.MVC.ViewPage
    {
        public ViewPage()
        {
            LangService = SysLangService.Instance;
            ModuleService = SysModuleService.Instance;
            SiteService = SysSiteService.Instance;
            TemplateService = SysTemplateService.Instance;
            PageService = SysPageService.Instance;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ResourceService = new IniSqlResourceService(CurrentLang);

            //lượt truy cập
            //Lib.Global.Utils.UpdateOnline();
        }

        protected override IPageInterface PageNotFound()
        {
            Error404();
            return null;
        }

        public void Error404()
        {
            Core.Web.HttpRequest.Error404();
        }

        public new SysSiteEntity CurrentSite => base.CurrentSite as SysSiteEntity;
        public new SysTemplateEntity CurrentTemplate => base.CurrentTemplate as SysTemplateEntity;
        public new SysPageEntity CurrentPage => base.CurrentPage as SysPageEntity;
        public new SysLangEntity CurrentLang => base.CurrentLang as SysLangEntity;

        public ModCleanURLEntity CurrentCleanUrl => ViewBag.CleanURL as ModCleanURLEntity;

        private string _currentUrl;
        public string CurrentURL => _currentUrl ?? (_currentUrl = GetPageURL(CurrentPage));

        //Module
        private string _feedbackUrl;
        public string FeedbackUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_feedbackUrl)) return _feedbackUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MFeedback" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _feedbackUrl = GetURL(item.Code);

                return _feedbackUrl;
            }
        }

        private string _searchUrl;
        public string SearchUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_searchUrl)) return _searchUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MSearch" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _searchUrl = GetURL(item.Code);

                return _searchUrl;
            }
        }

        private string _favoriteUrl;
        public string FavoriteUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_favoriteUrl)) return _favoriteUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MFavorite" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _favoriteUrl = GetURL(item.Code);

                return _favoriteUrl;
            }
        }

        private string _recentlyUrl;
        public string RecentlyUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_recentlyUrl)) return _recentlyUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MRecently" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _recentlyUrl = GetURL(item.Code);

                return _recentlyUrl;
            }
        }
        private string _compareUrl;
        public string CompareUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_compareUrl)) return _compareUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MCompare" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _compareUrl = GetURL(item.Code);

                return _compareUrl;
            }
        }
        private string _viewCartUrl;
        public string ViewCartUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_viewCartUrl)) return _viewCartUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MViewCart" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _viewCartUrl = GetURL(item.Code);

                return _viewCartUrl;
            }
        }
        private string _viewOderCartUrl;
        public string ViewOderCartUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_viewOderCartUrl)) return _viewOderCartUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MCart" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _viewOderCartUrl = GetURL(item.Code);

                return _viewOderCartUrl;
            }
        }

        private string _checkOutUrl;
        public string CheckOutUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_checkOutUrl)) return _checkOutUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MCheckOut" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _checkOutUrl = GetURL(item.Code);

                return _checkOutUrl;
            }
        }

        private string _completeUrl;
        public string CompleteUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_completeUrl)) return _completeUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MComplete" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _completeUrl = GetURL(item.Code);

                return _completeUrl;
            }
        }
        private string _checkoutUrl;
        public string CheckoutUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_checkoutUrl)) return _checkoutUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MCheckout" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _checkoutUrl = GetURL(item.Code);

                return _checkoutUrl;
            }
        }
        private string _loginUrl;
        public string LoginUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_loginUrl)) return _loginUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MLogin" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _loginUrl = GetURL(item.Code);

                return _loginUrl;
            }
        }

        private string _registerUrl;
        public string RegisterUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_registerUrl)) return _registerUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MRegister" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _registerUrl = GetURL(item.Code);

                return _registerUrl;
            }
        }

        private string _forgotUrl;
        public string ForgotUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_forgotUrl)) return _forgotUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MForgot" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _forgotUrl = GetURL(item.Code);

                return _forgotUrl;
            }
        }

        private string _oAccountURL;
        public string AccountURL
        {
            get
            {
                if (!string.IsNullOrEmpty(_oAccountURL))
                    return _oAccountURL;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MAccount" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null)
                    _oAccountURL = GetURL(item.Code);

                return _oAccountURL;
            }
        }

        private string _OrderUrl;
        public string OrderUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_OrderUrl)) return _OrderUrl;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MOrder" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null) _OrderUrl = GetURL(item.Code);

                return _OrderUrl;
            }
        }
        private string _oLogoutURL;
        public string LogoutURL
        {
            get
            {
                if (!string.IsNullOrEmpty(_oLogoutURL))
                    return _oLogoutURL;

                var item = SysPageService.Instance.CreateQuery()
                                    .Select(o => o.Code)
                                    .Where(o => o.Activity == true && o.ModuleCode == "MAccount" && o.LangID == CurrentLang.ID)
                                    .ToSingle_Cache();

                if (item != null)
                    _oLogoutURL = GetURL(item.Code + "/logout");

                return _oLogoutURL;
            }
        }

        public Message Message { get; } = new Message();

        public string SortMode
        {
            get
            {
                var sort = Core.Web.HttpQueryString.GetValue("sort").ToString().ToLower().Trim();

                if (sort == "new_asc" || sort == "price_asc" || sort == "price_desc" || sort == "view_desc")
                    return sort;

                return "new_asc";
            }
        }

        public string GetURL(string key, string value)
        {
            var url = string.Empty;
            for (var i = 0; i < PageViewState.Count; i++)
            {
                var tempKey = PageViewState.AllKeys[i];
                var tempValue = PageViewState[tempKey].ToString();

                if (string.Equals(tempKey, key, StringComparison.OrdinalIgnoreCase) || string.Equals(tempKey, "vsw", StringComparison.OrdinalIgnoreCase) || tempKey.IndexOf("web.", StringComparison.OrdinalIgnoreCase) >= 0)
                    continue;

                if (url.Length == 0)
                    url = "?" + tempKey + "=" + Server.UrlEncode(tempValue);
                else
                    url += "&" + tempKey + "=" + Server.UrlEncode(tempValue);
            }

            url += (url == string.Empty ? "?" : "&") + key + "=" + value;

            return url;
        }

        public string GetURL(int menuID, string code)
        {
            return GetURL(code);
        }

        public string GetPageURL(SysPageEntity page)
        {
            if (page.Code == "/") return "/";
            var typeValue = page.Items.GetValue("Type").ToString();

            if (typeValue.Length == 0)
                return GetURL(page.Code);

            if (!typeValue.Equals("http", StringComparison.OrdinalIgnoreCase)) return "#";

            var target = page.Items.GetValue("Target").ToString();
            var url = page.Items.GetValue("URL").ToString();

            if (url.Length == 0)
                url = page.Code;

            return url.Replace("{URLBase}/", URLBase).Replace("{PageExt}", PageExt) + (target == string.Empty ? string.Empty : "\" target=\"" + target);
        }

        public bool IsPageActived(SysPageEntity pageToCheck)
        {
            if (CurrentPage.ID == pageToCheck.ID)
                return true;

            var page = (SysPageEntity)CurrentPage.Clone();
            while (true)
            {
                page = SysPageService.Instance.GetByID_Cache(page.ParentID);

                if (page == null || page.ParentID == 0)
                    return false;

                if (page.ID == pageToCheck.ID)
                    return true;
            }
        }

        public bool IsPageActived(SysPageEntity page, int index)
        {
            return CurrentPage.ID == page.ID || CurrentVQS.Equals(index, page.Code);
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

        public void RefreshPage()
        {
            Response.Redirect(Request.RawUrl);
        }

        #region zebradialog

        public void Alert(string title, string content)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) return;

            var html = @"<script type=""text/javascript"">$(document).ready(function ($) {Alert('" + title + @"', '" + content + @"');})</script>";

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "AlertScript", html);
        }

        public void Alert(string content)
        {
            Alert("Thông báo !", content);
        }

        public void Redirect(string title, string content, string action)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(action)) return;

            var html = @"<script type=""text/javascript"">$(document).ready(function ($) {Redirect('" + title + @"', '" + content + @"', '" + action + @"');})</script>";

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "AlertScript", html);
        }

        public void Redirect(string content, string action)
        {
            Redirect("Thông báo !", content, action);
        }

        public void Redirect(string content)
        {
            Redirect("Thông báo !", content, "/");
        }

        public void Confirm(string title, string content, string action)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(action)) return;

            var html = @"<script type=""text/javascript"">$(document).ready(function ($) {Confirm('" + title + @"', '" + content + @"', '" + action + @"');})</script>";

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmScript", html);
        }
        public void Success(string title, string content, string action)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content) || string.IsNullOrEmpty(action)) return;
            var html = @"<script type=""text/javascript"">$(document).ready(function ($) {Success('" + title + @"', '" + content + @"', '" + action + @"');})</script>";

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmScript", html);
        }

        public void Confirm(string content, string action)
        {
            Confirm("Thông báo !", content, action);
        }

        public void Confirm(string content)
        {
            Confirm("Thông báo !", content, "/");
        }

        #endregion zebradialog
        public string GetHtmlControl(Controller controller, string pathView, string viewLayout)
        {
            try
            {
                Control control = new Control();
                ViewControl viewControl = LoadControl(pathView) as ViewControl;
                viewControl.VSWID = viewLayout;
                viewControl.ViewData = controller.ViewData;
                viewControl.ViewBag = controller.ViewBag;
                viewControl.ViewPage = this;
                control.Controls.Add(viewControl);
                StringBuilder stringBuilder = new StringBuilder();
                StringWriter stringWriter = new StringWriter(stringBuilder);
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
                control.RenderControl(htmlTextWriter);
                string text = stringBuilder.ToString();
                stringWriter.Close();
                htmlTextWriter.Close();

                ResourceService = new IniSqlResourceService(CurrentLang);
                bool flag4 = ResourceService != null && this.CurrentLang != null;
                if (flag4)
                {
                    MatchCollection matchCollection = new Regex("\\{RS:[\\w]+\\}").Matches(text);
                    for (int i = 0; i < matchCollection.Count; i++)
                    {
                        string text2 = matchCollection[i].Value.Replace("{RS:", string.Empty).Replace("}", string.Empty);
                        text = text.Replace("{RS:" + text2 + "}", ResourceService.VSW_Core_GetByCode(text2, string.Empty));
                    }
                }
                return text;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}