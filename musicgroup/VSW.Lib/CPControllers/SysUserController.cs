using System;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysUserController : CPController
    {
        public SysUserController()
        {
            //khoi tao Service
            DataService = CPUserService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysUserModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = CPUserService.Instance.CreateQuery()
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(SysUserModel model)
        {
            _item = model.RecordID > 0 ? CPUserService.Instance.GetByID(model.RecordID) : new CPUserEntity();

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysUserModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysUserModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysUserModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        #region private func

        private CPUserEntity _item;

        private bool ValidSave(SysUserModel model)
        {
            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra ten dang nhap
            if (_item.LoginName.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tên người sử dụng.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            if (model.NewPassword != string.Empty)
                _item.Password = Security.Md5(model.NewPassword);

            try
            {
                //save
                CPUserService.Instance.Save(_item);

                //xoa
                CPUserRoleService.Instance.Delete(o => o.UserID == _item.ID);

                //them
                for (var i = 0; model.ArrRole != null && i < model.ArrRole.Length; i++)
                {
                    CPUserRoleService.Instance.Save(new CPUserRoleEntity
                    {
                        UserID = _item.ID,
                        RoleID = model.ArrRole[i]
                    });
                }
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

    public class SysUserModel : DefaultModel
    {
        public int[] ArrRole { get; set; }
        public string NewPassword { get; set; }
    }
}