using VSW.Lib.MVC;
using VSW.Lib.Models;
using System.Collections.Generic;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "ĐK: Thuộc tính", Code = "CProperty", IsControl = true, Order = 2)]
    public class CPropertyController : Controller
    {
        Dictionary<WebPropertyEntity, List<WebPropertyEntity>> dicItem;
        public override void OnLoad()
        {
            var menu = WebMenuService.Instance.GetByID_Cache(ViewPage.CurrentPage.MenuID);
            if (menu == null || menu.PropertyID < 1)
                return;

            var listItem = WebPropertyService.Instance.CreateQuery()
                                                .Select(o => new { o.Name, o.Code, o.ID })
                                                .Where(o => o.Activity == true && o.ParentID == menu.PropertyID)
                                                .OrderByAsc(o => o.Order)
                                                .ToList_Cache();

            if (listItem == null)
                return;

            dicItem = new Dictionary<WebPropertyEntity, List<WebPropertyEntity>>();

            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var listChildItem = WebPropertyService.Instance.CreateQuery()
                                                    .Select(o => new { o.ID, o.Name, o.Code })
                                                    .Where(o => o.Activity == true && o.ParentID == listItem[i].ID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList_Cache();

                if (listChildItem == null)
                    continue;

                for (var j = 0; j < listChildItem.Count; j++)
                {
                    listChildItem[j].Count = ModPropertyService.Instance.GetCount(listChildItem[j].ID, ViewPage.CurrentPage.MenuID);
                }

                listChildItem.RemoveAll(o => o.Count < 1);

                dicItem[listItem[i]] = listChildItem;
            }

            ViewBag.Data = dicItem;
        }

        private static List<WebPropertyEntity> Remove(List<WebPropertyEntity> list, WebPropertyEntity item)
        {
            var listItem = new List<WebPropertyEntity>(list.Count - 1);
            foreach (var o in list)
            {
                if (o.ID != item.ID)
                    listItem.Add(o);
            }

            return listItem;
        }
    }
}
