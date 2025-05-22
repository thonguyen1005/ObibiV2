using System.Web;

namespace VSW.Lib.Global
{
    public class Application
    {
        public static void OnError()
        {
            //lay loi tu server
            var ex = HttpContext.Current.Server.GetLastError();

            //ghi lai loi
            if (ex.InnerException != null)
                Error.Write(ex.InnerException.InnerException ?? ex.InnerException);

            Error.Write(ex);

            //chay che do release
            if (Core.Global.Application.Debug)
                return;

            ////trang hien tai la webpage
            if (Core.Web.Application.CurrentViewPage is MVC.ViewPage webPage)
            {
                if (webPage.CurrentSite != null && webPage.CurrentPage != null)
                {
                    // loi xay ra khong phai trang chu
                    if (webPage.CurrentSite.PageID != webPage.CurrentPage.ID && ex is HttpException)
                    {
                        //khong hien ra loi
                        HttpContext.Current.Server.ClearError();

                        Core.Web.HttpRequest.Redirect301(Core.Web.HttpRequest.Domain);
                    }
                }
            }

            //khong hien ra loi
            HttpContext.Current.Server.ClearError();
        }
    }
}