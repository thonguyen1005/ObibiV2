using System;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Showroom",
        Description = "Quản lý - Showroom",
        Code = "ModAddress",
        Access = 31,
        Order = 4,
        ShowInMenu = true,
        CssClass = "university")]
    public class ModAddressController : CPController
    {
        public ModAddressController()
        {
            //khoi tao Service
            DataService = ModAddressService.Instance;
            DataEntity = new ModAddressEntity();
            CheckPermissions = true;
        }

        public void ActionIndex(ModAddressModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = ModAddressService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(model.SearchText)))
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModAddressModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModAddressService.Instance.GetByID(model.RecordID);

                //khoi tao gia tri mac dinh khi update
                if (_item.Updated <= DateTime.MinValue) _item.Updated = DateTime.Now;
            }
            else
            {
                _item = new ModAddressEntity
                {
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxOrder(),
                    Activity = CPViewPage.UserPermissions.Approve
                };

                //khoi tao gia tri mac dinh khi insert
                var json = Cookies.GetValue(DataService.ToString(), true);
                if (!string.IsNullOrEmpty(json))
                    _item = new JavaScriptSerializer().Deserialize<ModAddressEntity>(json);
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModAddressModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModAddressModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModAddressModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        #region private func

        private ModAddressEntity _item;

        private bool ValidSave(ModAddressModel model)
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

            if (string.IsNullOrEmpty(_item.Code)) _item.Code = Data.GetCode(_item.Name);

            try
            {
                //save
                ModAddressService.Instance.Save(_item);
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
            return ModAddressService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }

        #endregion private func
    }

    //day la 1 class model. chi can dinh nghia 1 property trung ten voi:
    //- POST: cac thuoc tinh name cua the input, select
    //- GET: cac params tren URL
    //vi du: GET (LangID - co 1 thuoc tinh la LangID (mac dinh = 1), se doc dc param LangID tu URL vi du http://localhost:40002/CP/SysMenu/Index.aspx/LangID/2)
    public class ModAddressModel : DefaultModel
    {
        public int LangID { get; set; } = 1;
        public string SearchText { get; set; }
    }
}
