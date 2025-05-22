using System;
using System.Collections.Generic;
using System.Linq;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;
namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO : Thương hiệu", Code = "MBrand", Order = 3)]
    public class MBrandController : Controller
    {
        [Core.MVC.PropertyInfo("Thương hiệu", "Type|Brand")]
        public int BrandID;
        [Core.MVC.PropertyInfo("Vị trí", "ConfigKey|Mod.ProductState")]
        public int State;
        [Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 20;
        public void ActionIndex(MBrandModel model)
        {
            var dbQuery = ModProductService.Instance.CreateQuery()
                                   .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion })
                                    .Where(o => o.Activity == true)
                                    .Where(State > 0, o => (o.State & State) == State)
                                    .Where(o => o.BrandID == BrandID)
                                    .Skip(PageSize * model.page)
                                    .Take(PageSize)
                                    .OrderByDesc(o => new { o.Order, o.ID });

            ViewBag.Data = dbQuery.ToList_Cache();
            model.TotalRecord = dbQuery.TotalRecord;
            model.PageSize = PageSize;
            ViewBag.Model = model;

            //SEO
            ViewPage.CurrentPage.PageURL = ViewPage.CurrentURL;
            ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(ViewPage.CurrentPage.File);
            //kiem tra do dai meta title co lon hơn 70 khong
            if (!string.IsNullOrEmpty(ViewPage.CurrentPage.PageTitle))
            {
                ViewPage.CurrentPage.PageTitle = ViewPage.CurrentPage.PageTitle.Length > 70 ? ViewPage.CurrentPage.PageTitle.Substring(0, 70) + "..." : ViewPage.CurrentPage.PageTitle;
            }
            //kiem tra do dai meta description co lon hơn 300 khong
            if (!string.IsNullOrEmpty(ViewPage.CurrentPage.PageDescription))
            {
                ViewPage.CurrentPage.PageDescription = ViewPage.CurrentPage.PageDescription.Length > 300 ? ViewPage.CurrentPage.PageDescription.Substring(0, 300) + "..." : ViewPage.CurrentPage.PageDescription;
            }
        }

        public void ActionDetail(string endCode)
        {
            var item = WebMenuService.Instance.CreateQuery()
                                .Where(o => o.Code == endCode)
                                .ToSingle_Cache();

            if (item != null)
            {
                var model = new MBrandModel();
                TryUpdateModel(model);
                BrandID = item.ID;
                var dbQuery = ModProductService.Instance.CreateQuery()
                                   .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion })
                                    .Where(o => o.Activity == true)
                                    .Where(State > 0, o => (o.State & State) == State)
                                    .Where(o => o.BrandID == BrandID)
                                    .Skip(PageSize * model.page)
                                    .Take(PageSize)
                                    .OrderByDesc(o => new { o.Order, o.ID });

                ViewBag.Data = dbQuery.ToList_Cache();
                model.TotalRecord = dbQuery.TotalRecord;
                model.PageSize = PageSize;
                ViewBag.Model = model;
            }
            else
            {
                ViewPage.Error404();
            }
        }
    }

    public class MBrandModel
    {
        private int _page;
        public int page
        {
            get { return _page; }
            set { _page = value - 1; }
        }

        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
    }
}