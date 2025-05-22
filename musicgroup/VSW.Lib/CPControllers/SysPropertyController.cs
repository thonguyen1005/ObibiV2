using System;
using System.Collections.Generic;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysPropertyController : CPController
    {
        public SysPropertyController()
        {
            //khoi tao Service
            DataService = WebPropertyService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysPropertyModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort, "[Order]");

            // tao danh sach
            var dbQuery = WebPropertyService.Instance.CreateQuery()
                                    .Where(o => o.ParentID == model.ParentID && o.LangID == model.LangID)
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(SysPropertyModel model)
        {
            if (model.RecordID > 0)
            {
                _item = WebPropertyService.Instance.GetByID(model.RecordID);

                // khoi tao gia tri mac dinh khi update
            }
            else
            {
                _item = new WebPropertyEntity
                {
                    ParentID = model.ParentID,
                    Activity = true,
                    LangID = model.LangID,
                    Order = GetMaxOrder(model)
                };

                // khoi tao gia tri mac dinh khi insert
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysPropertyModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysPropertyModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysPropertyModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            var list = new List<int>();
            GetPropertyIDChildForDelete(ref list, arrID);

            if (list != null && list.Count > 0)
            {
                var sWhere = "[ID] IN (" + Core.Global.Array.ToString(list.ToArray()) + ")";

                //xoa property
                WebPropertyService.Instance.Delete(sWhere);
            }

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        public void ActionUpload(SysPropertyModel model)
        {
            CPViewPage.Script("Redirect", "VSWRedirect('Import')");
        }

        public void ActionImport(SysPropertyModel model)
        {
            ViewBag.Model = model;
        }

        public override void ActionCancel()
        {
            CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl.Replace("Add.aspx", "Index.aspx")
                .Replace("Import.aspx", "Index.aspx"));
        }

        #region private func

        private WebPropertyEntity _item;

        private bool ValidSave(SysPropertyModel model)
        {
            if (!string.IsNullOrEmpty(model.Value) || CPViewPage.PageViewState.Exists("Value"))
            {
                if (model.Value.Trim() == string.Empty)
                    CPViewPage.Message.ListMessage.Add("Nhập tên chuyên mục.");

                if (CPViewPage.Message.ListMessage.Count != 0) return false;

                var parent = WebPropertyService.Instance.GetByID(model.ParentID);
                if (parent == null) return false;

                foreach (var t in model.Value.Split('\n'))
                {
                    if (string.IsNullOrEmpty(t.Trim()) || t.StartsWith("//"))
                        continue;

                    _item = new WebPropertyEntity { Name = t.Trim(), Code = Data.GetCode(t.Trim()) };

                    var exists = WebMenuService.Instance.CreateQuery()
                                        .Where(o => o.Code == _item.Code && o.LangID == parent.LangID)
                                        .Count()
                                        .ToValue()
                                        .ToBool();

                    if (exists) continue;

                    _item.ParentID = model.ParentID;
                    _item.LangID = parent.LangID;

                    _item.Order = GetMaxOrder(model);
                    _item.Activity = true;

                    WebPropertyService.Instance.Save(_item);
                }

                return true;
            }

            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra ten
            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tên loại sản phẩm.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            // neu code khong duoc nhap -> tu dong tao ra khi them moi
            if (_item.Code == string.Empty) _item.Code = Data.GetCode(_item.Name);

            try
            {
                //neu di chuyen thi cap nhat lai Order
                if (model.RecordID > 0 && _item.ParentID != model.ParentID)
                {
                    //cap nhat Order
                    _item.Order = GetMaxOrder(model);
                }

                WebPropertyService.Instance.Save(_item);
            }
            catch (Exception ex)
            {
                Error.Write(ex);
                CPViewPage.Message.ListMessage.Add(ex.Message);
                return false;
            }

            return true;
        }

        private static int GetMaxOrder(SysPropertyModel model)
        {
            return WebPropertyService.Instance.CreateQuery()
                                .Where(o => o.LangID == model.LangID && o.ParentID == model.ParentID)
                                .Max(o => o.Order)
                                .ToValue().ToInt(0) + 1;
        }

        private void GetPropertyIDChildForDelete(ref List<int> list, int[] arrId)
        {
            for (var i = 0; arrId != null && i < arrId.Length; i++)
            {
                GetPropertyIDChild(ref list, arrId[i]);
            }
        }

        private void GetPropertyIDChild(ref List<int> list, int propertyId)
        {
            list.Add(propertyId);

            var listProperty = WebPropertyService.Instance.CreateQuery()
                                                .Where(o => o.ParentID == propertyId)
                                                .ToList();

            for (var i = 0; listProperty != null && i < listProperty.Count; i++)
            {
                GetPropertyIDChild(ref list, listProperty[i].ID);
            }
        }

        #endregion private func
    }

    public class SysPropertyModel : DefaultModel
    {
        public int ParentID { get; set; }

        public int LangID { get; set; } = 1;

        public string SearchText { get; set; }
        public string Value { get; set; }
        public int MenuID { get; set; }
        public string ItemControl { get; set; }
        public bool IsMultiple { get; set; }
    }
}