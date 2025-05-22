using System;

using VSW.Lib.MVC;
using VSW.Lib.Models;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO : Tag bài viết", Code = "MNewsTag", Order = 3)]
    public class MNewsTagController : Controller
    {
        [VSW.Core.MVC.PropertyInfo("Số lượng")]
        public int PageSize = 20;

        public void ActionDetail(string tagCode)
        {
            MNewsTagModel model = new MNewsTagModel();

            var _NewsTag = ModTagService.Instance.GetByCode(tagCode);

            if (_NewsTag != null)
            {
                var dbQuery = ModNewsService.Instance.CreateQuery()
                                         .Select(o => new { o.ID, o.MenuID, o.Code, o.Name, o.State, o.File, o.Summary, o.Published })
                                         .Where(o => o.Activity == true)
                                         .WhereIn(o => o.ID, ModNewsTagService.Instance.CreateQuery().Select(o => o.NewsID).Where(o => o.TagID == _NewsTag.ID))
                                          .OrderByDesc(o => new { o.Order, o.ID })
                                         .Take(PageSize)
                                         .Skip(PageSize * model.page);

                ViewBag.Data = dbQuery.ToList();
                model.TotalRecord = dbQuery.TotalRecord;

                ViewPage.CurrentPage.PageTitle = !string.IsNullOrEmpty(_NewsTag.Title) ? _NewsTag.Title : _NewsTag.Name;

                if (!string.IsNullOrEmpty(_NewsTag.Description))
                    ViewPage.CurrentPage.PageDescription = _NewsTag.Description;

                if (!string.IsNullOrEmpty(_NewsTag.Keywords))
                    ViewPage.CurrentPage.PageKeywords = _NewsTag.Keywords;
            }
            else
            {
                ViewPage.Response.Redirect("~/");
                return;
            }

            ViewBag.Model = model;
            ViewBag.NewsTag = _NewsTag;
        }
    }

    public class MNewsTagModel
    {
        private int _page;
        public int page
        {
            get { return _page; }
            set { _page = value - 1; }
        }

        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
    }
}
