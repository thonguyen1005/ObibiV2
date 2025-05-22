using System;
using System.Collections.Generic;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysMenuController : CPController
    {
        public SysMenuController()
        {
            //khoi tao Service
            DataService = WebMenuService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysMenuModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort, "[Order]");
            if (model.ParentID == 0 && !string.IsNullOrEmpty(model.Type))
            {
                if (model.Selected > 0)
                {
                    var menu = WebMenuService.Instance.GetByID_Cache(model.Selected);
                    if (menu != null) model.ParentID = menu.ParentID;
                }
                else
                {
                    var menu = WebMenuService.Instance.CreateQuery().Where(o => o.ParentID == 0 && o.Type == model.Type).ToSingle_Cache();
                    if (menu != null) model.ParentID = menu.ID;
                }
            }
            //tao danh sach
            var dbQuery = WebMenuService.Instance.CreateQuery()
                                    .Where(!string.IsNullOrEmpty(model.SearchText),
                                        o => (o.Name.Contains(model.SearchText) || o.Code.Contains(model.SearchText)))
                                    .Where(o => o.ParentID == model.ParentID && o.LangID == model.LangID)
                                    .Take(model.PageSize)
                                    .Skip(model.PageIndex * model.PageSize)
                                    .OrderBy(orderBy);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(SysMenuModel model)
        {
            if (model.RecordID > 0)
            {
                _item = WebMenuService.Instance.GetByID(model.RecordID);

                //khoi tao gia tri mac dinh khi update
            }
            else
            {
                //khoi tao gia tri mac dinh khi insert
                _item = new WebMenuEntity
                {
                    LangID = model.LangID,
                    ParentID = model.ParentID,
                    Type = model.ParentID > 0 ? WebMenuService.Instance.GetByID(model.ParentID).Type : "News",
                    Order = GetMaxOrder(model),
                    Activity = true
                };
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysMenuModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysMenuModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysMenuModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            var list = new List<int>();
            GetMenuIDChildForDelete(ref list, arrID);

            if (list != null && list.Count > 0)
            {
                var sWhere = "[MenuID] IN (" + Core.Global.Array.ToString(list.ToArray()) + ")";

                ////xoa adv
                //if (ModAdvService.Instance.Exists(sWhere))
                //{
                //    CPViewPage.Alert("Chuyên mục bạn xóa còn chứa dữ liệu. Hãy xóa hết dữ liệu trước.");
                //    return;
                //}

                ////xoa news
                //if (ModNewsService.Instance.Exists(sWhere))
                //{
                //    CPViewPage.Alert("Chuyên mục bạn xóa còn chứa Bài viết. Hãy xóa hết Bài viết trong chuyên mục muốn xóa trước.");
                //    return;
                //}

                ////xoa product
                //if (ModProductService.Instance.Exists(sWhere))
                //{
                //   CPViewPage.Alert("Chuyên mục bạn xóa còn chứa Sản phẩm. Hãy xóa hết Sản phẩm trong chuyên mục muốn xóa trước.");
                //   return;
                //}

                //xoa menu
                sWhere = "[ID] IN (" + Core.Global.Array.ToString(list.ToArray()) + ")";

                WebMenuService.Instance.Delete(sWhere);
            }

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        public void ActionUpload(SysMenuModel model)
        {
            CPViewPage.Script("Redirect", "VSWRedirect('Import')");
        }

        public void ActionImport(SysMenuModel model)
        {
            ViewBag.Model = model;
        }

        public override void ActionCancel()
        {
            CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl.Replace("Add.aspx", "Index.aspx")
                .Replace("Import.aspx", "Index.aspx"));
        }
        #region private func

        private WebMenuEntity _item;
        private bool ValidSave(SysMenuModel model)
        {
            if (!string.IsNullOrEmpty(model.Value) || CPViewPage.PageViewState.Exists("Value"))
            {
                if (model.Value.Trim() == string.Empty)
                    CPViewPage.Message.ListMessage.Add("Nhập tên chuyên mục.");

                if (CPViewPage.Message.ListMessage.Count != 0) return false;

                var parent = WebMenuService.Instance.GetByID(model.ParentID);
                if (parent == null) return false;

                var ArrItem = model.Value.Split('\n');

                foreach (var t in ArrItem)
                {
                    if (string.IsNullOrEmpty(t.Trim()) || t.StartsWith("//"))
                        continue;

                    _item = new WebMenuEntity { Name = t.Trim(), Code = Data.GetCode(t.Trim()) };

                    var exists = WebMenuService.Instance.CreateQuery()
                                        .Where(o => o.Code == _item.Code && o.Type == parent.Type && o.LangID == parent.LangID)
                                        .Count()
                                        .ToValue()
                                        .ToBool();

                    if (exists) continue;

                    _item.Type = parent.Type;

                    _item.ParentID = model.ParentID;
                    _item.LangID = parent.LangID;

                    _item.Order = GetMaxOrder(model);
                    _item.Activity = true;

                    WebMenuService.Instance.Save(_item);
                }

                return true;
            }
            else
            {
                TryUpdateModel(_item);

                ViewBag.Data = _item;
                ViewBag.Model = model;

                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

                //kiem tra ten
                if (_item.Name.Trim() == string.Empty)
                    CPViewPage.Message.ListMessage.Add("Nhập tên chuyên mục.");

                if (CPViewPage.Message.ListMessage.Count != 0) return false;

                //neu code khong duoc nhap -> tu dong tao ra khi them moi
                if (_item.Code == string.Empty)
                    _item.Code = Data.GetCode(_item.Name);

                //cap nhat state
                _item.State = GetState(model.ArrState);

                try
                {
                    //neu di chuyen thi cap nhat lai Type va Order
                    if (model.RecordID > 0 && _item.ParentID != model.ParentID)
                    {
                        //cap nhat Type
                        if (_item.ParentID != 0)
                            _item.Type = WebMenuService.Instance.GetByID(_item.ParentID).Type;

                        //cap nhat Order
                        _item.Order = GetMaxOrder(model);
                    }

                    //save
                    WebMenuService.Instance.Save(_item);

                    //neu di chuyen thi cap nhat lai Type cua chuyen muc con
                    if (model.RecordID > 0 && _item.ParentID != model.ParentID && _item.ParentID != 0)
                    {
                        //lay danh sach chuyen muc con
                        var list = new List<int>();
                        GetMenuIDChild(ref list, model.RecordID);

                        //cap nhat Type cho danh sach chuyen muc con
                        if (list.Count > 1)
                            WebMenuService.Instance.Update("[ID] IN (" + Core.Global.Array.ToString(list.ToArray()) + ")",
                                "@Type", WebMenuService.Instance.GetByID(_item.ParentID).Type);
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
        }

        private static int GetMaxOrder(SysMenuModel model)
        {
            return WebMenuService.Instance.CreateQuery()
                    .Where(o => o.LangID == model.LangID && o.ParentID == model.ParentID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxOrder(int LangID, int ParentID)
        {
            return WebMenuService.Instance.CreateQuery()
                    .Where(o => o.LangID == LangID && o.ParentID == ParentID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private void GetMenuIDChildForDelete(ref List<int> list, int[] arrID)
        {
            for (var i = 0; arrID != null && i < arrID.Length; i++)
            {
                GetMenuIDChild(ref list, arrID[i]);
            }
        }

        private static void GetMenuIDChild(ref List<int> list, int menuID)
        {
            list.Add(menuID);

            var listMenu = WebMenuService.Instance.CreateQuery()
                                                .Where(o => o.ParentID == menuID)
                                                .ToList();

            for (var i = 0; listMenu != null && i < listMenu.Count; i++)
            {
                GetMenuIDChild(ref list, listMenu[i].ID);
            }
        }

        #endregion private func
    }

    public class SysMenuModel : DefaultModel
    {
        public int ParentID { get; set; }

        private int _langID = 1;
        public int LangID
        {
            get { return _langID; }
            set { _langID = value; }
        }

        public string SearchText { get; set; }

        public int State { get; set; }
        public int[] ArrState { get; set; }

        public string Value { get; set; }
        public string Excel { get; set; }
        public string Type { get; set; }
        public int Selected { get; set; }
        public string ItemControl { get; set; }
    }
}