using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VSW.Core.Interface;
using VSW.Core.Models;
using VSW.Core.MVC;
using VSW.Core.Web;

namespace VSW.Lib.Models
{
    public class SysPageEntity : EntityBase, IPageInterface
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int TemplateID { get; set; }

        [DataInfo]
        public int TemplateMobileID { get; set; }

        [DataInfo]
        public int TemplateTabletID { get; set; }

        [DataInfo]
        public string ModuleCode { get; set; }

        [DataInfo]
        public int LangID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }
        [DataInfo]
        public int ParentID { get; set; }
        [DataInfo]
        public int BrandID { get; set; }

        [DataInfo]
        public int State { get; set; }

        [DataInfo]
        public override string Name { get; set; }
        [DataInfo]
        public string AliasName { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string Icon { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public string Summary { get; set; }
        [DataInfo]
        public string LinkTitle { get; set; }

        [DataInfo]
        public string PageHeading { get; set; }

        [DataInfo]
        public string PageTitle { get; set; }
        [DataInfo]
        public string PageCanonical { get; set; }

        [DataInfo]
        public string PageDescription { get; set; }

        [DataInfo]
        public string PageKeywords { get; set; }
        [DataInfo]
        public string TopContent { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public string Custom { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }

        [DataInfo]
        public DateTime Updated { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public string SchemaJson { get; set; }
        public bool NoIndex { get; set; }

        #endregion Autogen by VSW

        #region SEO

        public bool PageState { get; set; } = true;

        public string PageURL { get; set; } = string.Empty;

        private string _pageFile = string.Empty;

        public string PageFile
        {
            get => _pageFile.IndexOf("base64", StringComparison.OrdinalIgnoreCase) > -1 ? string.Empty : _pageFile;
            set => _pageFile = value;
        }

        #endregion SEO

        public bool HasEnd { get; set; }

        public bool Root => ParentID == 0;

        public bool End => Items.GetValue("End").ToBool();

        private string _oBrand = string.Empty;
        public string Brand
        {
            get
            {
                if (string.IsNullOrEmpty(_oBrand))
                {
                    var ArrName = Name.Split(' ');
                    _oBrand = ArrName[ArrName.Length - 1];
                }

                return _oBrand;
            }
        }

        private SysPageEntity _oParent;
        public SysPageEntity Parent
        {
            get
            {
                if (_oParent == null)
                    _oParent = SysPageService.Instance.GetByID_Cache(ParentID);

                return _oParent ?? (_oParent = new SysPageEntity());
            }
        }

        private WebMenuEntity _oMenu;
        public WebMenuEntity Menu
        {
            get
            {
                if (_oMenu == null && MenuID > 0)
                    _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

                return _oMenu ?? (_oMenu = new WebMenuEntity());
            }
        }

        private List<WebPropertyEntity> _oGetBrand;
        public List<WebPropertyEntity> GetBrand
        {
            get
            {
                if (_oGetBrand != null) return _oGetBrand ?? (_oGetBrand = new List<WebPropertyEntity>());

                var listProperty = WebPropertyService.Instance.GetByParent_Cache(Menu.PropertyID);
                if (listProperty != null)
                    _oGetBrand = WebPropertyService.Instance.GetByParent_Cache(listProperty[0].ID);

                return _oGetBrand ?? (_oGetBrand = new List<WebPropertyEntity>());
            }
        }

        public long Count
        {
            get
            {
                return ModProductService.Instance.CreateQuery()
                                    .Select(o => o.ID)
                                    .Where(o => o.Activity == true)
                                    .WhereIn(o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("Product", MenuID, LangID))
                                    .Count()
                                    .ToValue_Cache()
                                    .ToLong(0);
            }
        }
    }

    public class SysPageService : ServiceBase<SysPageEntity>, IPageServiceInterface
    {
        #region Autogen by VSW

        public SysPageService() : base("[Sys_Page]")
        {
        }

        private static SysPageService _instance;

        public static SysPageService Instance => _instance ?? (_instance = new SysPageService());

        #endregion Autogen by VSW

        public SysPageEntity GetByID(int id)
        {
            return CreateQuery().Where(o => o.ID == id).ToSingle();
        }

        public SysPageEntity GetByCode(string code)
        {
            return CreateQuery().Where(o => o.Code == code).ToSingle();
        }

        public SysPageEntity GetByID_Cache(int id)
        {
            if (GetAll_Cache() == null) return null;

            return GetAll_Cache().Find(o => o.ID == id);
        }

        public List<SysPageEntity> GetByParent_Cache(int parentID)
        {
            if (GetAll_Cache() == null) return null;

            var list = GetAll_Cache().FindAll(o => o.ParentID == parentID);

            list.Sort((o1, o2) => o1.Order.CompareTo(o2.Order));

            return list;
        }
        public List<SysPageEntity> GetByParent_Cache(int parentID, int State)
        {
            if (GetAll_Cache() == null) return null;

            var list = GetAll_Cache().FindAll(o => o.ParentID == parentID);
            if (State > 0 && list != null)
            {
                list = list.FindAll(o => (o.State & State) == State);
            }

            list.Sort((o1, o2) => o1.Order.CompareTo(o2.Order));

            return list;
        }

        public List<SysPageEntity> GetByParent_Cache(SysPageEntity page, bool brand, int top)
        {
            if (GetAll_Cache() == null) return null;

            List<SysPageEntity> list;

            if (!brand)
            {
                list = GetAll_Cache().FindAll(o => o.ParentID == page.ID && o.BrandID < 1);
                if (list.Count < 1)
                    list = GetAll_Cache().FindAll(o => o.ParentID == page.ParentID && o.BrandID < 1);
            }
            else
                list = GetAll_Cache().FindAll(o => o.ParentID == page.ID && o.BrandID > 0);

            if (list.Count <= 1) return list;

            if (list.Count > top)
                list = list.GetRange(0, top);

            list.Sort((o1, o2) => o1.Order.CompareTo(o2.Order));

            return list;
        }

        public List<SysPageEntity> GetByParent_Cache(SysPageEntity page, bool brand)
        {
            if (GetAll_Cache() == null) return null;

            List<SysPageEntity> list;

            if (brand)
            {
                list = GetAll_Cache().FindAll(o => o.ParentID == page.ID && o.BrandID > 0);
                if (list.Count < 1)
                    list = GetAll_Cache().FindAll(o => o.ParentID == page.ParentID && o.BrandID > 0);
            }
            else
            {
                list = GetAll_Cache().FindAll(o => o.ParentID == page.ID && o.BrandID < 1);
                if (list.Count < 1)
                    list = GetAll_Cache().FindAll(o => o.ParentID == page.ParentID && o.BrandID < 1);
            }

            list.Sort((o1, o2) => o1.Order.CompareTo(o2.Order));

            return list;
        }

        public List<SysPageEntity> GetByGrandParent_Cache(int parentID)
        {
            if (GetAll_Cache() == null) return null;

            var list = GetAll_Cache().FindAll(o => o.ParentID == parentID);

            var listGrand = new List<SysPageEntity>();
            foreach (var t in list)
            {
                var temp = GetAll_Cache().FindAll(o => o.ParentID == t.ID);
                listGrand.AddRange(temp);
            }

            if (listGrand.Count > 0)
                listGrand.Sort((o1, o2) => o1.Order.CompareTo(o2.Order));

            return listGrand;
        }

        public List<SysPageEntity> GetByParent_Cache(int parentID, string module)
        {
            if (GetAll_Cache() == null) return null;

            var list = GetAll_Cache().FindAll(o => o.ParentID == parentID && o.ModuleCode == module);

            if (list.Count == 0) return null;

            list.Sort((o1, o2) => o1.Order.CompareTo(o2.Order));

            return list;
        }

        public string GetMapCode_Cache(SysPageEntity page)
        {
            var keyCache = Cache.CreateKey(/*"Sys_Page",*/ "GetMapCode_Cache." + page.ID);

            string mapCode;
            var obj = Cache.GetValue(keyCache);
            if (obj != null)
            {
                mapCode = obj.ToString();
            }
            else
            {
                var tempPage = page;

                mapCode = tempPage.Code;
                while (tempPage.ParentID > 0)
                {
                    var parentId = tempPage.ParentID;

                    tempPage = CreateQuery()
                           .Where(o => o.ID == parentId)
                           .ToSingle_Cache();

                    if (tempPage == null || tempPage.Root)
                        break;

                    mapCode = tempPage.Code + "/" + mapCode;
                }

                Cache.SetValue(keyCache, mapCode);
            }

            return mapCode;
        }

        private List<SysPageEntity> GetAll_Cache()
        {
            return CreateQuery().Where(o => o.Activity == true).ToList_Cache();
        }

        #region IPageServiceInterface Members

        public IPageInterface VSW_Core_GetByID(int id)
        {
            return GetAll_Cache().Find(o => o.ID == id);
        }

        //public IPageInterface VSW_Core_CurrentPage(ViewPage viewPage)
        //{
        //    var code = viewPage.CurrentVQS.GetString(0);

        //    if (code == string.Empty || code.Length > 260) return null;

        //    if (string.Equals(code.ToLower(), "ajax", StringComparison.OrdinalIgnoreCase) || code.StartsWith("sitemap", StringComparison.OrdinalIgnoreCase) || string.Equals(code, "rss", StringComparison.OrdinalIgnoreCase))
        //    {
        //        if (code.ToLower().StartsWith("sitemap") && viewPage.CurrentVQS.Count > 1)
        //            HttpRequest.Redirect301(viewPage.GetURL(code));

        //        var template = SysTemplateService.Instance.CreateQuery()
        //                                    .Select(o => o.ID)
        //                                    .ToSingle_Cache();

        //        if (template == null) return null;

        //        viewPage.CurrentVQSMVC.Trunc(viewPage.CurrentVQS, 1);
        //        if (code.StartsWith("sitemap", StringComparison.OrdinalIgnoreCase))
        //        {
        //            VSW.Lib.Global.Error.Write(code);
        //            return new SysPageEntity
        //            {
        //                TemplateID = template.ID,
        //                TemplateMobileID = template.ID,
        //                Name = "Sitemap",
        //                Code = "sitemap",
        //                ModuleCode = "MSitemap",
        //                Activity = true
        //            };

        //        }

        //        switch (code.ToLower())
        //        {
        //            case "ajax":
        //                return new SysPageEntity
        //                {
        //                    TemplateID = template.ID,
        //                    TemplateMobileID = template.ID,
        //                    Name = "Ajax",
        //                    Code = "ajax",
        //                    ModuleCode = "MAjax",
        //                    Activity = true
        //                };

        //            case "rss":
        //                return new SysPageEntity
        //                {
        //                    TemplateID = template.ID,
        //                    TemplateMobileID = template.ID,
        //                    Name = "Đọc tin RSS",
        //                    Code = "rss",
        //                    ModuleCode = "MRss",
        //                    Activity = true
        //                };
        //        }
        //    }

        //    var cleanUrl = ModCleanURLService.Instance.GetByCode(code, viewPage.CurrentLang.ID);
        //    if (cleanUrl == null)
        //    {
        //        cleanUrl = ModCleanURLService.Instance.CreateQuery().Where(o => o.Code.StartsWith(code) && o.LangID == viewPage.CurrentLang.ID).ToSingle();
        //        if (cleanUrl != null)
        //            HttpRequest.Redirect301(viewPage.GetURL(cleanUrl.Code));

        //        return null;
        //    }

        //    if (viewPage.CurrentVQS.Count > 1 && cleanUrl.MenuID > 0)
        //        HttpRequest.Redirect301(viewPage.GetURL(viewPage.CurrentVQS.EndCode));

        //    viewPage.ViewBag.CleanURL = cleanUrl;

        //    if (cleanUrl.Type == "Page")
        //    {
        //        viewPage.CurrentVQSMVC.Trunc(viewPage.CurrentVQS, 1);
        //        return GetByID_Cache(cleanUrl.Value);
        //    }

        //    //viewPage.CurrentVQSMVC.Trunc(viewPage.CurrentVQS, 0);
        //    viewPage.CurrentVQSMVC.Trunc(viewPage.CurrentVQS, viewPage.CurrentVQS.Count - 1);

        //    SysPageEntity page = null;

        //    var menuId = cleanUrl.MenuID;
        //    while (menuId > 0)
        //    {
        //        var id = menuId;
        //        page = CreateQuery().Where(o => o.MenuID == id && o.BrandID < 1 && o.Activity == true).ToSingle_Cache();

        //        if (page != null) break;

        //        var menu = WebMenuService.Instance.GetByID_Cache(menuId);

        //        if (menu == null || menu.ParentID == 0) break;

        //        menuId = menu.ParentID;
        //    }

        //    return page;
        //}
        public IPageInterface VSW_Core_CurrentPage(ViewPage viewPage)
        {
            var code = viewPage.CurrentVQS.EndCode;
            if (code == string.Empty || code.Length > 260) return null;
            var cleanUrl = ModCleanURLService.Instance.GetByCode(code, viewPage.CurrentLang.ID);
            if (cleanUrl == null)
            {
                code = viewPage.CurrentVQS.GetString(0);
                if (string.Equals(code.ToLower(), "ajax", StringComparison.OrdinalIgnoreCase) || code.StartsWith("sitemap", StringComparison.OrdinalIgnoreCase) || string.Equals(code, "rss", StringComparison.OrdinalIgnoreCase))
                {
                    if (code.ToLower().StartsWith("sitemap") && viewPage.CurrentVQS.Count > 1)
                        VSW.Core.Web.HttpRequest.Redirect301(viewPage.GetURL(code));

                    var template = SysTemplateService.Instance.CreateQuery()
                                                .Select(o => o.ID)
                                                .ToSingle_Cache();

                    if (template == null) return null;

                    viewPage.CurrentVQSMVC.Trunc(viewPage.CurrentVQS, 1);

                    if (code.StartsWith("sitemap", StringComparison.OrdinalIgnoreCase))
                    {
                        return new SysPageEntity
                        {
                            TemplateID = template.ID,
                            TemplateMobileID = template.ID,
                            Name = "Sitemap",
                            Code = "sitemap",
                            ModuleCode = "MSitemap",
                            Activity = true
                        };
                    }

                    switch (code.ToLower())
                    {
                        case "ajax":
                            return new SysPageEntity
                            {
                                TemplateID = template.ID,
                                TemplateMobileID = template.ID,
                                Name = "Ajax",
                                Code = "ajax",
                                ModuleCode = "MAjax",
                                Activity = true
                            };

                        case "rss":
                            return new SysPageEntity
                            {
                                TemplateID = template.ID,
                                TemplateMobileID = template.ID,
                                Name = "Đọc tin RSS",
                                Code = "rss",
                                ModuleCode = "MRss",
                                Activity = true
                            };
                    }
                }

                if (code == string.Empty || code.Length > 260) return null;
                cleanUrl = ModCleanURLService.Instance.GetByCode(code, viewPage.CurrentLang.ID);
            }

            if (cleanUrl == null)
            {
                cleanUrl = ModCleanURLService.Instance.CreateQuery().Where(o => o.Type == "Page").Where("'" + code + "' like [Code] + '%'").OrderBy("[CodeLength] DESC").ToSingle_Cache();

                if (cleanUrl == null)
                {
                    cleanUrl = ModCleanURLService.Instance.CreateQuery().Where(o => o.Code.StartsWith(code) && o.LangID == viewPage.CurrentLang.ID).ToSingle();
                    if (cleanUrl != null)
                        VSW.Core.Web.HttpRequest.Redirect301(viewPage.GetURL(cleanUrl.Code));
                    else VSW.Core.Web.HttpRequest.Redirect301(Core.Web.HttpRequest.Domain);
                }
            }
            SysPageEntity page = null;
            if (cleanUrl != null)
            {
                //if (viewPage.CurrentVQS.Count > 1 && cleanUrl.MenuID > 0)
                //    VSW.Core.Web.HttpRequest.Redirect301(viewPage.GetURL(viewPage.CurrentVQS.EndCode));

                viewPage.ViewBag.CleanURL = cleanUrl;

                if (cleanUrl.Type == "Page")
                {
                    viewPage.CurrentVQSMVC.Trunc(viewPage.CurrentVQS, 1);
                    page = GetByID_Cache(cleanUrl.Value);
                    return page;
                }

                //viewPage.CurrentVQSMVC.Trunc(viewPage.CurrentVQS, 0);
                viewPage.CurrentVQSMVC.Trunc(viewPage.CurrentVQS, viewPage.CurrentVQS.Count - 1);

                var menuId = cleanUrl.MenuID;
                while (menuId > 0)
                {
                    var id = menuId;
                    page = CreateQuery().Where(o => o.MenuID == id && o.BrandID < 1 && o.Activity == true).ToSingle_Cache();

                    if (page != null) break;

                    var menu = WebMenuService.Instance.GetByID_Cache(menuId);

                    if (menu == null || menu.ParentID == 0) break;

                    menuId = menu.ParentID;
                }
            }
            return page;
        }

        public void VSW_Core_CPSave(IPageInterface item)
        {
            Save(item as SysPageEntity);
        }

        #endregion IPageServiceInterface Members

        public List<SysPageEntity> GetChildIDForWeb_Cache(int pageId, int langId, int state = 0)
        {
            var keyCache = "Lib.App.WebPage.GetChildIDForWeb." + pageId + "." + langId;

            List<SysPageEntity> cacheValue;
            var obj = Core.Web.Cache.GetValue(keyCache);
            if (obj != null)
            {
                cacheValue = obj as List<SysPageEntity>;
            }
            else
            {
                var list = new List<SysPageEntity>();

                var listPage = CreateQuery()
                                    .Where(o => o.Activity == true && o.LangID == langId)
                                    .Where(state > 0, o => (o.State & state) == state)
                                    .Select(o => new { o.ID, o.ParentID })
                                    .ToList_Cache();

                GetChildIDForWeb_Cache(ref list, listPage, pageId, null);

                cacheValue = list;
                Core.Web.Cache.SetValue(keyCache, cacheValue);
            }
            return cacheValue;
        }

        private static void GetChildIDForWeb_Cache(ref List<SysPageEntity> list, List<SysPageEntity> listWebPage, int pageId, SysPageEntity page)
        {
            if (page != null) list.Add(page);
            if (page != null) pageId = page.ID;
            if (listWebPage == null)
                return;

            var listPage = listWebPage.FindAll(o => o.ParentID == pageId);
            foreach (var t in listPage)
            {
                if (page != null) pageId = page.ID;
                GetChildIDForWeb_Cache(ref list, listWebPage, t.ID, t);
            }
        }
        public List<SysPageEntity> GetChildIDModuleProductForWeb_Cache(int pageId, int langId, int state = 0)
        {
            var keyCache = "Lib.App.WebPage.GetChildIDModuleProductForWeb." + pageId + "." + langId;

            List<SysPageEntity> cacheValue;
            var obj = Core.Web.Cache.GetValue(keyCache);
            if (obj != null)
            {
                cacheValue = obj as List<SysPageEntity>;
            }
            else
            {
                var list = new List<SysPageEntity>();

                var listPage = CreateQuery()
                                    .Where(o => o.Activity == true && o.LangID == langId)
                                    .Where(state > 0, o => (o.State & state) == state)
                                    .Select(o => new { o.ID, o.ParentID, o.Name, o.Code, o.MenuID, o.ModuleCode })
                                    .ToList_Cache();

                GetChildIDForWeb_Cache(ref list, listPage, pageId, null);

                cacheValue = list;
                Core.Web.Cache.SetValue(keyCache, cacheValue);
            }
            return cacheValue;
        }
    }
}