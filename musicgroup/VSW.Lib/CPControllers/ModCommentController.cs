using System;
using System.Linq;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Bình luận",
        Description = "Quản lý - Bình luận",
        Code = "ModComment",
        Access = 31,
        Order = 9,
        ShowInMenu = true,
        CssClass = "star")]
    public class ModCommentController : CPController
    {
        public ModCommentController()
        {
            //khoi tao Service
            DataService = ModVoteService.Instance;
            DataEntity = new ModVoteEntity();
            CheckPermissions = true;
        }

        public void ActionIndex(ModCommentModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);
            if (model.ParentID < 0) model.ParentID = 0;
            //tao danh sach
            var dbQuery = ModVoteService.Instance.CreateQuery()
                                .Where(o => o.ParentID == model.ParentID)
                                .Where(o => o.Vote < 1)
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText)))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModCommentModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModVoteService.Instance.GetByID(model.RecordID);
            }
            else
            {
                _item = new ModVoteEntity
                {
                    Created = DateTime.Now,
                    Activity = CPViewPage.UserPermissions.Approve,
                    ParentID = model.ParentID,
                    ProductID = model.ProductID
                };

                if (model.ParentID > 0)
                {
                    var parent = ModVoteService.Instance.GetByID(model.ParentID);
                    _item.Type = parent?.Type;
                    _item.ProductID = parent?.ProductID ?? 0;
                }
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModCommentModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModCommentModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModCommentModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionPublish(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")", "@Activity", 1);
            ModVoteDetailService.Instance.Update("[CommentID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")", "@Activity", 1);
            ModVoteFileService.Instance.Update("[CommentID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")", "@Activity", 1);
            //thong bao
            CPViewPage.SetMessage("Đã duyệt thành công.");
            CPViewPage.RefreshPage();
        }

        public override void ActionUnPublish(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")", "@Activity", 0);
            ModVoteDetailService.Instance.Update("[CommentID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")", "@Activity", 0);
            ModVoteFileService.Instance.Update("[CommentID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")", "@Activity", 0);
            //thong bao
            CPViewPage.SetMessage("Đã bỏ duyệt thành công.");
            CPViewPage.RefreshPage();
        }

        public override void ActionPublishGX(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID]=" + arrID[0], "@Activity", arrID[1]);
            ModVoteDetailService.Instance.Update("[CommentID]=" + arrID[0], "@Activity", arrID[1]);
            ModVoteFileService.Instance.Update("[CommentID]=" + arrID[0], "@Activity", arrID[1]);

            //thong bao
            CPViewPage.SetMessage(arrID[1] == 0 ? "Đã bỏ duyệt thành công." : "Đã duyệt thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private ModVoteEntity _item;

        private bool ValidSave(ModCommentModel model)
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
            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tiêu đề.");
            if (_item.ProductID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn sản phẩm.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            try
            {
                if (!string.IsNullOrEmpty(model.Vote))
                {
                    _item.Vote = VSW.Core.Global.Convert.ToDouble(model.Vote.Replace(".", ","));
                }
                if (string.IsNullOrEmpty(_item.IP))
                {
                    _item.IP = VSW.Core.Web.HttpRequest.IP;
                }
                if (_item.ParentID < 0) _item.ParentID = 0;
                if (_item.Vote < 0) _item.Vote = 0;
                if (!string.IsNullOrEmpty(_item.OrderCode) && _item.OrderID < 1)
                {
                    var order = ModOrderService.Instance.GetByCode(_item.OrderCode);
                    _item.OrderID = order != null ? order.ID : 0;
                }
                //save
                ModVoteService.Instance.Save(_item);
                ModVoteDetailService.Instance.Update("[CommentID]=" + _item.ID, "@Activity", (_item.Activity ? 1 : 0));
                ModVoteFileService.Instance.Update("[CommentID]=" + _item.ID, "@Activity", (_item.Activity ? 1 : 0));
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

    public class ModCommentModel : DefaultModel
    {
        public int LangID { get; set; } = 1;
        public int ParentID { get; set; }
        public int ProductID { get; set; }
        public string SearchText { get; set; }
        public string Vote { get; set; }
        public int[] cid { get; set; }
    }
}
