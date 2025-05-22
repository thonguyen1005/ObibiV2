using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;

namespace VSW.Lib.CPControllers
{
    public class FormProductVoteController : CPController
    {
        public FormProductVoteController()
        {
            //khoi tao Service
            DataService = ModProductVoteService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModProductVoteModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort, "[Order] ASC");

            // tao danh sach
            var dbQuery = ModProductVoteService.Instance.CreateQuery()
                                            .Where(model.ProductID > 0, o => o.ProductID == model.ProductID)
                                            .Take(model.PageSize)
                                            .OrderBy(orderBy)
                                            .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModProductVoteModel model)
        {
            if (model.RecordID > 0)
            {
                item = ModProductVoteService.Instance.GetByID(model.RecordID);
            }
            else
            {
                model.IsSave = true;
                item = new ModProductVoteEntity();
                item.ProductID = model.ProductID;
                item.Order = GetMaxOrder(model);
                item.Activity = CPViewPage.UserPermissions.Approve;
            }
            ViewBag.Data = item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModProductVoteModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModProductVoteModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, item.ID);
        }

        public void ActionSaveNew(ModProductVoteModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, item.ID);
        }
        public void ActionSavePrice(ModProductVoteModel model)
        {
            if (model.cid != null && model.cid.Length > 0)
            {
                for (int i = 0; i < model.cid.Length; i++)
                {
                    var productVote = ModProductVoteService.Instance.GetByID(model.cid[i]);
                    if (productVote == null) continue;
                    productVote.Yes = CPViewPage.PageViewState.GetValue("Yes" + model.cid[i]).ToInt();
                    productVote.No = CPViewPage.PageViewState.GetValue("No" + model.cid[i]).ToInt();

                    ModProductVoteService.Instance.Save(productVote);
                }
                CPViewPage.SetMessage("Cập nhật thành công.");
                CPViewPage.RefreshPage();
            }
            else
            {
                CPViewPage.SetMessage_error("Chưa có đối tượng nào được chọn.");
                CPViewPage.RefreshPage();
            }
        }
        #region private func

        private ModProductVoteEntity item = null;
        private bool ValidSave(ModProductVoteModel model)
        {
            TryUpdateModel(item);

            //chong hack
            item.ID = model.RecordID;

            ViewBag.Model = model;
            ViewBag.Data = item;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            if (item.ProductID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn sản phẩm.");
            if (string.IsNullOrEmpty(item.Name))
                CPViewPage.Message.ListMessage.Add("Chọn hoặc nhập tên câu hỏi.");
            if (string.IsNullOrEmpty(item.NameYes))
                CPViewPage.Message.ListMessage.Add("Nhập tên hiển thị có.");
            if (string.IsNullOrEmpty(item.NameNo))
                CPViewPage.Message.ListMessage.Add("Nhập tên hiển thị không.");

            if (CPViewPage.Message.ListMessage.Count == 0)
            {
                try
                {
                    if (!string.IsNullOrEmpty(item.Name) && item.MenuID < 1 && model.IsSave)
                    {
                        var _menu = new WebMenuEntity { Name = item.Name.Trim(), Code = Data.GetCode(item.Name.Trim()), Type = "ProductVote" };

                        var exists = WebMenuService.Instance.CreateQuery()
                                            .Where(o => o.Code == _menu.Code && o.Type == _menu.Type && o.LangID == model.LangID)
                                            .Count()
                                            .ToValue()
                                            .ToBool();

                        if (!exists)
                        {
                            _menu.ParentID = 19265;
                            _menu.LangID = model.LangID;
                            _menu.Order = GetMaxMenuOrder(_menu.LangID, _menu.ParentID);
                            _menu.Activity = true;
                            _menu.Summary = (item.NameYes + "," + item.NameNo);
                            WebMenuService.Instance.Save(_menu);
                            item.MenuID = _menu.ID;
                        }
                    }
                    else if (string.IsNullOrEmpty(item.Name) && item.MenuID > 0)
                    {
                        var menu = WebMenuService.Instance.GetByID_Cache(item.MenuID);
                        item.Name = menu?.Name ?? "";
                    }
                    ModProductVoteService.Instance.Save(item);

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
        private static int GetMaxOrder(ModProductVoteModel model)
        {
            return ModProductVoteService.Instance.CreateQuery()
                    .Where(o => o.ProductID == model.ProductID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxMenuOrder(int LangID, int ParentID)
        {
            return WebMenuService.Instance.CreateQuery()
                    .Where(o => o.LangID == LangID && o.ParentID == ParentID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        #endregion
    }

    public class ModProductVoteModel : DefaultModel
    {
        private int _LangID = 1;
        public int LangID
        {
            get { return _LangID; }
            set { _LangID = value; }
        }

        public int ProductID { get; set; }
        public bool IsSave { get; set; }
        public int[] cid { get; set; }
    }
}

