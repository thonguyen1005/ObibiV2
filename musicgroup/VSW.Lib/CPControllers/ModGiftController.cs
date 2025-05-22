using System;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Quà tặng đi kèm",
        Description = "Quản lý - Quà tặng đi kèm",
        Code = "ModGift",
        Access = 31,
        Order = 1,
        ShowInMenu = true,
        CssClass = "gift")]
    public class ModGiftController : CPController
    {
        public ModGiftController()
        {
            //khoi tao Service
            DataService = ModGiftService.Instance;
            DataEntity = new ModGiftEntity();

            CheckPermissions = true;
        }

        public virtual void ActionIndex(ModGiftModel model)
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

            ViewBag.Data = dbQuery.ToList_Cache();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;

            //send to client
            //VSW.Core.SignalR.HubData<ModGiftEntity>.SendData(ViewBag.Data);
        }

        public void ActionAdd(ModGiftModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModGiftService.Instance.GetByID(model.RecordID);

                //khoi tao gia tri mac dinh khi update
                if (_item.Updated <= DateTime.MinValue) _item.Updated = DateTime.Now;
            }
            else
            {
                //khoi tao gia tri mac dinh khi insert
                _item = new ModGiftEntity
                {
                    MenuID = model.MenuID,
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxOrder(),
                    Activity = CPViewPage.UserPermissions.Approve
                };

                //var json = Cookies.GetValue(DataService.ToString(), true);
                //if (!string.IsNullOrEmpty(json))
                //    _item = new JavaScriptSerializer().Deserialize<ModGiftEntity>(json);
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModGiftModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModGiftModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModGiftModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            //xoa cleanurl
            ModCleanURLService.Instance.Delete("[Value] IN (" + Core.Global.Array.ToString(arrID) + ") AND [Type]='Gift'");

            //xoa Gift
            DataService.Delete("[ID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private ModGiftEntity _item;

        private bool ValidSave(ModGiftModel model)
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

            if (ModCleanURLService.Instance.CheckCode(_item.Code, "Gift", _item.ID, model.LangID))
                CPViewPage.Message.ListMessage.Add("Mã đã tồn tại. Vui lòng chọn mã khác.");

            //kiem tra chuyen muc
            if (_item.MenuID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn chuyên mục.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            if (string.IsNullOrEmpty(_item.Code)) _item.Code = Data.GetCode(_item.Name);

            try
            {
                //save
                ModGiftService.Instance.Save(_item);

                //update url
                ModCleanURLService.Instance.InsertOrUpdate(_item.Code, "Gift", _item.ID, _item.MenuID, model.LangID);
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
            return ModGiftService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    public class ModGiftModel : DefaultModel
    {
        public int LangID { get; set; } = 1;

        public string SearchText { get; set; }

        public int MenuID { get; set; }
        public int Value { get; set; }
    }
}