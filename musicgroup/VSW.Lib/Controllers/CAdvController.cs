using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "ĐK : Quảng cáo / Liên kết", Code = "CAdv", IsControl = true, Order = 1)]
    public class CAdvController : Controller
    {
        [Core.MVC.PropertyInfo("Chuyên mục", "Type|Adv")]
        public int MenuID;
        [Core.MVC.PropertyInfo("Dữ liệu")]
        public bool MultiRecord = true;
        [Core.MVC.PropertyInfo("Tiêu đề")]
        public string Title;
        public override void OnLoad()
        {
            if (!MultiRecord)
                ViewBag.Data = ModAdvService.Instance.CreateQuery()
                                        .Where(o => o.Activity == true && o.MenuID == MenuID)
                                        .OrderByAsc(o => new { o.Order, o.ID })
                                        .Take(1)
                                        .ToSingle_Cache();
            else
                ViewBag.Data = ModAdvService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.MenuID == MenuID)
                                            .OrderByAsc(o => new { o.Order, o.ID })
                                            .ToList_Cache();

            ViewBag.Title = Title;
        }
    }
}