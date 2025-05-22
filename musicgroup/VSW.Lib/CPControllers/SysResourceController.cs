using System;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class SysResourceController : CPController
    {
        public SysResourceController()
        {
            //khoi tao Service
            DataService = WebResourceService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(SysResourceModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort, "[Code]");

            //tao danh sach
            var dbQuery = WebResourceService.Instance.CreateQuery()
                                .Where(o => o.LangID == model.LangID)
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => o.Code.Contains(model.SearchText))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionUpload(SysResourceModel model)
        {
            CPViewPage.Script("Redirect", "VSWRedirect('Import')");
        }

        public void ActionImport(SysResourceModel model)
        {
            ViewBag.Model = model;
        }

        public void ActionAdd(SysResourceModel model)
        {
            _item = model.RecordID > 0 ? WebResourceService.Instance.GetByID(model.RecordID) : new WebResourceEntity { LangID = model.LangID };

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(SysResourceModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(SysResourceModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(SysResourceModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionCancel()
        {
            CPViewPage.Response.Redirect(CPViewPage.Request.RawUrl.Replace("Add.aspx", "Index.aspx")
                .Replace("Import.aspx", "Index.aspx"));
        }

        #region private func

        private WebResourceEntity _item;

        private bool ValidSave(SysResourceModel model)
        {
            if (!string.IsNullOrEmpty(model.Resource))
            {
                var ArrItem = model.Resource.Split('\n');
                foreach (var t in ArrItem)
                {
                    if (string.IsNullOrEmpty(t.Trim()) || t.StartsWith("//"))
                        continue;

                    var index = t.IndexOf('=');
                    if (index < 0)
                        continue;

                    var key = t.Substring(0, index).Trim();
                    var value = t.Substring(index + 1).Trim();

                    _item = WebResourceService.Instance.CreateQuery()
                                                .Where(o => o.LangID == model.LangID && o.Code == key)
                                                .ToSingle();

                    if (_item == null)
                        _item = new WebResourceEntity { ID = 0, LangID = model.LangID, Code = key };

                    _item.Value = value;

                    WebResourceService.Instance.Save(_item);
                }

                return true;
            }
            else
            {
                TryUpdateModel(_item);

                ViewBag.Data = _item;
                ViewBag.Model = model;

                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

                //kiem tra ma
                if (_item.Code.Trim() == string.Empty)
                    CPViewPage.Message.ListMessage.Add("Nhập mã.");

                //kiem tra ton tai
                if (model.RecordID < 1 && WebResourceService.Instance.CP_HasExists(_item.Code, _item.LangID))
                    CPViewPage.Message.ListMessage.Add("Mã đã tồn tại.");

                if (CPViewPage.Message.ListMessage.Count != 0) return false;

                try
                {
                    //save
                    WebResourceService.Instance.Save(_item);
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
        
        #endregion private func
    }

    public class SysResourceModel : DefaultModel
    {
        private int _langID = 1;

        public int LangID
        {
            get => _langID;
            set => _langID = value;
        }

        public string SearchText { get; set; }
        public string Resource { get; set; }
    }
}