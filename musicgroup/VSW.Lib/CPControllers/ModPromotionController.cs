using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Khuyến mại",
        Description = "Quản lý - Khuyến mại",
        Code = "ModPromotion",
        Access = 31,
        Order = 2,
        ShowInMenu = true,
        CssClass = "gift")]
    public class ModPromotionController : CPController
    {
        public ModPromotionController()
        {
            //khoi tao Service
            DataService = ModPromotionService.Instance;
            DataEntity = new ModPromotionEntity();
            CheckPermissions = true;
        }

        public void ActionIndex(ModPromotionModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = ModPromotionService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText)))
                                .Where(model.MenuID > 0, o => o.MenuID == model.MenuID)
                                .Where(model.BrandID > 0, o => o.BrandID == model.BrandID)
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModPromotionModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModPromotionService.Instance.GetByID(model.RecordID);

            }
            else
            {
                _item = new ModPromotionEntity
                {
                    MenuID = model.MenuID,
                    Created = DateTime.Now,
                    Order = GetMaxOrder(),
                    Activity = CPViewPage.UserPermissions.Approve
                };
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModPromotionModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModPromotionModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModPromotionModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            //xoa cleanurl
            ModCleanURLService.Instance.Delete("[Value] IN (" + Core.Global.Array.ToString(arrID) + ") AND [Type]='Promotion'");

            //xoa Promotion
            DataService.Delete("[ID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        #region private func

        private ModPromotionEntity _item;

        private bool ValidSave(ModPromotionModel model)
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

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            try
            {
                //save
                ModPromotionService.Instance.Save(_item);
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
            return ModPromotionService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    public class ModPromotionModel : DefaultModel
    {
        public int LangID { get; set; } = 1;
        public int MenuID { get; set; }
        public int BrandID { get; set; }
        public string SearchText { get; set; }
    }
}