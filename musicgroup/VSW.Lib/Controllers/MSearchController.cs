using System;
using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;
using System.Collections.Generic;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO : Tìm kiếm", Code = "MSearch", Order = 50)]
    public class MSearchController : Controller
    {
        [VSW.Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 20;
        public void ActionIndex(MSearchModel model)
        {
            string keyword = !string.IsNullOrEmpty(model.keyword) ? Data.GetCode(model.keyword) : string.Empty;
            string menulist = WebMenuService.Instance.GetChildIDForWeb_Cache("Product", model.searchmenuid, ViewPage.CurrentLang.ID);
            string wherein = "";
            if (!string.IsNullOrEmpty(menulist))
            {
                string[] arrmenu = menulist.Split(',');
                for (int i = 0; arrmenu != null && i < arrmenu.Length; i++)
                {
                    if (string.IsNullOrEmpty(arrmenu[i].Trim())) continue;
                    wherein += (!string.IsNullOrEmpty(wherein) ? " or " : "") + " CHARINDEX('" + arrmenu[i].Trim() + "', [MenuListID]) > 0 ";
                }
            }
            var dbQuery = ModProductService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion, o.Summary })
                                    .Where(o => o.Activity == true)
                                    .Where(!string.IsNullOrEmpty(keyword), o => (o.Code.Contains(keyword) || o.Model.Contains(keyword)))
                                    .Where(model.searchmenuid > 0, wherein)
                                    .Take(PageSize)
                                    .Skip(PageSize * model.page)
                                    .OrderByDesc(o => new { o.Order, o.ID });

            ViewBag.Data = dbQuery.ToList_Cache();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;

            //SEO
            ViewPage.CurrentPage.PageURL = ViewPage.CurrentURL;
            ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetCropFile(ViewPage.CurrentPage.File, 200, 200);
        }
    }

    public class MSearchModel
    {
        private int _page;
        public int page
        {
            get { return _page; }
            set { _page = value - 1; }
        }

        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
        public string keyword { get; set; }
        public int searchmenuid { get; set; }
    }
}
