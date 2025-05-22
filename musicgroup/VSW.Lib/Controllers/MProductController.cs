using System;
using System.Collections.Generic;
using System.Linq;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;
namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO : Sản phẩm", Code = "MProduct", Order = 2)]
    public class MProductController : Controller
    {
        [Core.MVC.PropertyInfo("Chuyên mục", "Type|Product")]
        public int MenuID;
        [Core.MVC.PropertyInfo("Thương hiệu", "Type|Brand")]
        public int BrandID;
        [Core.MVC.PropertyInfo("Vị trí", "ConfigKey|Mod.ProductState")]
        public int State;
        [Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 20;
        [Core.MVC.PropertyInfo("Giao diện chi tiết", "Chi tiết|56")]
        public int TemplateDetailID = 56;
        public void ActionIndex(MProductModel model)
        {
            model.PageSize = PageSize;
            model.state = State;

            ViewPage.ViewBag.Model = ViewBag.Model = model;

            string menulist = WebMenuService.Instance.GetChildIDForWeb_Cache("Product", MenuID, ViewPage.CurrentLang.ID);
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
            var dbQuery2 = ModProductService.Instance.CreateQuery()
                                    .Select(o => new { o.BrandID })
                                    .Where(o => o.Activity == true)
                                    .Where(State > 0, o => (o.State & State) == State);
            
            dbQuery2.Where(MenuID > 0, wherein);
           
            var listBrand = dbQuery2.GroupBy(" [BrandID] ").ToList_Cache();
            ViewPage.ViewBag.ListBrand = ViewBag.ListBrand = listBrand;

            string urlFull = ViewPage.Request.RawUrl;
            if (urlFull.StartsWith("/")) urlFull = urlFull.Remove(0, 1);
            if (urlFull.Contains("?")) urlFull = urlFull.Split('?')[0].Trim();
            //SEO
            ViewPage.CurrentPage.PageURL = ViewPage.CurrentURL;
            ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(ViewPage.CurrentPage.File);
           
            ModUrlSeoActivityEntity SeoUrlPage = ModUrlSeoActivityService.Instance.GetByUrlCache(urlFull);
            
            if (SeoUrlPage != null)
            {
                ViewPage.CurrentPage.PageURL = ViewPage.Request.RawUrl;
                ViewPage.CurrentPage.PageTitle = SeoUrlPage.PageTitle;
                ViewPage.CurrentPage.PageDescription = SeoUrlPage.PageDescription;
                ViewPage.CurrentPage.PageKeywords = SeoUrlPage.PageKeywords;
                if (!string.IsNullOrEmpty(SeoUrlPage.File))
                {
                    ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(SeoUrlPage.File);
                }
                if (!string.IsNullOrEmpty(SeoUrlPage.SchemaJson))
                {
                    ViewPage.CurrentPage.SchemaJson = SeoUrlPage.SchemaJson;
                }
                ViewBag.Content = SeoUrlPage.Content;
            }

            ViewPage.CurrentPage.PageCanonical = Core.Web.HttpRequest.Domain + "/" + urlFull + Setting.Sys_PageExt;
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
            if (ViewPage.CurrentPage.MenuID > 0)
                MenuID = ViewPage.CurrentPage.MenuID;

            var item = ModProductService.Instance.CreateQuery()
                                .Where(o => o.Code == endCode)
                                .ToSingle_Cache();

            if (item != null)
            {
                //neu k phai thiet bi cam thay thi moi doi template
                if (TemplateDetailID > 0)
                {
                    var template = SysTemplateService.Instance.GetByID(TemplateDetailID);
                    if (template != null)
                        ViewPage.ChangeTemplate(template);
                }

                //up view
                item.UpView();

                ViewBag.Other = ModProductService.Instance.CreateQuery()
                            .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion })
                            .Where(o => o.ID != item.ID)
                            .Where(o => o.Activity == true)
                            .WhereIn(item.MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("News", item.MenuID, ViewPage.CurrentLang.ID))
                            .OrderByAsc(o => new { o.Price, o.Order, o.ID })
                            .Take(PageSize)
                            .ToList_Cache();

                if (item.GroupMenuID > 0)
                {
                    ViewBag.PhienBan = ModProductService.Instance.CreateQuery()
                            .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion })
                            .Where(o => o.Activity == true)
                            .Where(o => o.GroupMenuID == item.GroupMenuID)
                            .OrderByAsc(o => o.PhienBan)
                            .ToList_Cache();
                }

                //var listPropertysShow = WebPropertyConfigService.Instance.CreateQuery()
                //                            .Where(o => o.MenuID == item.MenuID && o.ShowBreadCrumb == true)
                //                            .ToList();
                //if (listPropertysShow != null)
                //{
                //    string joinP = VSW.Core.Global.Array.ToString(listPropertysShow.Select(o => o.PropertyID).ToList().ToArray());
                //    var listPropertys = ModPropertyService.Instance.CreateQuery()
                //                    .Where(o => o.ProductID == item.ID)
                //                    .WhereIn(o => o.PropertyValueID, joinP)
                //                    .ToList();

                //    ViewPage.ViewBag.Propertys = ViewBag.Propertys = listPropertys;
                //}

                ViewPage.ViewBag.Data = ViewBag.Data = item;
                ViewPage.ViewBag.BrandID = ViewBag.BrandID = item.BrandID;
                ViewPage.ViewBag.MenuID = ViewBag.MenuID = item.MenuID;

                //SEO
                ViewPage.CurrentPage.PageURL = ViewPage.GetURL(item.MenuID, item.Code);
                ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(item.File);
                ViewPage.CurrentPage.PageTitle = string.IsNullOrEmpty(item.PageTitle) ? item.Name : item.PageTitle;
                ViewPage.CurrentPage.PageDescription = string.IsNullOrEmpty(item.PageDescription) ? item.Summary : item.PageDescription;
                ViewPage.CurrentPage.PageKeywords = !string.IsNullOrEmpty(item.PageKeywords) ? item.PageKeywords : ViewPage.CurrentPage.PageKeywords;
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
                if (!string.IsNullOrEmpty(item.SchemaJson))
                {
                    ViewPage.CurrentPage.SchemaJson = item.SchemaJson;
                }
            }
            else
            {
                ViewPage.Error404();
            }
        }
    }

    public class MProductModel
    {
        private int _page;
        public int page
        {
            get { return _page; }
            set { _page = value - 1; }
        }

        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
        public int state { get; set; }
        public string sort { get; set; }
        public string c { get; set; }
        public string b { get; set; }
    }
}