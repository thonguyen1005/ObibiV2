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
    [HtmlTargetElement("vsw-faq")]
    public class FaqTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("MenuID")]
        public int MenuID { get; set; }
        [HtmlAttributeName("PageSize")]
        public int PageSize { get; set; } = 4;
        [HtmlAttributeName("Title")]
        public string Title { get; set; }

        private ModFaqRepository _repo = null;
        public FaqTagHelper(IWorkingContext<FaqTagHelper> workingContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor) : base(viewRenderService, httpContextAccessor)
        {
            _repo = new ModFaqRepository(context: workingContext);
        }
        private string PartialView = "/Views/Partial/FAQ/{0}.cshtml";
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (ViewLayout.IsEmpty() && DefaultLayout.IsNotEmpty()) { ViewLayout = DefaultLayout; }
            if (ViewLayout.IsEmpty())
            {
                output.Content.SetHtmlContent("");
                return;
            }

            var dbQuery = _repo.GetTable()
                            .Where(o => o.Activity == true && o.MenuID == MenuID);

            var model = await dbQuery.OrderByDescending(o => new { o.Order, o.ID })
                    .Take(PageSize)
                    .ToListAsync();


            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                ["Title"] = Title
            };

            string layout = PartialView.Format(ViewLayout);
            var html = await _viewRenderService.PartialAsync(layout, model, viewData);
            output.Content.SetHtmlContent(html);
        }
    }
}
