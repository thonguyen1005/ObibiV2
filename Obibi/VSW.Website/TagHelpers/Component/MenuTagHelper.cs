using LinqToDB;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using VSW.Core;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Interface;

namespace VSW.Website.TagHelpers
{
    [HtmlTargetElement("vsw-menu")]
    public class MenuTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("PageID")]
        public int PageID { get; set; }
        [HtmlAttributeName("State")]
        public int State { get; set; }
        [HtmlAttributeName("Title")]
        public string Title { get; set; }

        private SysPageRepository _repo = null;
        public MenuTagHelper(IWorkingContext<MenuTagHelper> workingContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor) : base(viewRenderService, httpContextAccessor)
        {
            _repo = new SysPageRepository(context: workingContext);
        }
        private string PartialView = "/Views/Partial/CMenu/{0}.cshtml";
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (ViewLayout.IsEmpty() && DefaultLayout.IsNotEmpty()) { ViewLayout = DefaultLayout; }
            if (ViewLayout.IsEmpty())
            {
                output.Content.SetHtmlContent("");
                return;
            }

            var page = _repo.Get(PageID);

            var dbQuery = _repo.GetTable()
                                        .Where(o => o.LangID == CurrentSite.LangID && o.Activity == true)
                                        .Where(o => o.ParentID == PageID);
            if (State > 0)
            {
                dbQuery = dbQuery.Where(o => (o.State & State) == State);
            }
            var model = await dbQuery.OrderBy(o => new { o.Order, o.ID })
                                        .ToListAsync();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                ["Page"] = page,
                ["Title"] = Title
            };

            string layout = PartialView.Format(ViewLayout);
            var html = await _viewRenderService.PartialAsync(layout, model, viewData);
            output.Content.SetHtmlContent(html);
        }
    }
}
