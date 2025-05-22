using VSW.Lib.Global;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "ĐK : Text", Code = "CText", IsControl = true, Order = 99)]
    public class CTextController : Controller
    {
        [Core.MVC.PropertyInfo("Văn bản / Html")]
        public string Text;
        [Core.MVC.PropertyInfo("Tiêu đề")]
        public string Title = string.Empty;

        public override void OnLoad()
        {
            ViewBag.Text = !string.IsNullOrEmpty(Text) ? Data.Base64Decode(Text) : string.Empty;

            ViewBag.Title = Title;
        }
    }
}