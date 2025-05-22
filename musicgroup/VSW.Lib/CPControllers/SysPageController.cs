using System;
using System.Collections.Generic;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysPageController : CPController
    {
        public SysPageController()
        {
            //khoi tao Service
            DataService = SysPageService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysPageModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort, "[Order]");

            //tao danh sach
            var dbQuery = SysPageService.Instance.CreateQuery()
                                    .Where(o => o.ParentID == model.ParentID && o.LangID == model.LangID)
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(SysPageModel model)
        {
            if (model.RecordID > 0)
            {
                _item = SysPageService.Instance.GetByID(model.RecordID);

                //khoi tao gia tri mac dinh khi update
                if (_item.Updated <= DateTime.MinValue) _item.Updated = DateTime.Now;
            }
            else
            {
                _item = new SysPageEntity
                {
                    ParentID = model.ParentID,
                    LangID = model.LangID,
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxOrder(model),
                    Activity = true
                };

                //khoi tao gia tri mac dinh khi insert
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysPageModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysPageModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysPageModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }
        public override void ActionDelete(int[] arrID)
        {
            var list = new List<int>();
            GetPageIDChildForDelete(ref list, arrID);

            foreach (var pageId in list)
            {
                //xoa cleanurl
                var id = pageId;
                ModCleanURLService.Instance.Delete(o => o.Type == "Page" && o.Value == id);

                //xoa Page
                SysPageService.Instance.Delete(pageId);
            }

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        public void ActionUpload(SysPageModel model)
        {
            CPViewPage.Script("Redirect", "VSWRedirect('Import')");
        }

        public void ActionImport(SysPageModel model)
        {
            ViewBag.Model = model;
        }

        public override void ActionCancel()
        {
            CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl.Replace("Add.aspx", "Index.aspx")
                .Replace("Import.aspx", "Index.aspx"));
        }

        #region private func

        private SysPageEntity _item;

        private bool ValidSave(SysPageModel model)
        {
            if (!string.IsNullOrEmpty(model.Value) || CPViewPage.PageViewState.Exists("Value"))
            {
                if (model.Value.Trim() == string.Empty)
                    CPViewPage.Message.ListMessage.Add("Nhập tên chuyên mục.");

                if (CPViewPage.Message.ListMessage.Count != 0) return false;

                foreach (var t in model.Value.Split('\n'))
                {
                    if (string.IsNullOrEmpty(t.Trim()) || t.StartsWith("//"))
                        continue;

                    _item = new SysPageEntity { Name = t.Trim(), Code = Data.GetCode(t.Trim()) };

                    //khoi tao gia tri mac dinh khi insert

                    var menu = WebMenuService.Instance.CreateQuery()
                                        .Where(o => o.Code == _item.Code)
                                        .ToSingle();

                    if (menu != null)
                    {
                        _item.MenuID = menu.ID;
                        _item.ModuleCode = "M" + (string.IsNullOrEmpty(model.Type) ? menu.Type : model.Type);

                        var template = SysTemplateService.Instance.CreateQuery()
                                                    .Where(o => o.Name == menu.Type && o.LangID == menu.LangID)
                                                    .ToSingle() ?? SysTemplateService.Instance.CreateQuery().Where(o => o.LangID == menu.LangID).Take(1).ToSingle();

                        if (template != null) _item.TemplateID = template.ID;
                    }

                    _item.ParentID = model.ParentID;
                    _item.LangID = model.LangID;

                    _item.Created = DateTime.Now;
                    _item.Updated = DateTime.Now;
                    _item.Order = GetMaxOrder(model);
                    _item.Activity = true;

                    SysPageService.Instance.Save(_item);

                    //update url
                    ModCleanURLService.Instance.InsertOrUpdate(_item.Code, "Page", _item.ID, _item.MenuID, model.LangID);
                }

                return true;
            }

            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra ten
            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tên trang.");

            if (ModCleanURLService.Instance.CheckCode(_item.Code, "Page", _item.ID, model.LangID))
                CPViewPage.Message.ListMessage.Add("Mã đã tồn tại. Vui lòng chọn mã khác.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            //neu code khong duoc nhap -> tu dong tao ra khi them moi
            if (_item.Code == string.Empty)
            {
                if (_item.ModuleCode == "MBSCar")
                    _item.Code = _item.Parent.Code + "-" + Data.GetCode(_item.Name);
                else
                    _item.Code = Data.GetCode(_item.Name);
            }

            //neu di chuyen thi cap nhat lai Order
            if (model.RecordID > 0 && _item.ParentID != model.ParentID)
                _item.Order = GetMaxOrder(model);

            //cap nhat state
            _item.State = GetState(model.ArrState);

            try
            {
                //save
                SysPageService.Instance.Save(_item);

                //update url
                ModCleanURLService.Instance.InsertOrUpdate(_item.Code, "Page", _item.ID, _item.MenuID, model.LangID);
            }
            catch (Exception ex)
            {
                Error.Write(ex);
                CPViewPage.Message.ListMessage.Add(ex.Message);
                return false;
            }

            return true;
        }

        private static int GetMaxOrder(SysPageModel model)
        {
            return SysPageService.Instance.CreateQuery()
                    .Where(o => o.LangID == model.LangID && o.ParentID == model.ParentID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        private static void GetPageIDChildForDelete(ref List<int> list, int[] arrId)
        {
            for (var i = 0; arrId != null && i < arrId.Length; i++)
            {
                GetPageIDChild(ref list, arrId[i]);
            }
        }

        private static void GetPageIDChild(ref List<int> list, int pageId)
        {
            list.Add(pageId);

            var listPage = SysPageService.Instance.CreateQuery()
                                                .Where(o => o.ParentID == pageId)
                                                .ToList();

            for (var i = 0; listPage != null && i < listPage.Count; i++)
            {
                GetPageIDChild(ref list, listPage[i].ID);
            }
        }

        #endregion private func
    }

    public class SysPageModel : DefaultModel
    {
        public int ParentID { get; set; }

        public int LangID { get; set; } = 1;

        public int State { get; set; }
        public string[] ArrState { get; set; }

        public string Type { get; set; }
        public string Value { get; set; }
    }
}