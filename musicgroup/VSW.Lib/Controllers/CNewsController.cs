using VSW.Lib.MVC;
using VSW.Lib.Models;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "ĐK : Tin tức", Code = "CNews", IsControl = true, Order = 2)]
    public class CNewsController : Controller
    {
        [Core.MVC.PropertyInfo("Chuyên mục", "Type|News")]
        public int MenuID;
        [Core.MVC.PropertyInfo("Trang")]
        public int PageID;
        [Core.MVC.PropertyInfo("Vị trí", "ConfigKey|Mod.NewsState")]
        public int State;
        [Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 4;
        [Core.MVC.PropertyInfo("Tiêu đề")]
        public string Title;

        public override void OnLoad()
        {
            ViewBag.Data = ModNewsService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name, o.MenuID, o.State, o.Code, o.File, o.View, o.Published, o.Order, o.Activity, o.Summary, o.AuthorID })
                                    .Where(o => o.Activity == true)
                                    .Where(State > 0, o => (o.State & State) == State)
                                    .WhereIn(MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("News", MenuID, ViewPage.CurrentLang.ID))
                                    .OrderByDesc(o => new { o.Order, o.ID })
                                    .Take(PageSize)
                                    .ToList_Cache();

            ViewBag.Page = SysPageService.Instance.GetByID_Cache(PageID);
            ViewBag.Title = Title;
        }
    }
}