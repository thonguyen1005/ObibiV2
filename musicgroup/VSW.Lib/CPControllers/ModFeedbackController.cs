using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Liên hệ",
        Description = "Quản lý - Liên hệ",
        Code = "ModFeedback",
        Access = 9,
        Order = 11,
        ShowInMenu = true,
        CssClass = "newspaper-o")]
    public class ModFeedbackController : CPController
    {
        public ModFeedbackController()
        {
            //khoi tao Service
            DataService = ModFeedbackService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModFeedbackModel model)
        {
            //sap xep tu dong
            string orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = ModFeedbackService.Instance.CreateQuery()
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModFeedbackModel model)
        {
            _item = model.RecordID > 0 ? ModFeedbackService.Instance.GetByID(model.RecordID) : new ModFeedbackEntity();

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        #region private func

        private ModFeedbackEntity _item = null;

        #endregion private func
    }

    public class ModFeedbackModel : DefaultModel
    {
        public int LangID { get; set; } = 1;
    }
}