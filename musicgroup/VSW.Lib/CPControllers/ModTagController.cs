using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Tags",
        Description = "Quản lý - Tags",
        Code = "ModTag",
        Access = 13,
        Order = 6,
        ShowInMenu = true,
        CssClass = "pencil")]
    public class ModTagController : CPController
    {
        private ModTagModel model = null;
        public ModTagController()
        {
            //khoi tao Service
            DataService = ModTagService.Instance;
            CheckPermissions = true;
            model = new ModTagModel();
        }

        public void ActionIndex()
        {
            model.Sort = CPViewPage.PageViewState.GetValue("Sort").ToString();
            model.SearchText = CPViewPage.PageViewState.GetValue("SearchText").ToString();
            model.PageIndex = (CPViewPage.PageViewState.Exists("PageIndex") ? CPViewPage.PageViewState.GetValue("PageIndex").ToInt(0) : model.PageIndex);
            model.PageSize = (CPViewPage.PageViewState.Exists("PageSize") ? CPViewPage.PageViewState.GetValue("PageSize").ToInt(0) : model.PageSize);
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort);

            // tao danh sach
            var dbQuery = ModTagService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => o.Name.Contains(model.SearchText))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd()
        {
            model.RecordID = CPViewPage.PageViewState.GetValue("RecordID").ToInt(0);
            if (model.RecordID > 0)
            {
                item = ModTagService.Instance.GetByID(model.RecordID);
                // khoi tao gia tri mac dinh khi update
            }
            else
            {
                item = new ModTagEntity();
                // khoi tao gia tri mac dinh khi insert
            }

            ViewBag.Data = item;
            ViewBag.Model = model;
        }

        public void ActionSave()
        {
            if (ValidSave())
               SaveRedirect();
        }

        public void ActionApply()
        {
            if (ValidSave())
               ApplyRedirect(model.RecordID, item.ID);
        }

        public void ActionSaveNew()
        {
            if (ValidSave())
               SaveNewRedirect(model.RecordID, item.ID);
        }

        #region private func

        private ModTagEntity item = null;
        private bool ValidSave()
        {
            model.RecordID = CPViewPage.PageViewState.GetValue("RecordID").ToInt(0);
            if (model.RecordID > 0)
            {
                item = ModTagService.Instance.GetByID(model.RecordID);
                // khoi tao gia tri mac dinh khi update
            }
            else
            {
                item = new ModTagEntity();
                // khoi tao gia tri mac dinh khi insert
            }
            //chong hack
            item.ID = model.RecordID;
            item.Link = CPViewPage.PageViewState.GetValue("Link").ToString();
            item.Title = CPViewPage.PageViewState.GetValue("Title").ToString();
            item.Keywords = CPViewPage.PageViewState.GetValue("Keywords").ToString();
            item.Description = CPViewPage.PageViewState.GetValue("Description").ToString();

            ViewBag.Data = item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            //kiem tra ten 
            //if (item.Name.Trim() == string.Empty)
            //    CPViewPage.Message.ListMessage.Add("Nhập tên.");

            if (CPViewPage.Message.ListMessage.Count == 0)
            {
                 //neu khong nhap code -> tu sinh
                 //if (item.Code.Trim() == string.Empty)
                    //item.Code = Data.GetCode(item.Name);
                try
                {
                    //save
                    ModTagService.Instance.Save(item);
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

        #endregion
    }

    public class ModTagModel : DefaultModel
    {
        public string SearchText { get; set; }
    }
}

