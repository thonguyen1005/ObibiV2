using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using Array = VSW.Core.Global.Array;

namespace VSW.Lib.MVC
{
    public class CPController : Core.MVC.Controller
    {
        public CPViewPage CPViewPage => ViewPageBase as CPViewPage;
        public CPViewControl CPViewControl => ViewControl as CPViewControl;

        protected dynamic DataService { get; set; }
        protected dynamic DataEntity { get; set; }
        protected bool CheckPermissions { get; set; }

        protected string AutoSort(string sort)
        {
            return AutoSort(sort, "[ID] DESC");
        }

        protected string AutoSort(string sort, string orderDefault)
        {
            if (string.IsNullOrEmpty(sort))
                return orderDefault;

            var sortType = sort.Split('-')[0]
                                  .Replace("'", string.Empty)
                                  .Replace("-", string.Empty)
                                  .Replace(";", string.Empty);

            var sortDesc = string.Equals("desc", sort.Split('-')[1].ToLower(), StringComparison.OrdinalIgnoreCase);

            return "[" + sortType + "] " + (sortDesc ? "DESC" : "ASC");
        }

        protected int GetState(int[] arrState)
        {
            var state = 0;

            for (var i = 0; arrState != null && i < arrState.Length; i++)
                if (arrState[i] > 0) state ^= arrState[i];

            return state;
        }
        protected int GetState(string[] arrState)
        {
            var state = 0;

            for (var i = 0; arrState != null && i < arrState.Length; i++)
                if (!string.IsNullOrEmpty(arrState[i])) state ^= VSW.Core.Global.Convert.ToInt(arrState[i]);

            return state;
        }
        protected int GetState(List<int> arrState)
        {
            var state = 0;

            for (var i = 0; arrState != null && i < arrState.Count; i++)
                if (arrState[i] > 0) state ^= arrState[i];

            return state;
        }

        protected void SaveRedirect()
        {
            CPViewPage.SetMessage("Thông tin đã cập nhật.");
            CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl.Replace("Add.aspx", "Index.aspx").Replace("Import.aspx", "Index.aspx"));
        }

        protected void ApplyRedirect(int recordID, int itemID)
        {
            CPViewPage.SetMessage("Thông tin đã cập nhật.");

            if (recordID > 0)
                CPViewPage.RefreshPage();
            else
                CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl + "/RecordID/" + itemID);
        }

        protected void SaveNewRedirect(int recordID, int itemID)
        {
            CPViewPage.SetMessage("Thông tin đã cập nhật.");

            CPViewPage.Response.Redirect(recordID > 0
                ? CPViewPage.Request.RawUrl.Replace("/RecordID/" + itemID, string.Empty)
                : CPViewPage.Request.RawUrl);
        }

        public virtual void ActionCancel()
        {
            CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl.Replace("Add.aspx", "Index.aspx"));
        }

        public virtual void ActionConfig()
        {
            //Core.Web.Cache.Clear(DataService);
            Core.Web.Cache.Clear();
            //Core.Web.Storage.Clear();

            //thong bao
            CPViewPage.SetMessage("Xóa cache thành công.");
            CPViewPage.RefreshPage();
        }

        public virtual void ActionCopy(int id)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            var item = DataService.GetByID(id);

            item.ID = 0;
            item.Name += " - (Copy)";

            DataService.Save(item);

            //thong bao
            CPViewPage.SetMessage("Sao chép thành công.");
            CPViewPage.RefreshPage();
        }

        public void ActionAutoSave(int recordID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            dynamic item;

            if (recordID > 0)
            {
                item = DataService.GetByID(recordID);
                if (item != null)
                {
                    TryUpdateModel(item);

                    DataService.Save(item);
                }
            }
            else
            {
                item = DataEntity;
                TryUpdateModel(item);

                var json = new JavaScriptSerializer().Serialize(item);
                //var json = JsonConvert.SerializeObject(item);
                Cookies.SetValue(DataService.ToString(), json, true);
            }

            //thong bao
            CPViewPage.SetMessage("Tự động lưu nội dung.");
            CPViewPage.RefreshPage();
        }

        public virtual void ActionPublish(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID] IN (" + Array.ToString(arrID) + ")", "@Activity", 1);

            //thong bao
            CPViewPage.SetMessage("Đã duyệt thành công.");
            CPViewPage.RefreshPage();
        }

        public virtual void ActionUnPublish(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID] IN (" + Array.ToString(arrID) + ")", "@Activity", 0);

            //thong bao
            CPViewPage.SetMessage("Đã bỏ duyệt thành công.");
            CPViewPage.RefreshPage();
        }

        public virtual void ActionDelete(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Delete)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Delete("[ID] IN (" + Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        public virtual void ActionSaveOrder(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            for (var i = 0; i < arrID.Length - 1; i = i + 2)
            {
                DataService.Update("[ID]=" + arrID[i], "@Order", arrID[i + 1]);
            }

            //thong bao
            CPViewPage.SetMessage("Đã sắp xếp thành công.");
            CPViewPage.RefreshPage();
        }

        public virtual void ActionPublishGX(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID]=" + arrID[0], "@Activity", arrID[1]);

            //thong bao
            CPViewPage.SetMessage(arrID[1] == 0 ? "Đã bỏ duyệt thành công." : "Đã duyệt thành công.");
            CPViewPage.RefreshPage();
        }
    }

    public class DefaultModel
    {
        private int _pageIndex;

        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value - 1;
        }

        private int _PageSize = 20;
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        public int TotalRecord { get; set; }

        public string Sort { get; set; }

        public int RecordID { get; set; }
    }
}