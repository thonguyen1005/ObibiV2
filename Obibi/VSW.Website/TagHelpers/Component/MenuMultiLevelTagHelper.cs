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
    [HtmlTargetElement("vsw-menu-multi")]
    public class MenuMultiTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("PageID")]
        public int PageID { get; set; }
        [HtmlAttributeName("State")]
        public int State { get; set; }
        [HtmlAttributeName("Title")]
        public string Title { get; set; }

        private SysPageRepository _repo = null;
        public MenuMultiTagHelper(IWorkingContext<MenuMultiTagHelper> workingContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor) : base(viewRenderService, httpContextAccessor)
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

            string sql = @"WITH MenuCTE AS (
                            SELECT 
                                [ID]
                              ,[Name]
                              ,[Code]
                              ,[Icon]
                              ,[File]
                              ,[Summary]
                              ,ParentID
                              ,MenuID                              
                              ,BrandID
							  ,[Order]
                              ,0 AS Level
                            FROM [Sys_Page] with (nolock)
                            WHERE ParentID = @PageID AND Activity = 1 AND LangID = @LangID";
            if (State > 0)
            {
                sql += " AND (State & " + State + ") = " + State;
            }

            sql += @" UNION ALL

                            SELECT m.[ID]
                              ,m.[Name]
                              ,m.[Code]
                              ,m.[Icon]
                              ,m.[File]
                              ,m.[Summary]
                              ,m.ParentID
                              ,m.MenuID                              
                              ,m.BrandID
                              ,m.[Order]
                              ,1 AS Level
                            FROM [Sys_Page] m with (nolock)
                            INNER JOIN MenuCTE cte ON m.[ParentID] = cte.id
                        )
                        SELECT * FROM MenuCTE";

            var model = await _repo.WithSqlText(sql).AddParameter("@PageID", PageID)
                                    .AddParameter("@LangID", CurrentSite.LangID)
                                    .QueryAsync<SysPageModel>();

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
