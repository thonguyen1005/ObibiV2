using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.DataBase.Repositories;

namespace VSW.Website.TagHelpers
{
    public class BreadcrumbViewComponent : ViewComponent
    {
        private SysPageRepository _repo;

        public BreadcrumbViewComponent(IWorkingContext<BreadcrumbViewComponent> context)
        {
            _repo = new SysPageRepository(context: context);
        }

        public async Task<IViewComponentResult> InvokeAsync(int pageID, string layout)
        {
            var model = await _repo.WithSqlText(@"WITH ParentTree AS (
                                SELECT ID, Code, Name, ParentID
                                FROM Sys_Page
                                WHERE ID = @pageID

                                UNION ALL

                                SELECT c.ID, c.Code, c.Name, c.ParentID
                                FROM Sys_Page c
                                INNER JOIN ParentTree pt ON c.ID = pt.ParentID
                                WHERE c.ParentID <> 0
                            )
                            SELECT * FROM ParentTree Order by ParentID").AddParameter("@pageID", pageID).QueryAsync<SysPageModel>();

            return View("~/Views/Partial/CBreadcrumb/" + layout + ".cshtml", model);
        }
    }
}
