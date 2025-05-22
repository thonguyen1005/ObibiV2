using System.Linq;
using VSW.Lib.Global;
using VSW.Lib.Models;

namespace VSW.Lib.CPControllers
{
    public class FormProductVideoController : ModVideoController
    {
        public override void ActionIndex(ModVideoModel model)
        {
            // sap xep tu dong
            var orderBy = AutoSort(model.Sort);

            // tao danh sach
            var dbQuery = ModVideoService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(model.SearchText)))
                                .Where(model.State > 0, o => (o.State & model.State) == model.State)
                                .WhereIn(o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("Video", model.MenuID, model.LangID))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            var list = dbQuery.ToList();
            if (model.Value > 0)
            {
                var listOther = ModProductVideoService.Instance.GetAll(model.Value);
                if(listOther != null)
                {
                    list.ForEach(
                        x=>
                        {
                            x.Check = (listOther.Where(o => o.VideoID == x.ID).Count() > 0 ? true : false);
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