using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "ĐK : Menu", Code = "CMenu", IsControl = true, Order = 2)]
    public class CMenuController : Controller
    {
        [Core.MVC.PropertyInfo("Trang")]
        public int PageID;
        [Core.MVC.PropertyInfo("Vị trí", "ConfigKey|Mod.PageState")]
        public int State;
        [Core.MVC.PropertyInfo("Tiêu đề")]
        public string Title;

        public override void OnLoad()
        {
            var page = SysPageService.Instance.GetByID_Cache(PageID);
            if (page != null)
            {
                ViewBag.Page = page;
                ViewBag.Data = SysPageService.Instance.GetByParent_Cache(page.ID, State);
            }
            else
            {
                ViewBag.Data = SysPageService.Instance.CreateQuery()
                                        .Where(o => o.LangID == ViewPage.CurrentLang.ID && o.Activity == true)
                                        .Where(State > 0, o => (o.State & State) == State)
                                        .OrderByAsc(o => new { o.Order, o.ID })
                                        .ToList_Cache();
            }
            ViewBag.Title = Title;
        }
    }
}