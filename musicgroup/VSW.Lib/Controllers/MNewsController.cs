using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO: Tin tức", Code = "MNews", Order = 1)]
    public class MNewsController : Controller
    {
        [Core.MVC.PropertyInfo("Chuyên mục", "Type|News")]
        public int MenuID;
        [Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 20;
        [Core.MVC.PropertyInfo("Giao diện chi tiết", "Chi tiết|60")]
        public int TemplateDetailID = 60;

        public void ActionIndex(MNewsModel model)
        {

            if (ViewPage.CurrentPage.MenuID > 0)
                MenuID = ViewPage.CurrentPage.MenuID;

            var dbQuery = ModNewsService.Instance.CreateQuery()
                                    .Where(o => o.Activity == true)
                                    .WhereIn(MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("News", MenuID, ViewPage.CurrentLang.ID))
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
            if (ViewPage.CurrentPage.MenuID > 0)
                MenuID = ViewPage.CurrentPage.MenuID;

            var item = ModNewsService.Instance.CreateQuery()
                                .Where(o => o.Activity == true && o.Code == endCode)
                                //.WhereIn(MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("News", MenuID, ViewPage.CurrentLang.ID))
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

                ViewBag.Other = ModNewsService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ID != item.ID)
                                            .WhereIn(MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("News", MenuID, ViewPage.CurrentLang.ID))
                                            .OrderByDesc(o => new { o.Order, o.ID })
                                            .Take(3)
                                            .ToList_Cache();

                ViewPage.ViewBag.Data = ViewBag.Data = item;

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

    public class MNewsModel
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