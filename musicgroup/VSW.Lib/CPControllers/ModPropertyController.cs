using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;

namespace VSW.Lib.CPControllers
{
    public class ModPropertyController : CPController
    {
        public ModPropertyController()
        {
            //khoi tao Service
            DataService = ModPropertyService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModPropertyModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort);

            // tao danh sach
            var dbQuery = ModPropertyService.Instance.CreateQuery()
                                .WhereIn(o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("Property", model.KhoavantayID, model.LangID))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModPropertyModel model)
        {
            if (model.RecordID > 0)
            {
                entity = ModPropertyService.Instance.GetByID(model.RecordID);

                // khoi tao gia tri mac dinh khi update
            }
            else
            {
                entity = new ModPropertyEntity();

                // khoi tao gia tri mac dinh khi insert
                entity.MenuID = model.MenuID;
            }

            ViewBag.Data = entity;
            ViewBag.Model = model;
        }

        public void ActionSave(ModPropertyModel model)
        {
            if (ValidSave(model))
               SaveRedirect();
        }

        public void ActionApply(ModPropertyModel model)
        {
            if (ValidSave(model))
               ApplyRedirect(model.RecordID, entity.ID);
        }

        public void ActionSaveNew(ModPropertyModel model)
        {
            if (ValidSave(model))
               SaveNewRedirect(model.RecordID, entity.ID);
        }

        #region private func

        private ModPropertyEntity entity = null;

        private bool ValidSave(ModPropertyModel model)
        {
            TryUpdateModel(entity);

            //chong hack
            entity.ID = model.RecordID;

            ViewBag.Data = entity;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            //kiem tra chuyen muc
            if (entity.MenuID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn chuyên mục.");

            if (CPViewPage.Message.ListMessage.Count == 0)
            {

                //save
                ModPropertyService.Instance.Save(entity);

                return true;
            }

            return false;
        }

        #endregion
    }

    public class ModPropertyModel : DefaultModel
    {
        private int _LangID = 1;
        public int LangID
        {
            get { return _LangID; }
            set { _LangID = value; }
        }

        public int MenuID { get; set; }
        public int KhoavantayID { get;set; }
    }
}

