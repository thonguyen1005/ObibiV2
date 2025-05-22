using VSW.Lib.Models;

namespace VSW.Lib.CPControllers
{
    public class FormProductGroupController : ModProductController
    {
        public override void ActionIndex(ModProductModel model)
        {
            if (model.GroupMenuID > 0)
            {
                var dbQuery = ModProductService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion })
                                    .Where(o => o.GroupMenuID == model.GroupMenuID)
                                    .Take(model.PageSize)
                                    .OrderByAsc(o => o.PhienBan)
                                    .Skip(model.PageIndex * model.PageSize);
                var list = dbQuery.ToList_Cache();
                ViewBag.Data = list;
                model.TotalRecord = dbQuery.TotalRecord;
            }
            ViewBag.Model = model;
        }
    }
}