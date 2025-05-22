using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Sản phẩm",
        Description = "Quản lý - Sản phẩm",
        Code = "ModProduct",
        Access = 31,
        Order = 1,
        ShowInMenu = true,
        CssClass = "icon-16-article")]
    public class ModProductController : CPController
    {
        public ModProductController()
        {
            //khoi tao Service
            DataService = ModProductService.Instance;
            DataEntity = new ModProductEntity();

            CheckPermissions = true;
        }

        public virtual void ActionIndex(ModProductModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort, "[Order] DESC");
            string menulist = WebMenuService.Instance.GetChildIDForWeb_Cache("Product", model.MenuID, model.LangID);
            string wherein = "";
            if (!string.IsNullOrEmpty(menulist))
            {
                string[] arrmenu = menulist.Split(',');
                for (int i = 0; arrmenu != null && i < arrmenu.Length; i++)
                {
                    if (string.IsNullOrEmpty(arrmenu[i].Trim())) continue;
                    wherein += (!string.IsNullOrEmpty(wherein) ? " or " : "") + " CHARINDEX('" + arrmenu[i].Trim() + "', [MenuListID]) > 0 ";
                }
            }
            //tao danh sach
            var dbQuery = ModProductService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion })
                                    .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(Data.GetCode(model.SearchText)) || o.Model.Contains(Data.GetCode(model.SearchText)) || o.Model.Contains(model.SearchText)))
                                    .Where(model.State > 0, o => (o.State & model.State) == model.State)
                                    .Where(model.GroupMenuID > 0, o => o.GroupMenuID == model.GroupMenuID)
                                    .Where(model.UserID > 0, o => o.CPUserID == model.UserID)
                                    .Where(model.MenuID > 0, wherein)
                                    .Where(model.Status == 2, o => o.Activity == false)
                                    .Where(model.Status == 1, o => o.Activity == true)
                                    .WhereIn(model.BrandID > 0, o => o.BrandID, WebMenuService.Instance.GetChildIDForCP("Brand", model.BrandID, model.LangID))
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;

            //send to client
            //VSW.Core.SignalR.HubData<ModProductEntity>.SendData(ViewBag.Data);
        }

        public void ActionAdd(ModProductModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModProductService.Instance.GetByID(model.RecordID);
                //khoi tao gia tri mac dinh khi update
                if (_item.Updated <= DateTime.MinValue) _item.Updated = DateTime.Now;
            }
            else
            {
                //khoi tao gia tri mac dinh khi insert
                _item = new ModProductEntity
                {
                    Status = 18897,
                    Star = 5,
                    MenuID = model.MenuID,
                    BrandID = model.BrandID,
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxOrder(),
                    CPUserID = CPLogin.UserID,
                    Activity = CPViewPage.UserPermissions.Approve
                };

                var json = Cookies.GetValue(DataService.ToString(), true);
                if (!string.IsNullOrEmpty(json))
                    _item = new JavaScriptSerializer().Deserialize<ModProductEntity>(json);
            }
            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModProductModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModProductModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModProductModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }

        public override void ActionDelete(int[] arrID)
        {
            //xoa cleanurl
            ModCleanURLService.Instance.Delete("[Value] IN (" + Core.Global.Array.ToString(arrID) + ") AND [Type]='Product'");
            ModProductGiftService.Instance.Delete("[ProductID] IN (" + Core.Global.Array.ToString(arrID) + ")");
            ModProductFileService.Instance.Delete("[ProductID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //xoa Product
            DataService.Delete("[ID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }
        #region private func

        private ModProductEntity _item;

        private bool ValidSave(ModProductModel model)
        {
            TryUpdateModel(_item);
            //_item.Summary = model.Summary == null ? "" : string.Join("<br/>", model.Summary);
            //_item.Promotion = model.Promotion == null ? "" : string.Join("<br/>", model.Promotion);
            //_item.Specifications = model.Specifications == null ? "" : string.Join("<br/>", model.Specifications);

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
            //if (string.IsNullOrEmpty(_item.MenuListID))
            //    CPViewPage.Message.ListMessage.Add("Chọn chuyên mục.");

            //if (_item.KhoavantayID < 1)
            //    CPViewPage.Message.ListMessage.Add("Chọn chuyên mục khoa.");


            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            if (string.IsNullOrEmpty(_item.Code)) _item.Code = Data.GetCode(_item.Name.Replace(".", "-"));

            //cap nhat state
            _item.State = GetState(model.ArrState);
            try
            {
                _item.MenuListID += (!string.IsNullOrEmpty(_item.MenuListID) ? "," : "") + _item.MenuID;

                //save
                ModProductService.Instance.Save(_item);

                //update url
                ModCleanURLService.Instance.InsertOrUpdate(_item.Code, "Product", _item.ID, _item.MenuID, model.LangID);
                //update tag
                ModTagService.Instance.UpdateProductTag(_item.ID, _item.Tags);
                //cap nhat thuoc tinh
                ModPropertyService.Instance.Delete(o => o.ProductID == _item.ID);

                if (!string.IsNullOrEmpty(_item.File))
                {
                    if (!ModProductFileService.Instance.Exists(_item.ID, _item.File))
                    {
                        ModProductFileService.Instance.Save(new ModProductFileEntity()
                        {
                            ID = 0,
                            ProductID = _item.ID,
                            File = _item.File,
                            Order = 0,
                            Activity = true
                        });
                    }
                }

                foreach (var key in CPViewPage.PageViewState.AllKeys)
                {
                    if (!key.StartsWith("Property_")) continue;

                    var arrProperty = key.Split('_');
                    if (arrProperty.Length < 2) continue;

                    int propertyID = 0, propertyValueID = 0;

                    switch (arrProperty.Length)
                    {
                        case 2:
                            propertyID = Core.Global.Convert.ToInt(key.Replace("Property_", ""));
                            propertyValueID = Core.Global.Convert.ToInt(CPViewPage.PageViewState[key]);
                            break;

                        case 3:
                            propertyID = Core.Global.Convert.ToInt(arrProperty[1]);
                            propertyValueID = Core.Global.Convert.ToInt(CPViewPage.PageViewState[key]);
                            break;
                    }

                    if (propertyID < 1 || propertyValueID < 1) continue;

                    var property = ModPropertyService.Instance.GetByID(_item.ID, _item.MenuID, propertyID, propertyValueID);
                    if (property != null) continue;

                    property = new ModPropertyEntity
                    {
                        ID = 0,
                        ProductID = _item.ID,
                        MenuID = _item.MenuID,
                        PropertyID = propertyID,
                        PropertyValueID = propertyValueID
                    };

                    ModPropertyService.Instance.Save(property);
                    if (!string.IsNullOrEmpty(_item.MenuListID))
                    {
                        string[] arrMenu = _item.MenuListID.Split(',');
                        for (int i = 0; arrMenu != null && i < arrMenu.Length; i++)
                        {
                            if (string.IsNullOrEmpty(arrMenu[i].Trim())) continue;
                            if (arrMenu[i].Trim() == _item.MenuID.ToString()) continue;
                            int menu2 = VSW.Core.Global.Convert.ToInt(arrMenu[i].Trim());
                            var property2 = ModPropertyService.Instance.GetByID(_item.ID, menu2, propertyID, propertyValueID);
                            if (property2 != null) continue;

                            property2 = new ModPropertyEntity
                            {
                                ID = 0,
                                ProductID = _item.ID,
                                MenuID = menu2,
                                PropertyID = propertyID,
                                PropertyValueID = propertyValueID
                            };
                            ModPropertyService.Instance.Save(property2);
                        }
                    }
                }
                Core.Web.Cache.Clear(ModPropertyService.Instance);
                Core.Web.Cache.Clear(WebPropertyService.Instance);
                if (_item.IsGift)
                {
                    var gift = ModGiftService.Instance.CreateQuery().Where(o => o.ProductID == _item.ID).ToSingle_Cache();
                    if (gift == null)
                    {
                        gift = new ModGiftEntity();
                        gift.ID = 0;
                        gift.MenuID = 5;
                        gift.MenuProductID = 0;
                        gift.BrandID = 0;
                        gift.State = 0;
                        gift.Name = _item.Name;
                        gift.Code = _item.Code + "-gift";
                        gift.Model = _item.Model;
                        gift.Price = _item.Price;
                        gift.File = _item.File;
                        gift.Summary = _item.Summary;
                        gift.Content = _item.Content;
                        gift.PageTitle = _item.PageTitle;
                        gift.PageDescription = _item.PageDescription;
                        gift.PageKeywords = _item.PageKeywords;
                        gift.Published = _item.Published;
                        gift.Updated = _item.Updated;
                        gift.Order = GetMaxGiftOrder();
                        gift.Activity = _item.Activity;
                        gift.ProductID = _item.ID;
                        gift.ProductCode = _item.Code;
                        ModGiftService.Instance.Save(gift);
                        //update url
                        ModCleanURLService.Instance.InsertOrUpdate(gift.Code, "Gift", gift.ID, gift.MenuID, model.LangID);
                    }
                }
                //send to client
                //HubData<ModProductEntity>.SendData(_item);

                ModProductClassifyService.Instance.Delete("[ProductID] = " + _item.ID + "");
                ModProductClassifyDetailService.Instance.Delete("[ProductID] = " + _item.ID + "");
                ModProductClassifyDetailPriceService.Instance.Delete("[ProductID] = " + _item.ID + "");

                if (model.ProductClassify != null)
                {

                    var LstProductClassifyDetail = model.ProductClassifyParrent != null ? model.ProductClassifyParrent.ToList().GroupBy(o => o).Select(grp => grp.Key).ToList() : new List<string>();
                    var dic1 = new Dictionary<int, int>();
                    var dic2 = new Dictionary<int, int>();
                    if (LstProductClassifyDetail.Count == model.ProductClassify.Length)
                    {
                        for (int i = 0; model.ProductClassify != null && i < model.ProductClassify.Length; i++)
                        {
                            var itemProductClassify = new ModProductClassifyEntity();
                            itemProductClassify.Name = model.ProductClassify[i].Trim();
                            itemProductClassify.ProductID = _item.ID;
                            ModProductClassifyService.Instance.Save(itemProductClassify);

                            string valueProductClassifyDetail = LstProductClassifyDetail[i];
                            for (int j = 0; model.ProductClassifyValue != null && j < model.ProductClassifyValue.Length; j++)
                            {
                                if (valueProductClassifyDetail != model.ProductClassifyParrent[j])
                                {
                                    continue;
                                }

                                var itemProductClassifyDetail = new ModProductClassifyDetailEntity();
                                itemProductClassifyDetail.ProductID = _item.ID;
                                itemProductClassifyDetail.ClassifyID = itemProductClassify.ID;
                                itemProductClassifyDetail.Name = model.ProductClassifyValue[j];
                                itemProductClassifyDetail.File = model.ProductClassifyFile[j];
                                ModProductClassifyDetailService.Instance.Save(itemProductClassifyDetail);

                                if (i == 0) dic1.Add(j, itemProductClassifyDetail.ID);
                                else if (i == 1) dic2.Add(j, itemProductClassifyDetail.ID);
                            }
                        }
                    }
                    int indexValue = 0;
                    foreach (var item in dic1)
                    {
                        if (dic2.Count > 0)
                        {
                            foreach (var item2 in dic2)
                            {
                                var itemProductClassifyDetailPrice = new ModProductClassifyDetailPriceEntity();
                                itemProductClassifyDetailPrice.ProductID = _item.ID;
                                itemProductClassifyDetailPrice.ClassifyDetailID1 = item.Value;
                                itemProductClassifyDetailPrice.ClassifyDetailID2 = item2.Value;
                                itemProductClassifyDetailPrice.Price = VSW.Core.Global.Convert.ToLong(model.PriceProductClassifyDetail[indexValue].Replace(",", ""), 0);
                                itemProductClassifyDetailPrice.SoLuong = VSW.Core.Global.Convert.ToInt(model.SoLuongProductClassifyDetail[indexValue].Replace(",", ""), 0);
                                itemProductClassifyDetailPrice.Sku = model.SKUProductClassifyDetail[indexValue];
                                ModProductClassifyDetailPriceService.Instance.Save(itemProductClassifyDetailPrice);
                                indexValue++;
                            }
                        }
                        else
                        {
                            var itemProductClassifyDetailPrice = new ModProductClassifyDetailPriceEntity();
                            itemProductClassifyDetailPrice.ProductID = _item.ID;
                            itemProductClassifyDetailPrice.ClassifyDetailID1 = item.Value;
                            itemProductClassifyDetailPrice.ClassifyDetailID2 = 0;
                            itemProductClassifyDetailPrice.Price = VSW.Core.Global.Convert.ToLong(model.PriceProductClassifyDetail[indexValue].Replace(",", ""), 0);
                            itemProductClassifyDetailPrice.SoLuong = VSW.Core.Global.Convert.ToInt(model.SoLuongProductClassifyDetail[indexValue].Replace(",", ""), 0);
                            itemProductClassifyDetailPrice.Sku = model.SKUProductClassifyDetail[indexValue];
                            ModProductClassifyDetailPriceService.Instance.Save(itemProductClassifyDetailPrice);
                            indexValue++;
                        }
                    }
                }
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
            return ModProductService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxGiftOrder()
        {
            return ModGiftService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        #endregion private func
    }

    public class ModProductModel : DefaultModel
    {
        public int LangID { get; set; } = 1;

        public string SearchText { get; set; }

        public int MenuID { get; set; }
        public int BrandID { get; set; }
        public int Status { get; set; }
        public int State { get; set; }
        public string[] ArrState { get; set; }
        public int Value { get; set; }
        public int GroupMenuID { get; set; }
        public int UserID { get; set; }
        public string Type { get; set; }
        //public string[] Summary { get; set; }
        //public string[] Promotion { get; set; }
        //public string[] Specifications { get; set; }
        public string[] ProductClassify { get; set; }
        public string[] ProductClassifyValue { get; set; }
        public string[] ProductClassifyFile { get; set; }
        public string[] ProductClassifyParrent { get; set; }
        public string[] PriceProductClassifyDetail { get; set; }
        public string[] SoLuongProductClassifyDetail { get; set; }
        public string[] SKUProductClassifyDetail { get; set; }
        public string[] ClassifyDetailIndex1ProductClassifyDetail { get; set; }
        public string[] ClassifyDetailIndex2ProductClassifyDetail { get; set; }
    }
}