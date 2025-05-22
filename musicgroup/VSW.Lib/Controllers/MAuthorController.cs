using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO: Tác giả", Code = "MAuthor", Order = 9)]
    public class MAuthorController : Controller
    {
        [Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 10;

        public void ActionIndex(MAuthorModel model)
        {

            var dbQuery = ModAuthorService.Instance.CreateQuery()
                                    .Skip(PageSize * model.page)
                                    .Take(PageSize)
                                    .OrderByDesc(o => new { o.ID });

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
            var item = ModAuthorService.Instance.CreateQuery()
                                .Where(o => o.Code == endCode)
                                .ToSingle_Cache();

            if (item != null)
            {
                ViewBag.Data = item;

                //SEO
                ViewPage.CurrentPage.PageURL = ViewPage.GetURL("author/" + item.Code);
                ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(item.File);
                ViewPage.CurrentPage.PageTitle = string.IsNullOrEmpty(item.PageTitle) ? item.Name : item.PageTitle;
                ViewPage.CurrentPage.PageDescription = item.PageDescription;
                ViewPage.CurrentPage.PageKeywords = item.PageKeywords;
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
            else
            {
                ViewPage.Error404();
            }
        }
    }

    public class MAuthorModel
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