using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Repositories;

namespace VSW.Website.TagHelpers
{
    public class MenuViewComponent : ViewComponent
    {
        private SysPageRepository _repo;

        public MenuViewComponent(IWorkingContext<MenuViewComponent> context)
        {
            _repo = new SysPageRepository(context: context);
        }

        public async Task<IViewComponentResult> InvokeAsync(SYS_PAGEEntity page, string layout, int state = 0)
        {
            var model = await _repo.GetTable().Where(o => o.ParentID == page.ID && o.Activity == true).Where(state > 0, o => (o.State & state) == state).OrderBy(m => m.Order).ToListWithCacheAsync();

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                ["Page"] = page
            };

            return View("~/Views/Partial/CMenu/" + layout + ".cshtml", model);
        }
    }
}
