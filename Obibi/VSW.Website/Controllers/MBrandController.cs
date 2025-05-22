using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.Controllers
{
    public class MBrandController : BaseController
    {
        public int BrandID { get; set; }
        public int State { get; set; }
        public int PageSize { get; set; } = 20;
        private ModProductRepository _repo = null;
        public MBrandController(IWorkingContext<MBrandController> context) : base(context)
        {
            _repo = new ModProductRepository(context: context);
        }
        public async Task<IActionResult> Index(ProductSearchModel searchModel)
        {
            searchModel.PageSize = PageSize;
            var dbQuery = _repo.GetTable().Where(o => o.Activity == true)
                                          .Where(State > 0, o => (o.State & State) == State)
                                          .Where(o => o.BrandID == BrandID);

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

            ViewBag.Model = searchModel;
            return View(model);
        }
    }
}
