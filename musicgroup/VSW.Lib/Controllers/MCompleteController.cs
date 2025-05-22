using VSW.Lib.Global;
using VSW.Lib.MVC;
using Controller = VSW.Lib.MVC.Controller;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO: Đặt hàng thành công", Code = "MComplete", Order = 11)]
    public class MCompleteController : Controller
    {
        public void ActionIndex(MCompleteModel model)
        {
            string vrCode = Cookies.GetValue("vrCode");
            Cookies.Remove("vrCode");

            //if (string.IsNullOrEmpty(model.code) || Data.FormatRemoveSql(model.code) != vrCode)
            //    ViewPage.Response.Redirect("/");
            ViewBag.Model = model;
            //SEO
            ViewPage.CurrentPage.PageURL = ViewPage.CurrentURL;
            ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(ViewPage.CurrentPage.File);
        }
    }

    public class MCompleteModel
    {
        public string code { get; set; }
        public string returnpath { get; set; }
        public string Error { get; set; }
        public int OrderID { get; set; }
    }
}