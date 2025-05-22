using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "SEO đường dẫn",
        Description = "Quản lý - SEO đường dẫn",
        Code = "ModUrlSeoActivity",
        Access = 31,
        Order = 4,
        ShowInMenu = true,
        CssClass = "google-plus-square")]
    public class ModUrlSeoActivityController : CPController
    {
        public ModUrlSeoActivityController()
        {
            //khoi tao Service
            DataService = ModUrlSeoActivityService.Instance;
            DataEntity = new ModUrlSeoActivityEntity();
            CheckPermissions = true;
        }

        public void ActionIndex(ModUrlSeoActivityModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);
            //tao danh sach
            var dbQuery = ModUrlSeoActivityService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Url.Contains(model.SearchText) || o.UrlRedirect.Contains(model.SearchText)))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModUrlSeoActivityModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModUrlSeoActivityService.Instance.GetByID(model.RecordID);
            }
            else
            {
                _item = new ModUrlSeoActivityEntity
                {
                    CountSearch = 1
                };
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModUrlSeoActivityModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModUrlSeoActivityModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModUrlSeoActivityModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }
        #region private func

        private ModUrlSeoActivityEntity _item;

        private bool ValidSave(ModUrlSeoActivityModel model)
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
            if (_item.Url.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập đường dẫn.");
            if (_item.UrlRedirect.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập đường dẫn chuẩn SEO.");

            try
            {
                //save
                ModUrlSeoActivityService.Instance.Save(_item);
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

    public class ModUrlSeoActivityModel : DefaultModel
    {
        public int LangID { get; set; } = 1;
        public string SearchText { get; set; }
        public string Excel { get; set; }
    }
}