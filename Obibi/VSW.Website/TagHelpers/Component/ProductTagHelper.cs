using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using VSW.Core;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Interface;

namespace VSW.Website.TagHelpers
{
    [HtmlTargetElement("vsw-product")]
    public class ProductTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("MenuID")]
        public int MenuID { get; set; }
        [HtmlAttributeName("PageID")]
        public int PageID { get; set; }
        [HtmlAttributeName("State")]
        public int State { get; set; }
        [HtmlAttributeName("PageSize")]
        public int PageSize { get; set; } = 5;
        [HtmlAttributeName("Title")]
        public string Title { get; set; }

        private ModProductRepository _repo = null;
        private SysPageRepository _repoPage = null;
        private WebMenuRepository _repoMenu = null;
        public ProductTagHelper(IWorkingContext<ProductTagHelper> workingContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor) : base(viewRenderService, httpContextAccessor)
        {
            _repo = new ModProductRepository(context: workingContext);
            _repoPage = new SysPageRepository(context: workingContext);
            _repoMenu = new WebMenuRepository(context: workingContext);
        }
        private string PartialView = "/Views/Partial/CProduct/{0}.cshtml";
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (ViewLayout.IsEmpty() && DefaultLayout.IsNotEmpty()) { ViewLayout = DefaultLayout; }
            if (ViewLayout.IsEmpty())
            {
                output.Content.SetHtmlContent("");
                return;
            }
            var page = _repoPage.Get(PageID);

            var dbQuery = _repo.GetTable().Where(o => o.Activity == true)
                                          .Where(State > 0, o => (o.State & State) == State);
            if (MenuID > 0)
            {
                var lstMenuId = _repoMenu.GetChildIDForWeb_Cache("Product", MenuID, CurrentSite.LangID);
                if (lstMenuId.IsNotEmpty())
                {
                    dbQuery = dbQuery.Where(o => lstMenuId.Contains(o.MenuID));
                }
            }
            var model = await dbQuery.OrderByDescending(o => new { o.Order, o.ID })
                    .Take(PageSize)
                    .Select(o => new ModProductModel
                    {
                        ID = o.ID,
                        Name = o.Name,
                        PhienBan = o.PhienBan,
                        MenuID = o.MenuID,
                        BrandID = o.BrandID,
                        State = o.State,
                        Code = o.Code,
                        Model = o.Model,
                        File = o.File,
                        Price = o.Price,
                        Price2 = o.Price2,
                        View = o.View,
                        Published = o.Published,
                        Order = o.Order,
                        Activity = o.Activity
                    })
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
