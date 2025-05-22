using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Global;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.Controllers
{
    public class MSearchController : BaseController
    {
        public int PageSize { get; set; } = 20;
        private ModProductRepository _repo = null;
        public MSearchController(IWorkingContext<MSearchController> context) : base(context)
        {
            _repo = new ModProductRepository(context: context);
        }
        public async Task<IActionResult> Index(MSearchModel searchModel)
        {
            string keyword = !string.IsNullOrEmpty(searchModel.keyword) ? Data.GetCode(searchModel.keyword) : string.Empty;
            searchModel.PageSize = PageSize;
            var dbQuery = _repo.GetTable().Where(o => o.Activity == true)
                                          .Where(!string.IsNullOrEmpty(keyword), o => o.Code.Contains(keyword) || o.Model.Contains(keyword));

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
                    .ToPagingAsync<ModProductModel>(searchModel.Page, searchModel.PageSize);

            searchModel.TotalRecord = model.TotalCount;
            ViewBag.Model = searchModel;
            return View(model);
        }
    }
}
