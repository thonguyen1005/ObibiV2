using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Tác giả",
        Description = "Quản lý - Tác giả",
        Code = "ModAuthor",
        Access = 31,
        Order = 5,
        ShowInMenu = true,
        CssClass = "user")]
    public class ModAuthorController : CPController
    {
        public ModAuthorController()
        {
            //khoi tao Service
            DataService = ModAuthorService.Instance;
            DataEntity = new ModAuthorEntity();
            CheckPermissions = true;
        }

        public void ActionIndex(ModAuthorModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = ModAuthorService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(Data.GetCode(model.SearchText))))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModAuthorModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModAuthorService.Instance.GetByID(model.RecordID);
            }
            else
            {
                _item = new ModAuthorEntity
                {
                };
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModAuthorModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModAuthorModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModAuthorModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }
        public override void ActionDelete(int[] arrID)
        {
            //xoa cleanurl
            ModCleanURLService.Instance.Delete("[Value] IN (" + Core.Global.Array.ToString(arrID) + ") AND [Type]='Author'");

            //xoa Author
            DataService.Delete("[ID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private ModAuthorEntity _item;

        private bool ValidSave(ModAuthorModel model)
        {
            TryUpdateModel(_item);

            //chong hack
            _item.ID = model.RecordID;

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            //kiem tra ten
            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tên.");

            if (ModCleanURLService.Instance.CheckCode(_item.Code, model.LangID, _item.ID))
                CPViewPage.Message.ListMessage.Add("Mã đã tồn tại. Vui lòng chọn mã khác.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            if (string.IsNullOrEmpty(_item.Code)) _item.Code = Data.GetCode(_item.Name);

            try
            {
                //save
                ModAuthorService.Instance.Save(_item);

                //update url
                ModCleanURLService.Instance.InsertOrUpdate(_item.Code, "Author", _item.ID, 0, model.LangID);
            }
            catch (Exception ex)
            {
                Error.Write(ex);
                CPViewPage.Message.ListMessage.Add(ex.Message);
                return false;
            }

            return true;
        }

        #endregion private func
    }

    public class ModAuthorModel : DefaultModel
    {
        public int LangID { get; set; } = 1;
        public string SearchText { get; set; }
    }
}