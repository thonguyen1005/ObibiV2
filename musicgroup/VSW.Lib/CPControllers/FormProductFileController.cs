using System.Linq;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class FormProductFileController : CPController
    {
        public FormProductFileController()
        {
            //khoi tao Service
            DataService = ModProductFileService.Instance;
            DataEntity = new ModProductFileEntity();
        }
        public void ActionIndex(ModProductFileModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort, "[Order] Asc");

            // tao danh sach
            var dbQuery = ModProductFileService.Instance.CreateQuery()
                                .Where(o => o.ProductID == model.ProductID)
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }
    }
    public class ModProductFileModel : DefaultModel
    {
        public int ProductID { get; set; }
    }
}