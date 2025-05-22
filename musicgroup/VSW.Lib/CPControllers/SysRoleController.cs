using System;
using System.Collections.Generic;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysRoleController : CPController
    {
        public SysRoleController()
        {
            //khoi tao Service
            DataService = CPRoleService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysRoleModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort, "[Order]");

            //tao danh sach
            var dbQuery = CPRoleService.Instance.CreateQuery()
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(SysRoleModel model)
        {
            _item = model.RecordID > 0 ? CPRoleService.Instance.GetByID(model.RecordID) : new CPRoleEntity { Order = GetMaxOrder() };

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysRoleModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysRoleModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysRoleModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            foreach (var id in arrID)
            {
                var item = CPRoleService.Instance.GetByID(id);

                if (item.Lock)
                    continue;

                //thuc thi
                var roleID = id;
                CPUserRoleService.Instance.Delete(o => o.RoleID == roleID);
                var id1 = id;
                CPAccessService.Instance.Delete(o => o.RoleID == id1);
                CPRoleService.Instance.Delete(id);
            }

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private CPRoleEntity _item;

        private bool ValidSave(SysRoleModel model)
        {
            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra ten
            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tên nhóm người sử dụng.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            try
            {
                //save
                CPRoleService.Instance.Save(_item);

                UpdateRoleModule(model);
            }
            catch (Exception ex)
            {
                Error.Write(ex);
                CPViewPage.Message.ListMessage.Add(ex.Message);
                return false;
            }

            return true;
        }

        private void UpdateRoleModule(SysRoleModel model)
        {
            CPAccessService.Instance.Delete(o => o.Type == "CP.MODULE" && o.RoleID == _item.ID);

            for (var i = -1; i < Web.Application.CPModules.Count; i++)
            {
                var moduleCode = "SysAdministrator";

                if (i > -1)
                    moduleCode = Web.Application.CPModules[i].Code;

                var access = 0;

                if (model.ArrApprove != null && System.Array.IndexOf(model.ArrApprove, moduleCode) > -1)
                    access |= 16;
                if (model.ArrDelete != null && System.Array.IndexOf(model.ArrDelete, moduleCode) > -1)
                    access |= 8;
                if (model.ArrEdit != null && System.Array.IndexOf(model.ArrEdit, moduleCode) > -1)
                    access |= 4;
                if (model.ArrAdd != null && System.Array.IndexOf(model.ArrAdd, moduleCode) > -1)
                    access |= 2;
                if (model.ArrView != null && System.Array.IndexOf(model.ArrView, moduleCode) > -1)
                    access |= 1;

                if (access <= 0) continue;

                if ((access & 1) != 1)
                    access |= 1;

                var accessEntity = new CPAccessEntity
                {
                    RefCode = moduleCode,
                    RoleID = _item.ID,
                    Value = access,
                    Type = "CP.MODULE"
                };
                CPAccessService.Instance.Save(accessEntity);
            }
        }

        private static int GetMaxOrder()
        {
            return CPRoleService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    public class SysRoleModel : DefaultModel
    {
        public string[] ArrApprove { get; set; }
        public string[] ArrDelete { get; set; }
        public string[] ArrEdit { get; set; }
        public string[] ArrAdd { get; set; }
        public string[] ArrView { get; set; }
    }
}