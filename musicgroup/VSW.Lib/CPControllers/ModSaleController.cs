using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Mã giảm giá",
        Description = "Quản lý  - Mã giảm giá",
        Code = "ModSale",
        Access = 31,
        Order = 11,
        ShowInMenu = true,
        CssClass = "crosshairs")]
    public class ModSaleController : CPController
    {
        public ModSaleController()
        {
            //khoi tao Service
            DataService = ModSaleService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModSaleModel model)
        {
            var FromDate = DateTime.MinValue;
            var ToDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(model.FromDate))
            {
                FromDate = VSW.Core.Global.Convert.ToDateTime(model.FromDate + " 00:00:00");
            }
            if (!string.IsNullOrEmpty(model.ToDate))
            {
                ToDate = VSW.Core.Global.Convert.ToDateTime(model.ToDate + " 23:59:59");
            }
            // tao danh sach
            var dbQuery = ModSaleService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(model.SearchText)))
                                .Where(FromDate > DateTime.MinValue, o => o.DateStart >= FromDate)
                                .Where(ToDate > DateTime.MinValue, o => o.DateStart <= ToDate)
                                .Take(model.PageSize)
                                .OrderByDesc(o => o.Published)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModSaleModel model)
        {
            if (model.RecordID > 0)
            {
                item = ModSaleService.Instance.GetByID(model.RecordID);

                // khoi tao gia tri mac dinh khi update
            }
            else
            {
                item = new ModSaleEntity();
                item.Published = DateTime.Now;
                item.DateStart = DateTime.Now;
                item.DateEnd = DateTime.Now.AddDays(7);
                item.Activity = CPViewPage.UserPermissions.Approve;
            }

            ViewBag.Data = item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModSaleModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModSaleModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, item.ID);
        }

        public void ActionSaveNew(ModSaleModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, item.ID);
        }

        #region private func

        private ModSaleEntity item = null;

        private bool ValidSave(ModSaleModel model)
        {
            TryUpdateModel(item);

            //chong hack
            item.ID = model.RecordID;

            ViewBag.Data = item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
            //kiem tra ten 
            if (item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập tên chương trình khuyến mại.");
            if (item.Code.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập mã giảm giá.");
            if (item.DateStart == DateTime.MinValue)
                CPViewPage.Message.ListMessage.Add("Nhập ngày bắt đầu.");
            if (item.DateEnd == DateTime.MinValue)
                CPViewPage.Message.ListMessage.Add("Nhập ngày kết thúc.");

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            if (CPViewPage.Message.ListMessage.Count == 0)
            {
                try
                {
                    //save
                    ModSaleService.Instance.Save(item);
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

    public class ModSaleModel : DefaultModel
    {
        private int _LangID = 1;
        public int LangID
        {
            get { return _LangID; }
            set { _LangID = value; }
        }
        public string SearchText { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
