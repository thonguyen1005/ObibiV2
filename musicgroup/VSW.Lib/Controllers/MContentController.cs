using VSW.Lib.Global;
using VSW.Lib.MVC;
using Controller = VSW.Lib.MVC.Controller;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO: Bài viết", Code = "MContent", Order = 8)]
    public class MContentController : Controller
    {
        public void ActionIndex()
        {
            //SEO
            ViewPage.CurrentPage.PageURL = (ViewPage.CurrentURL == "/" ? "" : ViewPage.CurrentURL);
            ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(ViewPage.CurrentPage.File);
        }
    }
}