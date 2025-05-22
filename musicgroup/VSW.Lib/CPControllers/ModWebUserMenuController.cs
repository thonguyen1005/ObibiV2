using System;
using System.Collections.Generic;
using System.Web;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Phân loại khách hàng",
        Description = "Quản lý - Phân loại khách hàng",
        Code = "ModWebUserMenu",
        Access = 31,
        Order = 8,
        ShowInMenu = true,
        CssClass = "user")]
    public class ModWebUserMenuController : CPController
    {
        public ModWebUserMenuController()
        {
            //khoi tao Service
            DataService = ModWebUserMenuService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(ModWebUserMenuModel model)
        {
            //sap xep tu dong
            string orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = ModWebUserMenuService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText)))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModWebUserMenuModel model)
        {
            if (model.RecordID > 0)
            {
                _item = model.RecordID > 0 ? ModWebUserMenuService.Instance.GetByID(model.RecordID) : new ModWebUserMenuEntity();
            }
            else
            {
                //khoi tao gia tri mac dinh khi insert
                _item = new ModWebUserMenuEntity
                {
                    Created = DateTime.Now,
                    Order = GetMaxOrder(),
                    Activity = CPViewPage.UserPermissions.Approve
                };
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModWebUserMenuModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModWebUserMenuModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModWebUserMenuModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            //xoa News
            DataService.Delete("[ID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }
        #region private func

        private ModWebUserMenuEntity _item;

        private bool ValidSave(ModWebUserMenuModel model)
        {
            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập: Tên phân loại.");

            if (CPViewPage.Message.ListMessage.Count == 0)
            {
                try
                {
                    _item.Created = DateTime.Now;
                    _item.Count = _item.Count < 0 ? _item.Count : 0;
                    //save
                    ModWebUserMenuService.Instance.Save(_item);
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
            return ModWebUserMenuService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        #endregion private func
    }

    public class ModWebUserMenuModel : DefaultModel
    {
        public string SearchText { get; set; }
        public string Password2 { get; set; }
    }
}