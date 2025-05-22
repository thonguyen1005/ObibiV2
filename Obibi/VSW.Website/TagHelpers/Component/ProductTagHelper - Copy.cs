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
using VSW.Website.DataBase.Repositories;
using VSW.Website.Interface;

namespace VSW.Website.TagHelpers.Component
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

            string sql = @"WITH Buyed AS (
                                SELECT ProductID, SUM(Quantity) AS TotalBuy
                                FROM Mod_OrderDetail with (nolock)
                                GROUP BY ProductID
                            ),
                            Commented AS (
                                SELECT ProductID, COUNT(*) AS CountComment
                                FROM Mod_Vote with (nolock)
                                GROUP BY ProductID
                            )
                            SELECT 
                                p.ID, p.Name, p.PhienBan, p.MenuID, p.BrandID, p.State, p.Code, p.Model, p.[File], p.Price, p.Price2, p.[View], p.Published, p.[Order], p.Activity, p.DatePromotion, p.PricePromotion, p.HasProperty,
								COALESCE(s.TotalBuy, 0) AS TotalBuy,
                                COALESCE(c.CountComment, 0) AS CountComment
                            FROM Mod_Product p with (nolock)
                            LEFT JOIN Buyed s ON s.ProductID = p.ID
                            LEFT JOIN Commented c ON c.ProductID = p.ID
                        Where p.Activity = 1";

            List<DataParameter> lstParams = new List<DataParameter>();  
            string where = "";
            if(State > 0)
            {
                where += " And (p.State & @State) = @State";
                lstParams.Add(new DataParameter("@State", State));
            }
            if (MenuID > 0)
            {
                var lstMenuId = _repoMenu.GetChildIDForWeb_Cache("Product", MenuID, CurrentSite.LangID);
                if (lstMenuId.IsNotEmpty())
                {
                    where += " And p.MenuID in (" + string.Join(",", lstMenuId) + @")";
                }
            }
            if (where.IsNotEmpty()) sql += where;
            sql += " ORDER BY p.[Order] DESC, p.ID DESC";
            sql += " OFFSET 0 ROWS FETCH NEXT "+ PageSize + " ROWS ONLY";
            var model = await _repo.WithSqlText(sql).AddParameter(lstParams).QueryAsync<MOD_PRODUCTEntity>();
            
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
