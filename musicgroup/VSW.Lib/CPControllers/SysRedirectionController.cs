using System;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysRedirectionController : CPController
    {
        public SysRedirectionController()
        {
            //khoi tao Service
            DataService = WebRedirectionService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysRedirectionModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort, "[Order]");

            //tao danh sach
            var dbQuery = WebRedirectionService.Instance.CreateQuery()
                                    .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Url.Contains(model.SearchText) || o.Redirect.Contains(model.SearchText)))
                                    .Take(model.PageSize)
                                    .Skip(model.PageIndex * model.PageSize)
                                    .OrderByDesc(o => o.ID);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(SysRedirectionModel model)
        {
            _item = model.RecordID > 0 ? WebRedirectionService.Instance.GetByID(model.RecordID) : new WebRedirectionEntity();

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysRedirectionModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysRedirectionModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysRedirectionModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public void ActionImport(SysRedirectionModel model)
        {
            ViewBag.Model = model;
        }

        public override void ActionCancel()
        {
            CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl.Replace("Add.aspx", "Index.aspx")
                .Replace("Import.aspx", "Index.aspx"));
        }

        public void ActionPost(SysRedirectionModel model)
        {
            if (ValidImport(model))
            {
                CPViewPage.SetMessage("Thông tin đã cập nhật.");
                CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl.Replace("Import.aspx", "Index.aspx"));
            }
            else
                CPViewPage.SetMessage("Có lỗi xảy ra.");
        }

        #region private func

        private WebRedirectionEntity _item;

        private bool ValidImport(SysRedirectionModel model)
        {
            ViewBag.Model = model;

            if (string.IsNullOrEmpty(model.Redirections)) return false;

            var arrRedirection = model.Redirections.Split('\n');

            foreach (var redirection in arrRedirection)
            {
                if (string.IsNullOrEmpty(redirection.Trim())) continue;

                var arrTemp = redirection.Split('=');
                if (arrTemp.Length < 2) continue;

                _item = WebRedirectionService.Instance.CreateQuery()
                                    .Where(o => o.Url == arrTemp[0])
                                    .ToSingle() ?? new WebRedirectionEntity();

                _item.Url = arrTemp[0];
                _item.Redirect = arrTemp[1];

                WebRedirectionService.Instance.Save(_item);
            }

            return true;
        }

        private bool ValidSave(SysRedirectionModel model)
        {
            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra ten
            if (_item.Url.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập link cũ.");

            if (_item.Redirect.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập lin mới.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            //neu code khong duoc nhap -> tu dong tao ra khi them moi

            try
            {
                //save
                WebRedirectionService.Instance.Save(_item);
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

    public class SysRedirectionModel : DefaultModel
    {
        private int _langID = 1;

        public int LangID
        {
            get => _langID;
            set => _langID = value;
        }

        public string SearchText { get; set; }
        public string Redirections { get; set; }
    }
}