using System;
using System.Linq;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "ĐK : Sản phẩm", Code = "CProduct", IsControl = true, Order = 3)]
    public class CProductController : Controller
    {
        [Core.MVC.PropertyInfo("Chuyên mục", "Type|Product")]
        public int MenuID;
        [Core.MVC.PropertyInfo("Trang")]
        public int PageID;
        [Core.MVC.PropertyInfo("Vị trí", "ConfigKey|Mod.ProductState")]
        public int State;
        [Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 5;
        [Core.MVC.PropertyInfo("Tiêu đề")]
        public string Title;

        public override void OnLoad()
        {
            var dbQuery = ModProductService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion })
                                    .Where(o => o.Activity == true)
                                    .Where(State > 0, o => (o.State & State) == State)
                                    .Take(PageSize)
                                    .WhereIn(MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("Product", MenuID, ViewPage.CurrentLang.ID))
                                    .OrderByAsc(o => new { o.Order, o.ID });

            ViewBag.Data = dbQuery.ToList_Cache();
            ViewBag.Page = SysPageService.Instance.GetByID_Cache(PageID);
            ViewBag.Title = Title;
        }
    }
}