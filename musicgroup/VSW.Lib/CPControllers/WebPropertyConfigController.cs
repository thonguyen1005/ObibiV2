using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Thông số thuộc tính",
        Description = "Quản lý - Thông số thuộc tính",
        Code = "WebPropertyConfig",
        Access = 31,
        Order = 2,
        ShowInMenu = true,
        CssClass = "pencil")]
    public class WebPropertyConfigController : CPController
    {
        public WebPropertyConfigController()
        {
            //khoi tao Service
            DataService = WebPropertyConfigService.Instance;
            DataEntity = new WebPropertyConfigEntity();
            CheckPermissions = true;
        }
        Dictionary<WebPropertyEntity, List<WebPropertyEntity>> dicItem;
        public void ActionIndex(WebPropertyConfigModel model)
        {
            var menu = WebMenuService.Instance.GetByID_Cache(model.MenuID);
            if (menu != null && menu.PropertyID > 0)
            {

                var listItem = WebPropertyService.Instance.CreateQuery()
                                                    .Select(o => new { o.Name, o.Code, o.ID })
                                                    .Where(o => o.Activity == true && o.ParentID == menu.PropertyID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList_Cache();
                if (listItem != null)
                {

                    dicItem = new Dictionary<WebPropertyEntity, List<WebPropertyEntity>>();

                    for (var i = 0; listItem != null && i < listItem.Count; i++)
                    {
                        var listChildItem = WebPropertyService.Instance.CreateQuery()
                                                            .Select(o => new { o.ID, o.Name, o.Code })
                                                            .Where(o => o.Activity == true && o.ParentID == listItem[i].ID)
                                                            .OrderByAsc(o => o.Order)
                                                            .ToList_Cache();

                        if (listChildItem == null)
                            continue;

                        for (var j = 0; j < listChildItem.Count; j++)
                        {
                            listChildItem[j].Count = ModPropertyService.Instance.GetCount(listChildItem[j].ID, model.MenuID);
                        }

                        listChildItem.RemoveAll(o => o.Count < 1);

                        dicItem[listItem[i]] = listChildItem;
                    }

                }
            }
            var listProduct = ModProductService.Instance.CreateQuery()
                       .Select(o => o.BrandID)
                       .Where(o => o.Activity == true)
                       .WhereIn(model.MenuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForCP("Product", model.MenuID, model.LangID))
                       .GroupBy(" [BrandID] ").ToList_Cache();

            List<WebMenuEntity> listBrand = null;
            if (listProduct != null)
            {
                var listB = listProduct.Select(o => o.BrandID).ToList().ToArray();
                string joinB = VSW.Core.Global.Array.ToString(listB);
                listBrand = WebMenuService.Instance.CreateQuery()
                                .WhereIn(o => o.ID, joinB)
                                .ToList();
            }
            ViewBag.Data = dicItem;
            ViewBag.ListBrand = listBrand;
            ViewBag.Model = model;
        }
        public void ActionApply(WebPropertyConfigModel model)
        {
            if (ValidSave(model))
            {
                CPViewPage.SetMessage("Đã cập nhật thành công.");
                CPViewPage.RefreshPage();
            }
        }
        private bool ValidSave(WebPropertyConfigModel model)
        {

            ViewBag.Model = model;
            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            //kiem tra chuyen muc
            if (model.MenuID < 1)
                CPViewPage.Message.ListMessage.Add("Chọn chuyên mục.");

            try
            {

                //cap nhat thuoc tinh
                //WebPropertyConfigService.Instance.Delete(o => o.MenuID == model.MenuID);
                int count = CPViewPage.PageViewState.GetValue("CountProperty").ToInt();
                for (int i = 0; i <= count; i++)
                {
                    if (!CPViewPage.PageViewState.Exists("ID" + i)) continue;
                    int ID = CPViewPage.PageViewState.GetValue("ID" + i).ToInt();
                    int BrandID = CPViewPage.PageViewState.GetValue("BrandID" + i).ToInt();
                    int PropertyID = CPViewPage.PageViewState.GetValue("PropertyID" + i).ToInt();
                    bool ShowMenu = CPViewPage.PageViewState.GetValue("ShowMenu" + i).ToBool();
                    bool ShowFilterFast = CPViewPage.PageViewState.GetValue("ShowFilterFast" + i).ToBool();
                    bool ShowBreadCrumb = CPViewPage.PageViewState.GetValue("ShowBreadCrumb" + i).ToBool();
                    if (PropertyID < 1 && BrandID < 1) continue;
                    if (BrandID > 0)
                    {
                        var config = WebPropertyConfigService.Instance.CreateQuery()
                                    .Where(o => o.MenuID == model.MenuID && o.BrandID == BrandID)
                                    .ToSingle_Cache();
                        if (config == null)
                        {
                            config = new WebPropertyConfigEntity();
                            config.ID = 0;
                            config.PropertyID = 0;
                            config.BrandID = BrandID;
                            config.MenuID = model.MenuID;
                        }
                        config.IsShowMenu = ShowMenu;
                        config.ShowFilterFast = ShowFilterFast;
                        config.ShowBreadCrumb = ShowBreadCrumb;
                        WebPropertyConfigService.Instance.Save(config);
                    }
                    else if (PropertyID > 0)
                    {
                        var config = WebPropertyConfigService.Instance.CreateQuery()
                                    .Where(o => o.MenuID == model.MenuID && o.PropertyID == PropertyID)
                                    .ToSingle_Cache();

                        if (config == null)
                        {
                            config = new WebPropertyConfigEntity();
                            config.ID = 0;
                            config.BrandID = 0;
                            config.PropertyID = PropertyID;
                            config.MenuID = model.MenuID;

                            var parent = WebPropertyService.Instance.GetByID_Cache(PropertyID);
                            if (parent != null) config.PropertyParentID = parent.ParentID;
                        }
                        config.IsShowMenu = ShowMenu;
                        config.ShowFilterFast = ShowFilterFast;
                        config.ShowBreadCrumb = ShowBreadCrumb;
                        WebPropertyConfigService.Instance.Save(config);
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
    }

    public class WebPropertyConfigModel : DefaultModel
    {
        public int LangID { get; set; } = 1;
        public int MenuID { get; set; }
    }
}