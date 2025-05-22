using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;


namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Hỏi đáp",
        Description = "Quản lý  - Hỏi đáp",
        Code = "ModFAQ",
        Access = 31,
        Order = 7,
        ShowInMenu = true,
        CssClass = "question-circle")]
    public class ModFAQController : CPController
    {
        public ModFAQController()
        {
            //khoi tao Service
            DataService = ModFAQService.Instance;
            CheckPermissions = true;
        }

        public virtual void ActionIndex(ModFAQModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort);

            // tao danh sach
            var dbQuery = ModFAQService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => o.Name.Contains(model.SearchText))
                                .WhereIn(o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("FAQ", model.MenuID, model.LangID))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModFAQModel model)
        {
            if (model.RecordID > 0)
            {
                item = ModFAQService.Instance.GetByID(model.RecordID);

                // khoi tao gia tri mac dinh khi update
            }
            else
            {
                item = new ModFAQEntity();

                // khoi tao gia tri mac dinh khi insert
                item.MenuID = model.MenuID;
                item.Created = DateTime.Now;
                item.Activity = CPViewPage.UserPermissions.Approve;
                item.Order = GetMaxOrder(model);
            }

            ViewBag.Data = item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModFAQModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModFAQModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, item.ID);
        }

        public void ActionSaveNew(ModFAQModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            // xoa clear url
            ModCleanURLService.Instance.Delete("[Value] IN (" + VSW.Core.Global.Array.ToString(arrID) + ") AND [Type]='FAQ'");
            //xoa News
            DataService.Delete("[ID] IN (" + Core.Global.Array.ToString(arrID) + ")");
            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private ModFAQEntity item = null;
        private bool ValidSave(ModFAQModel model)
        {
            TryUpdateModel(item);

            //chong hack
            item.ID = model.RecordID;

            ViewBag.Data = item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            //kiem tra ten 
            if (item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tiêu đề.");
            if (item.Content.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập câu trả lời .");
            //kiem tra chuyen muc
            if (item.MenuID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn chuyên mục.");

            if (CPViewPage.Message.ListMessage.Count == 0)
            {
                if (string.IsNullOrEmpty(item.Code)) item.Code = Data.GetCode(item.Name + "-f" + item.ID);

                try
                {
                    //save
                    ModFAQService.Instance.Save(item);

                    //update url
                    ModCleanURLService.Instance.InsertOrUpdate(item.Code, "FAQ", item.ID, item.MenuID, model.LangID);
                }
                catch (Exception ex)
                {
                    Global.Error.Write(ex);
                    CPViewPage.Message.ListMessage.Add(ex.Message);
                    return false;
                }

                return true;
            }

            return false;
        }

        private int GetMaxOrder(ModFAQModel model)
        {
            return ModFAQService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion
    }

    public class ModFAQModel : DefaultModel
    {
        private int _LangID = 1;
        public int LangID
        {
            get { return _LangID; }
            set { _LangID = value; }
        }

        public int MenuID { get; set; }
        public string SearchText { get; set; }
        public int Value { get; set; }
    }
}
