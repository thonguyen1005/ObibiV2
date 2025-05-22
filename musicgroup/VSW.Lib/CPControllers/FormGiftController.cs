using System.Linq;
using VSW.Lib.Global;
using VSW.Lib.Models;

namespace VSW.Lib.CPControllers
{
    public class FormGiftController : ModGiftController
    {
        public override void ActionIndex(ModGiftModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = ModGiftService.Instance.CreateQuery()
                                    .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(model.SearchText)))
                                    .WhereIn(model.MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("Gift", model.MenuID, model.LangID))
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);

            
            var list = dbQuery.ToList_Cache();
            if (model.Value > 0)
            {
                var listOther = ModProductGiftService.Instance.GetAll(model.Value);
                if (listOther != null)
                {
                    list.ForEach(
                        x =>
                        {
                            x.Check = (listOther.Where(o => o.GiftID == x.ID).Count() > 0 ? true : false);
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