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
    public class MAuthorController : BaseController
    {
        private ModAuthorRepository _authorRepository = null;
        private ModNewsRepository _newRepository = null;
        public MAuthorController(IWorkingContext<MAuthorController> context) : base(context)
        {
            _authorRepository = new ModAuthorRepository(context: context);
            _newRepository = new ModNewsRepository(context: context);
        }
        public async Task<IActionResult> Detail([FromRoute] string id, NewsSearchModel searchModel)
        {
            var author = await _authorRepository.GetTable().Where(o => o.Code == id).FirstOrDefaultAsync();
            if (author != null)
            {
                searchModel.PageSize = 20;
                int Offset = 0;
                if (searchModel.Page > 0)
                {
                    Offset = (searchModel.Page - 1) * searchModel.PageSize;
                }
                string sql = @"SELECT 
                                n.ID, n.Name, n.MenuID, n.State, n.Code, n.[File], n.[View], n.Published, n.[Order], n.Activity, n.Summary, n.AuthorID,
                                COUNT(*) OVER() AS TotalCount
                            FROM Mod_News n with (nolock)
                            Where n.Activity = 1 And n.AuthorID = " + author.ID;

                sql += " ORDER BY n.[Order] DESC, n.ID DESC";
                sql += " OFFSET " + Offset + @" ROWS FETCH NEXT " + searchModel.PageSize + " ROWS ONLY";
                var model = await _newRepository.WithSqlText(sql).QueryAsync<ModNewsModel>();

                searchModel.TotalRecord = model.IsNotEmpty() ? model[0].TotalCount : 0;
                ViewBag.Model = searchModel;
                ViewBag.News = model;
            }
            ViewBag.Tag = id;
            return View(author);
        }
    }
}
