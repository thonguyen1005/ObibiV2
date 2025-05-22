using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Công trình",
        Description = "Quản lý - Công trình",
        Code = "ModCongTrinh",
        Access = 31,
        Order = 5,
        ShowInMenu = true,
        CssClass = "check-square-o")]
    public class ModCongTrinhController : CPController
    {
        public ModCongTrinhController()
        {
            //khoi tao Service
            DataService = ModCongTrinhService.Instance;
            DataEntity = new ModCongTrinhEntity();
            CheckPermissions = true;
        }

        public void ActionIndex(ModCongTrinhModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = ModCongTrinhService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(Data.GetCode(model.SearchText))))
                                    .Where(model.State > 0, o => (o.State & model.State) == model.State)
                                .WhereIn(model.MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("CongTrinh", model.MenuID, model.LangID))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModCongTrinhModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModCongTrinhService.Instance.GetByID(model.RecordID);

                //khoi tao gia tri mac dinh khi update
                if (_item.Updated <= DateTime.MinValue) _item.Updated = DateTime.Now;
            }
            else
            {
                _item = new ModCongTrinhEntity
                {
                    MenuID = model.MenuID,
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxOrder(),
                    Activity = CPViewPage.UserPermissions.Approve
                };

                //khoi tao gia tri mac dinh khi insert
                var json = Cookies.GetValue(DataService.ToString(), true);
                if (!string.IsNullOrEmpty(json))
                    _item = new JavaScriptSerializer().Deserialize<ModCongTrinhEntity>(json);
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModCongTrinhModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModCongTrinhModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModCongTrinhModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }
        public virtual void ActionPublishFeedback(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")", "@HasFeedback", 1);

            //thong bao
            CPViewPage.SetMessage("Đã duyệt thành công.");
            CPViewPage.RefreshPage();
        }
        public override void ActionDelete(int[] arrID)
        {
            //xoa cleanurl
            ModCleanURLService.Instance.Delete("[Value] IN (" + Core.Global.Array.ToString(arrID) + ") AND [Type]='CongTrinh'");

            //xoa CongTrinh
            DataService.Delete("[ID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private ModCongTrinhEntity _item;

        private bool ValidSave(ModCongTrinhModel model)
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
                CPViewPage.Message.ListMessage.Add("Nhập tiêu đề.");

            if (ModCleanURLService.Instance.CheckCode(_item.Code, "CongTrinh", _item.ID, model.LangID))
                CPViewPage.Message.ListMessage.Add("Mã đã tồn tại. Vui lòng chọn mã khác.");

            //kiem tra chuyen muc
            if (_item.MenuID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn chuyên mục.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            if (string.IsNullOrEmpty(_item.Code)) _item.Code = Data.GetCode(_item.Name);

            //cap nhat state
            _item.State = GetState(model.ArrState);

            try
            {
                //save
                ModCongTrinhService.Instance.Save(_item);

                //update url
                ModCleanURLService.Instance.InsertOrUpdate(_item.Code, "CongTrinh", _item.ID, _item.MenuID, model.LangID);
            }
            catch (Exception ex)
            {
                Error.Write(ex);
                CPViewPage.Message.ListMessage.Add(ex.Message);
                return false;
            }

            return true;
        }

        private static int GetMaxOrder()
        {
            return ModCongTrinhService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    public class ModCongTrinhModel : DefaultModel
    {
        public int LangID { get; set; } = 1;

        public int MenuID { get; set; }
        public int State { get; set; }
        public string SearchText { get; set; }

        public int[] ArrState { get; set; }
    }
}