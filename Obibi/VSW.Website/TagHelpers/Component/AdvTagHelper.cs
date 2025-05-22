using LinqToDB;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using VSW.Core;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Interface;

namespace VSW.Website.TagHelpers
{
    [HtmlTargetElement("vsw-adv")]
    public class AdvTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("MenuID")]
        public int MenuID { get; set; }
        [HtmlAttributeName("MultiRecord")]
        public bool MultiRecord { get; set; } = true;
        [HtmlAttributeName("Title")]
        public string Title { get; set; }

        private ModAdvRepository _repo = null;
        public AdvTagHelper(IWorkingContext<AdvTagHelper> workingContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor) : base(viewRenderService, httpContextAccessor)
        {
            _repo = new ModAdvRepository(context: workingContext);
        }
        private string PartialView = "/Views/Partial/CAdv/{0}.cshtml";
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
                                        .Where(o => o.Activity == true && o.MenuID == MenuID)
                                        .OrderByDescending(o => new { o.Order, o.ID });


            var model = new List<MOD_ADVEntity>();
            if (!MultiRecord)
            {
                model = await dbQuery.Take(1).ToListAsync();
            }
            else
            {
                model = await dbQuery.ToListAsync();
            }

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
