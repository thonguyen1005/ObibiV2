using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using VSW.Core;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Helpers;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.Controllers
{
    public class MNewsController : BaseController
    {
        public int MenuID { get; set; }
        public int PageSize { get; set; } = 20;

        private ModNewsRepository _repo = null;
        private WebMenuRepository _repoMenu = null;
        private ModAuthorRepository _authorRepository = null;
        private SysPageRepository _pageRepository = null;
        public MNewsController(IWorkingContext<MNewsController> context) : base(context)
        {
            _repo = new ModNewsRepository(context: context);
            _repoMenu = new WebMenuRepository(context: context);
            _authorRepository = new ModAuthorRepository(context: context);
            _pageRepository = new SysPageRepository(context: context);
        }
        public async Task<IActionResult> Index(NewsSearchModel searchModel)
        {
            if (CurrentPage.MenuID > 0)
                MenuID = CurrentPage.MenuID;

            searchModel.PageSize = PageSize;
            int Offset = 0;
            if (searchModel.Page > 0)
            {
                Offset = (searchModel.Page - 1) * searchModel.PageSize;
            }
            string sql = @"SELECT 
                                n.ID, n.Name, n.MenuID, n.State, n.Code, n.[File], n.[View], n.Published, n.[Order], n.Activity, n.Summary, n.AuthorID,
		                        a.Code as AuthorCode, a.Name as AuthorName,
                                COUNT(*) OVER() AS TotalCount
                            FROM Mod_News n with (nolock)
                            LEFT JOIN Mod_Author a with (nolock) ON n.AuthorID = a.ID
                        Where n.Activity = 1";

            List<DataParameter> lstParams = new List<DataParameter>();
            string where = "";
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
            sql += " OFFSET " + Offset + @" ROWS FETCH NEXT " + searchModel.PageSize + " ROWS ONLY";
            var model = await _repo.WithSqlText(sql).AddParameter(lstParams).QueryAsync<ModNewsModel>();

            searchModel.TotalRecord = model.IsNotEmpty() ? model[0].TotalCount : 0;

            ViewBag.Model = searchModel;
            return View(model);
        }

        public async Task<IActionResult> Detail([FromRoute] string id)
        {
            if (CurrentPage.MenuID > 0)
                MenuID = CurrentPage.MenuID;

            var item = await _repo.GetTable()
                                 .Where(o => o.Activity == true && o.Code == id)
                                 .SingleAsync();

            var model = item.MapTo<ModNewsModel>();
            if (item != null)
            {
                //up view
                item.View++;
                await _repo.UpdateColumnAsync(o => o.ID == item.ID, o => new MOD_NEWSEntity
                {
                    View = item.View,
                });

                if (model.AuthorID > 0)
                {
                    var author = await _authorRepository.GetAsync(model.AuthorID);
                    if (author != null)
                    {
                        model.AuthorCode = author.Code;
                        model.AuthorName = author.Name;
                        ViewBag.Author = author;
                    }
                }

                string sql = @"SELECT 
                                n.ID, n.Name, n.MenuID, n.State, n.Code, n.[File], n.[View], n.Published, n.[Order], n.Activity, n.Summary, n.AuthorID,
		                        a.Code as AuthorCode, a.Name as AuthorName
                            FROM Mod_News n with (nolock)
                            LEFT JOIN Mod_Author a with (nolock) ON n.AuthorID = a.ID
                        Where n.Activity = 1 And n.ID != " + item.ID;

                List<DataParameter> lstParams = new List<DataParameter>();
                string where = "";
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
                sql += " OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY";
                ViewBag.Other = await _repo.WithSqlText(sql).AddParameter(lstParams).QueryAsync<ModNewsModel>();

                sql = @"select
                        p.*
                        from Mod_NewsProduct np with (nolock)
                        join Mod_Product p with (nolock) on p.ID = np.ProductID
                        where np.NewsID = " + item.ID;

                ViewBag.Products = await _repo.WithSqlText(sql).QueryAsync<MOD_PRODUCTEntity>();

                var pageMenu = await _pageRepository.GetTable().Where(o => o.MenuID == item.MenuID && o.ModuleCode == "MNews" && o.Activity == true)
                                        .Select(o => new SYS_PAGEEntity { ID = o.ID, Code = o.Code, Name = o.Name }).FirstOrDefaultAsync();
                if (pageMenu != null) ViewBag.Menu = pageMenu;

                ViewData["Title"] = string.IsNullOrEmpty(item.PageTitle) ? item.Name : item.PageTitle;
                ViewData["Keyword"] = !string.IsNullOrEmpty(item.PageKeywords) ? item.PageKeywords : CurrentPage.PageKeywords;
                ViewData["Description"] = string.IsNullOrEmpty(item.PageDescription) ? item.Summary : item.PageDescription;

                ViewData["Image"] = Request.Scheme + "://" + Request.Host + ImageHelper.ResizeToWebp(item.File);
                ViewData["Image_Url"] = Request.Scheme + "://" + Request.Host + ImageHelper.ResizeToWebp(item.File);
            }
            ViewBag.Layout = "NewsDetail";
            return View(model);
        }
    }
}
