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
    [HtmlTargetElement("vsw-news")]
    public class NewsTagHelper : BaseTagHelper
    {
        [HtmlAttributeName("MenuID")]
        public int MenuID { get; set; }
        [HtmlAttributeName("PageID")]
        public int PageID { get; set; }
        [HtmlAttributeName("State")]
        public int State { get; set; }
        [HtmlAttributeName("PageSize")]
        public int PageSize { get; set; } = 4;
        [HtmlAttributeName("Title")]
        public string Title { get; set; }

        private ModNewsRepository _repo = null;
        private SysPageRepository _repoPage = null;
        private WebMenuRepository _repoMenu = null;
        public NewsTagHelper(IWorkingContext<NewsTagHelper> workingContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor) : base(viewRenderService, httpContextAccessor)
        {
            _repo = new ModNewsRepository(context: workingContext);
            _repoPage = new SysPageRepository(context: workingContext);
            _repoMenu = new WebMenuRepository(context: workingContext);
        }
        private string PartialView = "/Views/Partial/CNews/{0}.cshtml";
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

            string sql = @"SELECT 
                                n.ID, n.Name, n.MenuID, n.State, n.Code, n.[File], n.[View], n.Published, n.[Order], n.Activity, n.Summary, n.AuthorID,
		                        a.Code as AuthorCode, a.Name as AuthorName
                            FROM Mod_News n with (nolock)
                            LEFT JOIN Mod_Author a with (nolock) ON n.AuthorID = a.ID
                        Where n.Activity = 1";

            List<DataParameter> lstParams = new List<DataParameter>();
            string where = "";
            if (State > 0)
            {
                where += " And (n.State & @State) = @State";
                lstParams.Add(new DataParameter("@State", State));
            }
            if (MenuID > 0)
            {
                var lstMenuId = _repoMenu.GetChildIDForWeb_Cache("News", MenuID, CurrentSite.LangID);
                if (lstMenuId.IsNotEmpty())
                {
                    where += " And n.MenuID in (" + string.Join(",", lstMenuId) + @")";
                }
            }
            if (where.IsNotEmpty()) sql += where;
            sql += " ORDER BY n.[Order] DESC, n.ID DESC";
            sql += " OFFSET 0 ROWS FETCH NEXT " + PageSize + " ROWS ONLY";
            var model = await _repo.WithSqlText(sql).AddParameter(lstParams).QueryAsync<ModNewsModel>();

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
