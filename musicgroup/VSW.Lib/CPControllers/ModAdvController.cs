using System;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Quảng cáo / Liên kết",
        Description = "Quản lý - Quảng cáo / Liên kết",
        Code = "ModAdv",
        Access = 31,
        Order = 10,
        ShowInMenu = true,
        CssClass = "icon-16-media")]
    public class ModAdvController : CPController
    {
        public ModAdvController()
        {
            //khoi tao Service
            DataService = ModAdvService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModAdvModel model)
        {
            //sap xep tu dong
            string orderBy = AutoSort(model.Sort, "[Order]");

            //tao danh sach
            var dbQuery = ModAdvService.Instance.CreateQuery()
                                    .WhereIn(model.MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("Adv", model.MenuID, model.LangID))
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModAdvModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModAdvService.Instance.GetByID(model.RecordID);

                //khoi tao gia tri mac dinh khi update
            }
            else
            {
                _item = new ModAdvEntity
                {
                    MenuID = model.MenuID,
                    Activity = CPViewPage.UserPermissions.Approve,
                    Order = GetMaxOrder()
                };

                //khoi tao gia tri mac dinh khi insert
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModAdvModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModAdvModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModAdvModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        #region private func

        private ModAdvEntity _item;

        private bool ValidSave(ModAdvModel model)
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

            //kiem tra chuyen muc
            if (_item.MenuID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn chuyên mục.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            try
            {
                //save
                ModAdvService.Instance.Save(_item);
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
            return ModAdvService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    public class ModAdvModel : DefaultModel
    {
        private int _LangID = 1;

        public int LangID
        {
            get => _LangID;
            set => _LangID = value;
        }

        public int MenuID { get; set; }
    }
}