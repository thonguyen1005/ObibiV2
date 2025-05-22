using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.Controllers
{
    public class MNewsTagController : BaseController
    {
        private ModTagRepository _tagRepository = null;
        private ModNewsTagRepository _newstagRepository = null;
        private ModNewsRepository _newsRepository = null;
        public MNewsTagController(IWorkingContext<MNewsTagController> context) : base(context)
        {
            _tagRepository = new ModTagRepository(context: context);
            _newstagRepository = new ModNewsTagRepository(context: context);
            _newsRepository = new ModNewsRepository(context: context);
        }
        public async Task<IActionResult> Detail([FromRoute] string id, NewsSearchModel searchModel)
        {
            var tagID = await _tagRepository.GetTable().Where(o => o.Code == id).Select(o => o.ID).FirstOrDefaultAsync(); ;

            if (tagID > 0)
            {
                searchModel.PageSize = 20;
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
                            Join Mod_NewsTag t with (nolock) on t.NewsID = n.ID
                            LEFT JOIN Mod_Author a ON n.AuthorID = a.ID
                            Where n.Activity = 1 and t.TagID = " + tagID;

                sql += " ORDER BY n.[Order] DESC, n.ID DESC";
                sql += " OFFSET " + Offset + @" ROWS FETCH NEXT " + searchModel.PageSize + " ROWS ONLY";
                var model = await _newsRepository.WithSqlText(sql).QueryAsync<ModNewsModel>();

                searchModel.TotalRecord = model.IsNotEmpty() ? model[0].TotalCount : 0;

                ViewBag.Model = searchModel;
                ViewBag.Tag = id;
                return View(model);
            }

            return Redirect("/");
        }
    }
}
