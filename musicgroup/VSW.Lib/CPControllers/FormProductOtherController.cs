using System.Linq;
using VSW.Lib.Global;
using VSW.Lib.Models;

namespace VSW.Lib.CPControllers
{
    public class FormProductOtherController : ModProductController
    {
        public override void ActionIndex(ModProductModel model)
        {
            var orderBy = AutoSort(model.Sort, "[Order] DESC");
            string menulist = WebMenuService.Instance.GetChildIDForWeb_Cache("Product", model.MenuID, model.LangID);
            string wherein = "";
            if (!string.IsNullOrEmpty(menulist))
            {
                string[] arrmenu = menulist.Split(',');
                for (int i = 0; arrmenu != null && i < arrmenu.Length; i++)
                {
                    if (string.IsNullOrEmpty(arrmenu[i].Trim())) continue;
                    wherein += (!string.IsNullOrEmpty(wherein) ? " or " : "") + " CHARINDEX('" + arrmenu[i].Trim() + "', [MenuListID]) > 0 ";
                }
            }
            //tao danh sach
            var dbQuery = ModProductService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion })
                                    .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(Data.GetCode(model.SearchText)) || o.Model.Contains(Data.GetCode(model.SearchText)) || o.Model.Contains(model.SearchText)))
                                    .Where(model.State > 0, o => (o.State & model.State) == model.State)
                                    .Where(model.MenuID > 0, wherein)
                                    .WhereIn(model.BrandID > 0, o => o.BrandID, WebMenuService.Instance.GetChildIDForCP("Brand", model.BrandID, model.LangID))
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);
            var list = dbQuery.ToList_Cache();
            if (model.Value > 0)
            {
                var listOther = ModProductOtherService.Instance.GetAll(model.Value);
                if(listOther != null)
                {
                    list.ForEach(
                        x=>
                        {
                            x.Check = (listOther.Where(o => o.ProductOtherID == x.ID).Count() > 0 ? true : false);
                        }
                    );
                }
            }

            ViewBag.Data = list;
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }
    }
}