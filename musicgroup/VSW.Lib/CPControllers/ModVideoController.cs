using HtmlAgilityPack;
using System;
using System.Xml;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;
using Array = VSW.Core.Global.Array;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Video",
        Description = "Quản lý  - Video",
        Code = "ModVideo",
        Access = 31,
        Order = 5,
        ShowInMenu = true,
        CssClass = "icon-16-article")]
    public class ModVideoController : CPController
    {
        public ModVideoController()
        {
            //khoi tao Service
            DataService = ModVideoService.Instance;
            CheckPermissions = true;
        }

        public virtual void ActionIndex(ModVideoModel model)
        {
            // sap xep tu dong
            var orderBy = AutoSort(model.Sort);

            // tao danh sach
            var dbQuery = ModVideoService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(model.SearchText)))
                                .Where(model.State > 0, o => (o.State & model.State) == model.State)
                                .WhereIn(o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("Video", model.MenuID, model.LangID))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModVideoModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModVideoService.Instance.GetByID(model.RecordID);

                // khoi tao gia tri mac dinh khi update
                if (_item.Updated <= DateTime.MinValue) _item.Updated = DateTime.Now;
            }
            else
            {
                _item = new ModVideoEntity
                {
                    MenuID = model.MenuID,
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxOrder(),
                    Activity = CPViewPage.UserPermissions.Approve
                };

                // khoi tao gia tri mac dinh khi insert
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModVideoModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModVideoModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModVideoModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            // xoa clear url
            ModCleanURLService.Instance.Delete("[Value] IN (" + Array.ToString(arrID) + ") AND [Type]='Video'");

            //xoa Video
            DataService.Delete("[ID] IN (" + Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private ModVideoEntity _item;

        private bool ValidSave(ModVideoModel model)
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

            if (ModCleanURLService.Instance.CheckCode(_item.Code, model.LangID, _item.ID))
                CPViewPage.Message.ListMessage.Add("Mã đã tồn tại. Vui lòng chọn mã khác.");

            //kiem tra chuyen muc
            if (_item.MenuID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn chuyên mục.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            if (string.IsNullOrEmpty(_item.Code)) _item.Code = Data.GetCode(_item.Name) + "-v" + _item.ID;

            //cap nhat state
            _item.State = GetState(model.ArrState);

            try
            {
                if (!string.IsNullOrEmpty(_item.File))
                {
                    if (_item.File.StartsWith("<iframe"))
                    {
                        var docxml = new HtmlDocument();
                        docxml.LoadHtml(_item.File);
                        var node = docxml.DocumentNode.SelectNodes(@"//iframe");
                        if (node != null)
                        {
                            _item.File = node[0].Attributes["src"].Value.Trim();
                        }
                    }
                    if (_item.File.Contains("youtube.com") && !_item.File.Contains("enablejsapi") && !_item.File.Contains("?"))
                    {
                        _item.File += "?autoplay=0&enablejsapi=1&hd=1&rel=0&wmode=transparent&modestbranding=1";
                    }
                }
                if (string.IsNullOrEmpty(_item.Image))
                {
                    string[] _ArrCode = _item.File.Split('/');
                    if (_ArrCode.Length > 1) _item.Image = "https://img.youtube.com/vi/" + _ArrCode[_ArrCode.Length - 1].Split('?')[0] + "/0.jpg";
                }
                //save
                ModVideoService.Instance.Save(_item);

                //update url
                ModCleanURLService.Instance.InsertOrUpdate(_item.Code, "Video", _item.ID, _item.MenuID, model.LangID);

                //update tag
                //ModTagService.Instance.UpdateTag(item.ID, item.Tags);
            }
            catch (Exception ex)
            {
                Error.Write(ex);
                CPViewPage.Message.ListMessage.Add(ex.Message);
                return false;
            }

            return true;
        }

        private static int GetMaxOrder()
        {
            return ModVideoService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    public class ModVideoModel : DefaultModel
    {
        private int _langID = 1;

        public int LangID
        {
            get { return _langID; }
            set { _langID = value; }
        }

        public int MenuID { get; set; }
        public int State { get; set; }
        public string SearchText { get; set; }

        public int[] ArrState { get; set; }
        public int Value { get; set; }
    }
}