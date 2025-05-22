using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;

namespace VSW.Lib.CPControllers
{
    public class FormProductSizeController : CPController
    {
        public FormProductSizeController()
        {
            //khoi tao Service
            DataService = ModProductSizeService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModProductSizeModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort, "[Order] ASC");

            // tao danh sach
            var dbQuery = ModProductSizeService.Instance.CreateQuery()
                                            .Where(model.ProductID > 0, o => o.ProductID == model.ProductID)
                                            .Take(model.PageSize)
                                            .OrderBy(orderBy)
                                            .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModProductSizeModel model)
        {
            if (model.RecordID > 0)
            {
                item = ModProductSizeService.Instance.GetByID(model.RecordID);
            }
            else
            {
                item = new ModProductSizeEntity();
                item.ProductID = model.ProductID;
                item.Activity = CPViewPage.UserPermissions.Approve;
            }
            ViewBag.Data = item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModProductSizeModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModProductSizeModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, item.ID);
        }

        public void ActionSaveNew(ModProductSizeModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, item.ID);
        }
        public void ActionSavePrice(ModProductSizeModel model)
        {
            if (model.cid != null && model.cid.Length > 0)
            {
                for (int i = 0; i < model.cid.Length; i++)
                {
                    var productsize = ModProductSizeService.Instance.GetByID(model.cid[i]);
                    if (productsize == null) continue;
                    productsize.Price = CPViewPage.PageViewState.GetValue("Price" + model.cid[i]).ToLong();
                    productsize.Price2 = CPViewPage.PageViewState.GetValue("Price2" + model.cid[i]).ToLong();
                    productsize.PricePromotion = CPViewPage.PageViewState.GetValue("PricePromotion" + model.cid[i]).ToLong();
                    productsize.Weight = CPViewPage.PageViewState.GetValue("Weight" + model.cid[i]).ToLong();

                    ModProductSizeService.Instance.Save(productsize);
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

        private ModProductSizeEntity item = null;
        private bool ValidSave(ModProductSizeModel model)
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

            if (CPViewPage.Message.ListMessage.Count == 0)
            {
                try
                {
                    ModProductSizeService.Instance.Save(item);
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

    public class ModProductSizeModel : DefaultModel
    {
        private int _LangID = 1;
        public int LangID
        {
            get { return _LangID; }
            set { _LangID = value; }
        }

        public int ProductID { get; set; }
        public int SizeID { get; set; }
        public int[] cid { get; set; }
    }
}

