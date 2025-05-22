using LinqToDB;
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
    [HtmlTargetElement("vsw-product-by-id")]
    public class ProductByIdTagHelper : TagHelper
    {
        [HtmlAttributeName("id")]
        public int Id { get; set; }

        [HtmlAttributeName("list-id")]
        public List<int> ListId { get; set; }

        [HtmlAttributeName("layout")]
        public string ViewLayout { get; set; }

        private IViewRenderService _viewRenderService = null;
        private ModProductRepository _repo = null;
        public ProductByIdTagHelper(IWorkingContext<ProductByIdTagHelper> workingContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor)
        {
            _viewRenderService = viewRenderService;
            _repo = new ModProductRepository(context: workingContext);
        }
        private string PartialView = "/Views/Partial/CProductById/{0}.cshtml";
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            if (ViewLayout.IsEmpty())
            {
                output.Content.SetHtmlContent("");
                return;
            }
            var model = new List<ModProductModel>();
            if (ListId.IsNotEmpty())
            {
                model = await _repo.GetTable()
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
                    .Where(o => o.Activity == true).Where(o => ListId.Contains(o.ID)).ToListAsync();
            }
            else if (Id > 0)
            {
                model = await _repo.GetTable()
                    .Where(o => o.ID == Id)
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
                    }).ToListAsync();
            }

            string layout = PartialView.Format(ViewLayout);
            var html = await _viewRenderService.PartialAsync(layout, model);
            output.Content.SetHtmlContent(html);
        }
    }
}
