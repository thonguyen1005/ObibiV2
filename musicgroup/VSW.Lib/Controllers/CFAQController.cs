using VSW.Lib.MVC;
using VSW.Lib.Models;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "ĐK : Hỏi đáp", Code = "CFAQ", IsControl = true, Order = 9)]
    public class CFAQController : Controller
    {
        [Core.MVC.PropertyInfo("Chuyên mục", "Type|FAQ")]
        public int MenuID;
        [Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 4;
        [Core.MVC.PropertyInfo("Tiêu đề")]
        public string Title;

        public override void OnLoad()
        {
            ViewBag.Data = ModFAQService.Instance.CreateQuery()
                                    .Where(o => o.Activity == true)
                                    .WhereIn(MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("FAQ", MenuID, ViewPage.CurrentLang.ID))
                                    .OrderByDesc(o => new { o.Order, o.ID })
                                    .Take(PageSize)
                                    .ToList_Cache();

            ViewBag.Title = Title;
        }
    }
}