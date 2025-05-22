using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO: Sitemap", Code = "MSitemap", Order = 50)]
    public class MSitemapController : Controller
    {
        private List<string> listSitemap = null;

        public void ActionIndex()
        {
            listSitemap = new List<string>();
            listSitemap.Add("sitemap");
            listSitemap.Add("sitemap-misc");
            listSitemap.Add("sitemap-category");
            listSitemap.Add("sitemap-sub-category");
            listSitemap.Add("sitemap-product");
            listSitemap.Add("sitemap-blog");
            listSitemap.Add("sitemap-page");

            string endCode = ViewPage.CurrentVQS.EndCode.ToLower();
            if (endCode.Equals("sitemap-misc", StringComparison.Ordinal))
            {
                RedirectToAction("Misc");
                return;
            }
            else if (endCode.Equals("sitemap-category", StringComparison.Ordinal))
            {
                RedirectToAction("Category");
                return;
            }
            else if (endCode.Equals("sitemap-sub-category", StringComparison.Ordinal))
            {
                RedirectToAction("Category");
                return;
            }
            else if (endCode.Equals("sitemap-page", StringComparison.Ordinal))
            {
                RedirectToAction("Page");
                return;
            }
            else if (endCode.StartsWith("sitemap-product", StringComparison.Ordinal))
            {
                RedirectToAction("Product");
                return;
            }
            else if (endCode.StartsWith("sitemap-blog", StringComparison.Ordinal))
            {
                RedirectToAction("News");
                return;
            }

            Core.Global.Sitemap._siteMapList = null;
            Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/sitemap-misc.xml", DateTime.Now);
            Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/sitemap-category.xml", DateTime.Now);
            Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/sitemap-sub-category.xml", DateTime.Now);

            //product
            var smName = "sitemap-product";
            Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/" + smName + ".xml");
            listSitemap.Add(smName);
            //var listProduct = ModProductService.Instance.CreateQuery()
            //                                .Select(o => o.Updated)
            //                                .Where(o => o.Activity == true)
            //                                .OrderByDesc(o => o.Updated)
            //                                .ToList_Cache();

            //Dictionary<string, DateTime> dicSmProduct = new Dictionary<string, DateTime>();
            //for (int i = 0; listProduct != null && i < listProduct.Count; i++)
            //{
            //    var smName = "sitemap-product-" + $"{listProduct[i].Updated:yyyy-MM-dd}";
            //    if (!dicSmProduct.ContainsKey(smName)) dicSmProduct[smName] = listProduct[i].Updated;
            //}

            //foreach (var o in dicSmProduct)
            //{
            //    Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/" + o.Key + ".xml", o.Value);

            //    listSitemap.Add(o.Key);
            //}

            //news
            smName = "sitemap-blog";
            Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/" + smName + ".xml");
            listSitemap.Add(smName);
            //var listNews = ModNewsService.Instance.CreateQuery()
            //                                .Select(o => o.Updated)
            //                                .Where(o => o.Activity == true)
            //                                .OrderByDesc(o => o.Updated)
            //                                .ToList_Cache();

            //Dictionary<string, DateTime> dicSmNews = new Dictionary<string, DateTime>();
            //for (int i = 0; listNews != null && i < listNews.Count; i++)
            //{
            //    if (listNews[i].Updated <= DateTime.MinValue) continue;
            //    var smName = "sitemap-news-" + $"{listNews[i].Updated:yyyy-MM-dd}";
            //    if (!dicSmNews.ContainsKey(smName)) dicSmNews[smName] = listNews[i].Updated;
            //}

            //foreach (var o in dicSmNews)
            //{
            //    Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/" + o.Key + ".xml", o.Value);

            //    listSitemap.Add(o.Key);
            //}

            Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/sitemap-page.xml", DateTime.Now);
            try
            {
                if (!listSitemap.Exists(o => o.Equals(endCode)))
                {
                    ViewPage.Error404();
                }

                string sitemap = Core.Global.Sitemap.BuiltRootXml();

                ViewPage.Response.ContentType = "text/xml";
                ViewPage.Response.Write(sitemap);
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message + " - Có lỗi xảy ra khi tạo sitemap.");

                ViewPage.Response.Write("Có lỗi xảy ra khi tạo sitemap.");
            }

            ViewPage.Response.End();
        }
        public void ActionMisc()
        {
            RenderView("Index");

            Core.Global.Sitemap._siteMapList = null;

            Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Domain, DateTime.Now, "1.0", Core.Global.ChangeFrequency.Daily);
            Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + (Core.Web.HttpRequest.IsLocal ? ":" + Core.Web.HttpRequest.Port : "") + "/sitemap.xml", DateTime.Now, "0.5", Core.Global.ChangeFrequency.Daily);

            try
            {
                string sitemap = Core.Global.Sitemap.BuiltXml();

                ViewPage.Response.ContentType = "text/xml";
                ViewPage.Response.Write(sitemap);
            }
            catch (Exception ex)
            {
                Error.Write(ex.Message + " - Có lỗi xảy ra khi tạo sitemap.");

                ViewPage.Response.Write("Có lỗi xảy ra khi tạo sitemap.");
            }

            ViewPage.Response.End();
        }
        public void ActionCategory()
        {
            RenderView("Index");

            string endCode = ViewPage.CurrentVQS.EndCode.ToLower();
            Core.Global.Sitemap._siteMapList = null;

            var listPage = SysPageService.Instance.CreateQuery()
                                    .Where(o => o.Activity == true && (o.ModuleCode == "MProduct") && o.LangID == 1 && o.TemplateID > 0 && o.MenuID > 0)
                                    .OrderByAsc(o => o.Created)
                                    .ToList_Cache();

            List<int> listMenuId = new List<int>();
            var menuProduct = WebMenuService.Instance.GetByParentIDAndType_Cache("Product", 0);
            if (menuProduct != null)
            {
                listMenuId.Add(menuProduct[0].ID);
                var listSubLv1 = WebMenuService.Instance.GetByParentID_Cache(menuProduct[0].ID);
                for (int j = 0; listSubLv1 != null && j < listSubLv1.Count; j++)
                {
                    listMenuId.Add(listSubLv1[j].ID);
                }
            }
            if (endCode.Equals("sitemap-category", StringComparison.Ordinal))
            {
                listPage = listPage.Where(o => listMenuId.Contains(o.MenuID) && o.MenuID > 0).OrderBy(o => o.Order).ToList();
            }
            else if (endCode.Equals("sitemap-sub-category", StringComparison.Ordinal))
            {
                listPage = listPage.Where(o => !listMenuId.Contains(o.ID)).OrderBy(o => o.ParentID).ThenBy(o => o.Order).ToList();
            }
            for (var i = 0; listPage != null && i < listPage.Count; i++)
            {
                var cleanURL = ModCleanURLService.Instance.CreateQuery()
                                                .Where(o => o.Type == "Page" && o.Code == listPage[i].Code && o.LangID == 1)
                                                .ToSingle_Cache();

                if (cleanURL == null || cleanURL.MenuID < 1 || cleanURL.ID < 1) continue;

                var url = ViewPage.GetPageURL(listPage[i]);
                var lastmod = listPage[i].Updated <= DateTime.MinValue ? (listPage[i].Created <= DateTime.MinValue ? DateTime.Now : listPage[i].Created) : listPage[i].Updated;

                Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Domain + "/" + (Setting.Sys_MultiSite ? ViewPage.CurrentSite.Code : "") + url, lastmod, "0.95", Core.Global.ChangeFrequency.Daily);
            }
            if (endCode.Equals("sitemap-sub-category", StringComparison.Ordinal))
            {
                var listUrlSeoActive = ModUrlSeoActivityService.Instance.CreateQuery().OrderByAsc(o => o.ID).ToList_Cache();
                for (var i = 0; listUrlSeoActive != null && i < listUrlSeoActive.Count; i++)
                {
                    Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Domain + "/" + (Setting.Sys_MultiSite ? ViewPage.CurrentSite.Code : "") + listUrlSeoActive[i].UrlRedirect + Setting.Sys_PageExt, DateTime.Now, "0.95", Core.Global.ChangeFrequency.Daily);
                }
            }
            try
            {
                string sitemap = Core.Global.Sitemap.BuiltXml();

                ViewPage.Response.ContentType = "text/xml";
                ViewPage.Response.Write(sitemap);
            }
            catch
            {
                ViewPage.Error404();
            }

            ViewPage.Response.End();
        }
        public void ActionProduct()
        {
            RenderView("Index");

            string endCode = ViewPage.CurrentVQS.EndCode.ToLower();
            //endCode = endCode.Replace("sitemap-product-", "");

            //var minDate = Core.Global.Convert.ToDateTime(endCode + "-1");
            //var maxDate = Core.Global.Convert.ToDateTime(endCode + "-31");
            //if (maxDate <= DateTime.MinValue) maxDate = Core.Global.Convert.ToDateTime(endCode + "-30");
            //if (maxDate <= DateTime.MinValue) maxDate = Core.Global.Convert.ToDateTime(endCode + "-29");
            //if (maxDate <= DateTime.MinValue) maxDate = Core.Global.Convert.ToDateTime(endCode + "-28");

            //if (minDate <= DateTime.MinValue || maxDate <= DateTime.MinValue) ViewPage.Error404();

            Core.Global.Sitemap._siteMapList = null;

            //product
            var listProduct = ModProductService.Instance.CreateQuery()
                                            .Select(o => new { o.Code, o.Published, o.Updated })
                                            .Where(o => o.Activity == true)
                                            //.Where(o => o.Updated >= minDate && o.Updated <= maxDate)
                                            .OrderByDesc(o => o.Updated)
                                            .ToList_Cache();

            for (var i = 0; listProduct != null && i < listProduct.Count; i++)
            {
                var cleanURL = ModCleanURLService.Instance.CreateQuery()
                                                .Where(o => o.Type == "Product" && o.Code == listProduct[i].Code && o.LangID == 1)
                                                .ToSingle_Cache();

                if (cleanURL == null || cleanURL.MenuID < 1 || cleanURL.ID < 1) continue;

                var url = ViewPage.GetURL(listProduct[i].Code);
                var lastmod = listProduct[i].Updated <= DateTime.MinValue ? listProduct[i].Published : listProduct[i].Updated;

                Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Domain + "/" + (Setting.Sys_MultiSite ? ViewPage.CurrentSite.Code : "") + url, lastmod, "0.9", Core.Global.ChangeFrequency.Daily);
            }

            try
            {
                string sitemap = Core.Global.Sitemap._siteMapList != null ? Core.Global.Sitemap.BuiltXml() : string.Empty;

                ViewPage.Response.ContentType = "text/xml";
                ViewPage.Response.Write(sitemap);

            }
            catch
            {
                ViewPage.Error404();
                //ViewPage.Response.Write(ex.Message);
            }

            ViewPage.Response.End();
        }
        public void ActionNews()
        {
            RenderView("Index");

            string endCode = ViewPage.CurrentVQS.EndCode.ToLower();
            //endCode = endCode.Replace("sitemap-news-", "");

            //var minDate = Core.Global.Convert.ToDateTime(endCode + "-1");
            //var maxDate = Core.Global.Convert.ToDateTime(endCode + "-31");
            //if (maxDate <= DateTime.MinValue) maxDate = Core.Global.Convert.ToDateTime(endCode + "-30");
            //if (maxDate <= DateTime.MinValue) maxDate = Core.Global.Convert.ToDateTime(endCode + "-29");
            //if (maxDate <= DateTime.MinValue) maxDate = Core.Global.Convert.ToDateTime(endCode + "-28");

            //if (minDate <= DateTime.MinValue || maxDate <= DateTime.MinValue) ViewPage.Error404();

            Core.Global.Sitemap._siteMapList = null;

            //product
            var listNews = ModNewsService.Instance.CreateQuery()
                                    .Select(o => new { o.Code, o.Published, o.Updated })
                                    .Where(o => o.Activity == true)
                                    //.Where(o => o.Updated >= minDate && o.Updated <= maxDate)
                                    .OrderByDesc(o => o.Updated)
                                    .ToList_Cache();

            for (var i = 0; listNews != null && i < listNews.Count; i++)
            {
                var cleanURL = ModCleanURLService.Instance.CreateQuery()
                                                .Where(o => o.Type == "News" && o.Code == listNews[i].Code && o.LangID == 1)
                                                .ToSingle_Cache();

                if (cleanURL == null || cleanURL.MenuID < 1 || cleanURL.ID < 1) continue;

                var url = ViewPage.GetURL(listNews[i].Code);
                var lastmod = listNews[i].Updated <= DateTime.MinValue ? listNews[i].Published : listNews[i].Updated;

                Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Domain + "/" + (Setting.Sys_MultiSite ? ViewPage.CurrentSite.Code : "") + url, lastmod, "0.95", Core.Global.ChangeFrequency.Daily);
            }

            try
            {
                string sitemap = Core.Global.Sitemap.BuiltXml();

                ViewPage.Response.ContentType = "text/xml";
                ViewPage.Response.Write(sitemap);

                Ping(Core.Web.HttpRequest.Scheme + "://" + Core.Web.HttpRequest.Host + "/" + endCode + ".xml");
            }
            catch
            {
                ViewPage.Error404();
            }

            ViewPage.Response.End();
        }
        public void ActionPage()
        {
            RenderView("Index");

            Core.Global.Sitemap._siteMapList = null;

            var listPage = SysPageService.Instance.CreateQuery()
                                    .Where(o => o.Activity == true && (o.ModuleCode == "MNews" || o.ModuleCode == "MContent" || o.ModuleCode == "MFeedback") && o.LangID == 1 && o.TemplateID > 0)
                                    .OrderByAsc(o => o.Created)
                                    .ToList_Cache();

            for (var i = 0; listPage != null && i < listPage.Count; i++)
            {
                var cleanURL = ModCleanURLService.Instance.CreateQuery()
                                                .Where(o => o.Type == "Page" && o.Code == listPage[i].Code && o.LangID == 1)
                                                .ToSingle_Cache();

                if (cleanURL == null || cleanURL.MenuID < 1 || cleanURL.ID < 1) continue;

                var url = ViewPage.GetPageURL(listPage[i]);
                var lastmod = listPage[i].Updated <= DateTime.MinValue ? (listPage[i].Created <= DateTime.MinValue ? DateTime.Now : listPage[i].Created) : listPage[i].Updated;

                Core.Global.Sitemap.AddLocation(Core.Web.HttpRequest.Domain + "/" + (Setting.Sys_MultiSite ? ViewPage.CurrentSite.Code : "") + url, lastmod, "0.8", Core.Global.ChangeFrequency.Daily);
            }

            try
            {
                string sitemap = Core.Global.Sitemap.BuiltXml();

                ViewPage.Response.ContentType = "text/xml";
                ViewPage.Response.Write(sitemap);
            }
            catch
            {
                ViewPage.Error404();
            }

            ViewPage.Response.End();
        }
        private static void Ping(string url)
        {
            //GOOGLE
            try
            {
                var request = System.Net.WebRequest.Create(Core.Web.HttpRequest.Scheme + "://www.google.com/webmasters/tools/ping?sitemap=" + url);
                request.GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("Ping sitemap to google had error - " + ex.Message);
            }
        }
    }
}