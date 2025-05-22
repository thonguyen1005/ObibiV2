using System;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysTemplateController : CPController
    {
        public SysTemplateController()
        {
            //khoi tao Service
            DataService = SysTemplateService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysTemplateModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort, "[Order]");

            //tao danh sach
            var dbQuery = SysTemplateService.Instance.CreateQuery()
                                .Where(o => o.LangID == model.LangID)
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(SysTemplateModel model)
        {
            _item = model.RecordID > 0 ? SysTemplateService.Instance.GetByID(model.RecordID) : new SysTemplateEntity { LangID = model.LangID, Order = GetMaxOrder(model) };

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysTemplateModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysTemplateModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysTemplateModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        #region private func

        private SysTemplateEntity _item;

        private bool ValidSave(SysTemplateModel model)
        {
            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra ten
            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tên mẫu giao diện.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            try
            {
                //save
                SysTemplateService.Instance.Save(_item);
            }
            catch (Exception ex)
            {
                Error.Write(ex);
                CPViewPage.Message.ListMessage.Add(ex.Message);
                return false;
            }

            return true;
        }

        private static int GetMaxOrder(SysTemplateModel model)
        {
            return SysTemplateService.Instance.CreateQuery()
                    .Where(o => o.LangID == model.LangID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    public class SysTemplateModel : DefaultModel
    {
        private int _langID = 1;

        public int LangID
        {
            get => _langID;
            set => _langID = value;
        }
    }
}