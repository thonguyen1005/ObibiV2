using System;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysSiteController : CPController
    {
        public SysSiteController()
        {
            //khoi tao Service
            DataService = SysSiteService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysSiteModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort, "[Order]");

            //tao danh sach
            var dbQuery = SysSiteService.Instance.CreateQuery()
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(SysSiteModel model)
        {
            _item = model.RecordID > 0 ? SysSiteService.Instance.GetByID(model.RecordID) : new SysSiteEntity { Order = GetMaxOrder() };

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysSiteModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysSiteModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysSiteModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public void ActionDefaultGX(int id)
        {
            //update for id
            SysSiteService.Instance.Update(o => o.ID == id, "@Default", 1);

            //update for != id
            SysSiteService.Instance.Update(o => o.ID != id, "@Default", 0);

            Core.Web.Cache.Clear(DataService);

            //thong bao
            CPViewPage.SetMessage("Đã thực hiện thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private SysSiteEntity _item;

        private bool ValidSave(SysSiteModel model)
        {
            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra ten
            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tên site.");

            //kiem tra ma
            if (_item.Code.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập mã.");

            if (CPViewPage.Message.ListMessage.Count == 0)
            {
                try
                {
                    //save
                    SysSiteService.Instance.Save(_item);
                }
                catch (Exception ex)
                {
                    Error.Write(ex);
                    CPViewPage.Message.ListMessage.Add(ex.Message);
                    return false;
                }

                return true;
            }

            return false;
        }

        private static int GetMaxOrder()
        {
            return SysSiteService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    public class SysSiteModel : DefaultModel
    {
    }
}