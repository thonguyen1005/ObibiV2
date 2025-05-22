using System.Linq;
using VSW.Lib.Global;
using VSW.Lib.Models;

namespace VSW.Lib.CPControllers
{
    public class FormProductFAQController : ModFAQController
    {
        public override void ActionIndex(ModFAQModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort);

            // tao danh sach
            var dbQuery = ModFAQService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => o.Name.Contains(model.SearchText))
                                .WhereIn(o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("FAQ", model.MenuID, model.LangID))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            var list = dbQuery.ToList();
            if (model.Value > 0)
            {
                var listOther = ModProductFAQService.Instance.GetAll(model.Value);
                if (listOther != null)
                {
                    list.ForEach(
                        x =>
                        {
                            x.Check = (listOther.Where(o => o.FAQID == x.ID).Count() > 0 ? true : false);
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