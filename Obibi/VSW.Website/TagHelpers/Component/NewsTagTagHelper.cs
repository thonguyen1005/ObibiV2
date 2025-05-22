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
    [HtmlTargetElement("vsw-newstag")]
    public class NewsTagTagHelper : BaseTagHelper
    {

        private ModTagRepository _repo = null;
        public NewsTagTagHelper(IWorkingContext<NewsTagTagHelper> workingContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor) : base(viewRenderService, httpContextAccessor)
        {
            _repo = new ModTagRepository(context: workingContext);
        }
        private string PartialView = "/Views/Partial/CNewsTag/{0}.cshtml";
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (ViewLayout.IsEmpty() && DefaultLayout.IsNotEmpty()) { ViewLayout = DefaultLayout; }
            if (ViewLayout.IsEmpty())
            {
                output.Content.SetHtmlContent("");
                return;
            }
            string sql = @"select top 10 b.* from Mod_NewsTag a with (nolock)
                                    Inner Join Mod_Tag b with (nolock) on a.TagID = b.ID order by b.[View] desc";

            var model = await _repo.WithSqlText(sql).QueryAsync<MOD_TAGEntity>();

            string layout = PartialView.Format(ViewLayout);
            var html = await _viewRenderService.PartialAsync(layout, model);
            output.Content.SetHtmlContent(html);
        }
    }
}
