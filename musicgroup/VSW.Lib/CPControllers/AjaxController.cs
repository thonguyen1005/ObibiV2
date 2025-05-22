using Aspose.Cells;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VSW.Core.Global;
using VSW.Core.MVC;
using VSW.Lib.Global;
using VSW.Lib.Global.ListItem;
using VSW.Lib.Models;
using VSW.Lib.MVC;
using Setting = VSW.Core.Web.Setting;

namespace VSW.Lib.CPControllers
{
    public class AjaxController : CPController
    {
        public void ActionIndex()
        {
        }

        public void ActionGetChild(GetChildModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var listItem = WebMenuService.Instance.GetByParentID_Cache(model.ParentID);

            string s = @"<option value=""0"">- chọn -</option>";
            if (model.ParentID > 0)
            {
                for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    s += @"<option value=""" + listItem[i].ID + @""" " + (listItem[i].ID == model.SelectedID ? @"selected=""selected""" : @"") + @">" + listItem[i].Name + @"</option>";
                }
                Json.Instance.Html = s;
            }
            ViewBag.Model = model;

            Json.Create();
        }

        public void ActionGetChild2(GetChildModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var listItem = WebMenuService.Instance.GetByParentID_Cache(model.ParentID);

            string s = @"<option value=""0"">- chọn -</option>";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<option value=""" + listItem[i].ID + @""" " + (listItem[i].ID == model.SelectedID ? @"selected=""selected""" : @"") + @">" + listItem[i].Name + @"</option>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;

            Json.Create();
        }

        public void ActionSiteGetPage(SiteGetPageModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var listPage = List.GetList(SysPageService.Instance, model.LangID);
            string s = "";
            for (var i = 0; listPage != null && i < listPage.Count; i++)
            {
                s += "<option " + (model.PageID.ToString() == listPage[i].Value ? "selected" : "") + " value='" + listPage[i].Value + "'>&nbsp; " + listPage[i].Name + "</option>";
            }
            Json.Instance.Params = s;
            Json.Create();
        }

        public void ActionTemplateGetCustom(int templateID)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            Json.Instance.Html = SysTemplateService.Instance.GetByID(templateID).Custom;

            Json.Create();
        }

        public void ActionPageGetCustom(int pageID)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            Json.Instance.Html = SysPageService.Instance.GetByID(pageID).Custom;

            Json.Create();
        }

        public void ActionPageGetControl(PageGetControlModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = Json.Instance.Extension = "";//chu set mac dinh gia tri rong cho no. thi no se cong don
            try
            {
                if (!string.IsNullOrEmpty(model.ModuleCode))
                {
                    SysPageEntity currentPage = null;
                    var currentModule = SysModuleService.Instance.VSW_Core_GetByCode(model.ModuleCode);

                    if (model.PageID > 0)
                        currentPage = SysPageService.Instance.GetByID(model.PageID);

                    if (currentModule != null)
                    {
                        var currentObject = new Class(currentModule.ModuleType);

                        var filePath = (File.Exists(CPViewPage.Server.MapPath("~/Views/Design/" + currentModule.Code + ".ascx")) ?
                            "~/Views/Design/" + currentModule.Code + ".ascx" : "~/" + Setting.Sys_CPDir + "/Design/EditModule.ascx");

                        var sHtml = Core.Web.Utils.GetHtmlControl(CPViewPage, filePath,
                                            "CurrentObject", currentObject,
                                            "CurrentPage", currentPage,
                                            "CurrentModule", currentModule,
                                            "LangID", model.LangID);

                        if (currentObject.ExistsField("MenuID"))
                        {
                            var fieldInfo = currentObject.GetFieldInfo("MenuID");
                            var attributes = fieldInfo.GetCustomAttributes(typeof(PropertyInfo), true);
                            if (attributes.GetLength(0) > 0)
                            {
                                var propertyInfo = (PropertyInfo)attributes[0];
                                var listMenu = List.GetListByText(propertyInfo.Value.ToString());

                                var menuType = List.FindByName(listMenu, "Type").Value;

                                listMenu = List.GetList(WebMenuService.Instance, model.LangID, menuType);
                                listMenu.Insert(0, new Item(string.Empty, string.Empty));

                                for (var j = 1; j < listMenu.Count; j++)
                                {
                                    if (string.IsNullOrEmpty(listMenu[j].Name)) continue;

                                    Json.Instance.Params += "<option " + (currentPage != null && currentPage.MenuID.ToString() == listMenu[j].Value ? "selected" : "") + " value='" + listMenu[j].Value + "'>&nbsp; " + listMenu[j].Name + "</option>";
                                }
                            }
                        }

                        if (currentObject.ExistsField("BrandID"))
                        {
                            var fieldInfo = currentObject.GetFieldInfo("BrandID");
                            var attributes = fieldInfo.GetCustomAttributes(typeof(PropertyInfo), true);
                            if (attributes.GetLength(0) > 0)
                            {
                                var propertyInfo = (PropertyInfo)attributes[0];
                                var listMenu = List.GetListByText(propertyInfo.Value.ToString());

                                var menuType = List.FindByName(listMenu, "Type").Value;

                                listMenu = List.GetList(WebMenuService.Instance, model.LangID, menuType);
                                listMenu.Insert(0, new Item(string.Empty, string.Empty));

                                for (var j = 1; j < listMenu.Count; j++)
                                {
                                    if (string.IsNullOrEmpty(listMenu[j].Name)) continue;

                                    Json.Instance.Js += "<option " + (currentPage != null && currentPage.BrandID.ToString() == listMenu[j].Value ? "selected" : "") + " value='" + listMenu[j].Value + "'>&nbsp; " + listMenu[j].Name + "</option>";
                                }
                            }
                        }

                        Json.Instance.Html = sHtml.Replace("{CPPath}", CPViewPage.CPPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.Write(ex);
            }

            Json.Create();
        }

        public void ActionGetProperties(GetPropertiesModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = Json.Instance.Extension = "";

            var menu = WebMenuService.Instance.GetByID(model.MenuID);
            if (menu == null) { Json.Instance.Html = ""; Json.Create(); }

            var property = WebPropertyService.Instance.GetByID(menu.PropertyID);
            if (property == null) { Json.Instance.Html = ""; Json.Create(); }

            var listProperty = WebPropertyService.Instance.CreateQuery()
                                        .Select(o => new { o.ID, o.Name, o.Code, o.Multiple })
                                        .Where(o => o.Activity == true && o.ParentID == property.ID)
                                        .OrderByAsc(o => o.Order)
                                        .ToList_Cache();
            Json.Instance.Html = "";
            string sall = "";
            for (var i = 0; listProperty != null && i < listProperty.Count; i++)
            {
                var listPropertyValue = WebPropertyService.Instance.CreateQuery()
                                                    .Select(o => new { o.ID, o.Name })
                                                    .Where(o => o.ParentID == listProperty[i].ID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList_Cache();

                if (listProperty[i].Multiple)
                {
                    string s = @"<div class=""form-group row"">
                                                <label class=""col-md-3 col-form-label text-right"">" + listProperty[i].Name + @":</label>
                                                <div class=""col-md-8"">
                                                    <div class=""checkbox-list"" id=""Property_" + listProperty[i].ID + @""" > ";

                    for (var j = 0; listPropertyValue != null && j < listPropertyValue.Count; j++)
                    {
                        var exists = model.ProductID > 0 && ModPropertyService.Instance.GetByID(model.ProductID, model.MenuID, listProperty[i].ID, listPropertyValue[j].ID) != null;
                        s += @"        <label class=""itemCheckBox itemCheckBox-sm"">
                                                            <input type=""checkbox"" " + (exists ? "checked" : "") + @" name=""Property_" + listProperty[i].ID + @"_" + listPropertyValue[j].ID + @""" id=""Property_" + listProperty[i].ID + @"_" + listPropertyValue[j].ID + @""" value=""" + listPropertyValue[j].ID + @""" />
                                                            <i class=""check-box""></i>
                                                            <span>" + listPropertyValue[j].Name + @"</span>
                                                        </label>";
                    }

                    s += @"        </div>
                                                </div>
                                                <div class=""col-md-1"">
                                                    <a href=""javascript:AddProperty("+ listProperty[i].Multiple.ToString().ToLower() + @",'Property_" + listProperty[i].ID + @"', "+ listProperty[i].ID + @")"" class=""text-primary""><i class=""fa fa-plus""></i></a>
                                                </div>
                                            </div>";
                    sall += s;
                }
                else
                {
                    string s = @" <div class=""form-group row"">
                                                <label class=""col-md-3 col-form-label text-right"">" + listProperty[i].Name + @":</label>
                                                <div class=""col-md-8"">
                                                    <select class=""form-control select2"" name=""Property_" + listProperty[i].ID + @""" id=""Property_" + listProperty[i].ID + @""">
                                                        <option value=""0"">Root</option>";

                    for (var j = 0; listPropertyValue != null && j < listPropertyValue.Count; j++)
                    {
                        var exists = model.ProductID > 0 && ModPropertyService.Instance.GetByID(model.ProductID, model.MenuID, listProperty[i].ID, listPropertyValue[j].ID) != null;
                        s += @"        <option value=""" + listPropertyValue[j].ID + @""" " + (exists ? "selected" : "") + @">" + listPropertyValue[j].Name + @"</option>";
                    }

                    s += @"        </select>
                                                </div>
                                                <div class=""col-md-1"">
                                                    <a href=""javascript:AddProperty(" + listProperty[i].Multiple.ToString().ToLower() + @",'Property_" + listProperty[i].ID + @"', " + listProperty[i].ID + @")"" class=""text-primary""><i class=""fa fa-plus""></i></a>
                                                </div>
                                            </div>";
                    sall += s;
                }
            }

            Json.Instance.Html = sall;
            Json.Create();
        }

        #region File Product
        public void ActionAddFile(FileModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }

            if (ModProductFileService.Instance.Exists(product.ID, model.File))
            {
                Json.Instance.Params = "Ảnh đã tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductFileService.Instance.Save(new ModProductFileEntity()
            {
                ID = 0,
                ProductID = product.ID,
                File = model.File,
                Order = GetMaxProductFileOrder(product.ID),
                Activity = true
            });

            var listItem = ModProductFileService.Instance.CreateQuery()
                                            .Where(o => o.ProductID == product.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();
            Json.Instance.Html = "";
            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            " + Utils.GetMedia(listItem[i].File, 80, 80) + @"

                                            <a href=""javascript:void(0)"" onclick=""deleteFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDeleteFile(FileModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var item = ModProductFileService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.File == model.File)
                                        .ToSingle();

            if (item != null)
                ModProductFileService.Instance.Delete(item);

            var listItem = ModProductFileService.Instance.CreateQuery()
                                            .Where(o => o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            " + Utils.GetMedia(listItem[i].File, 80, 80) + @"

                                            <a href=""javascript:void(0)"" onclick=""deleteFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUpFile(FileModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductFileService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.File == model.File)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductFileService.Instance.CreateQuery()
                                            .Where(o => o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModProductFileService.Instance.Save(item, o => o.Order);

            itemPrev.Order = (order == 0 ? (item.Order + 1) : order);
            ModProductFileService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModProductFileService.Instance.CreateQuery()
                                            .Where(o => o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            " + Utils.GetMedia(listItem[i].File, 80, 80) + @"

                                            <a href=""javascript:void(0)"" onclick=""deleteFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDownFile(FileModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductFileService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.File == model.File)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductFileService.Instance.CreateQuery()
                                            .Where(o => o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModProductFileService.Instance.Save(item, o => o.Order);

            itemNext.Order = (item.Order == 0 ? (item.Order + 1) : order);
            ModProductFileService.Instance.Save(itemNext, o => o.Order);

            listItem = ModProductFileService.Instance.CreateQuery()
                                            .Where(o => o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            " + Utils.GetMedia(listItem[i].File, 80, 80) + @"

                                            <a href=""javascript:void(0)"" onclick=""deleteFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFile('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionChoseFile(FileProductSizeModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (model.FileID < 1)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            else if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            else if (model.ProductSizeID < 1)
            {
                Json.Instance.Params = "Kích cỡ màu sắc sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductFileService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ID == model.FileID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            item.ProductSizeID += (!string.IsNullOrEmpty(item.ProductSizeID) ? "," : "") + model.ProductSizeID.ToString();
            ModProductFileService.Instance.Save(item, o => o.ProductSizeID);
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUnChoseFile(FileProductSizeModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (model.FileID < 1)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            else if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            else if (model.ProductSizeID < 1)
            {
                Json.Instance.Params = "Kích cỡ màu sắc sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductFileService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ID == model.FileID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            string size = "";
            if (!string.IsNullOrEmpty(item.ProductSizeID))
            {
                string[] arr = item.ProductSizeID.Split(',');
                for (int i = 0; i < arr.Length; i++)
                {
                    if (string.IsNullOrEmpty(arr[i].Trim())) continue;
                    if (arr[i].Trim() == model.ProductSizeID.ToString()) continue;
                    size += (!string.IsNullOrEmpty(size) ? "," : "") + arr[i].Trim();
                }
            }
            item.ProductSizeID = size;
            ModProductFileService.Instance.Save(item, o => o.ProductSizeID);
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionChoseFileAvatar(FileProductSizeModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (model.FileID < 1)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            else if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            else if (model.ProductSizeID < 1)
            {
                Json.Instance.Params = "Kích cỡ màu sắc sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductFileService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ID == model.FileID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            if (!item.ProductSizeID.Contains(model.ProductSizeID.ToString()))
                item.ProductSizeID += (!string.IsNullOrEmpty(item.ProductSizeID) ? "," : "") + model.ProductSizeID.ToString();
            item.IsAvatar = true;
            ModProductFileService.Instance.Save(item, o => new { o.ProductSizeID, o.IsAvatar });
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUnChoseFileAvatar(FileProductSizeModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (model.FileID < 1)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            else if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            else if (model.ProductSizeID < 1)
            {
                Json.Instance.Params = "Kích cỡ màu sắc sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductFileService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ID == model.FileID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            item.IsAvatar = false;
            ModProductFileService.Instance.Save(item, o => new { o.IsAvatar });
            ViewBag.Model = model;

            Json.Create();
        }
        #endregion File Product

        #region Gift
        public void ActionAddGift(GiftModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }

            if (model.GiftID < 1)
            {
                Json.Instance.Params = "Quà tặng không tồn tại. Chọn quà tặng khác";
                ViewBag.Model = model;

                Json.Create();
            }

            var gift = ModGiftService.Instance.GetByID(model.GiftID);
            if (gift == null)
            {
                Json.Instance.Params = "Quà tặng không tồn tại. Chọn quà tặng khác";
                ViewBag.Model = model;

                Json.Create();
            }

            if (ModProductGiftService.Instance.Exists(product.ID, model.GiftID))
            {
                Json.Instance.Params = "Quà tặng đã tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductGiftService.Instance.Save(new ModProductGiftEntity()
            {
                ID = 0,
                ProductID = product.ID,
                GiftID = model.GiftID,
                Name = gift.Name,
                File = gift.File,
                Price = gift.Price,
                Order = GetMaxProductGiftOrder(product.ID),
                Activity = true
            });

            var listItem = ModProductGiftService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == product.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />
                                            <b>" + listItem[i].Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionAddGiftMulti(GiftModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }
            var ListProductOtherID = CPViewPage.PageViewState.GetValue("ListProductOtherID[]").ToInts();
            if (ListProductOtherID.Length > 0)
            {
                for (int i = 0; i < ListProductOtherID.Length; i++)
                {
                    int GiftID = ListProductOtherID[i];
                    if (GiftID < 1) continue;

                    var gift = ModGiftService.Instance.GetByID(GiftID);
                    if (gift == null)
                    {
                        continue;
                    }

                    if (ModProductGiftService.Instance.Exists(product.ID, GiftID))
                    {
                        continue;
                    }

                    ModProductGiftService.Instance.Save(new ModProductGiftEntity()
                    {
                        ID = 0,
                        ProductID = product.ID,
                        GiftID = GiftID,
                        Name = gift.Name,
                        File = gift.File,
                        Price = gift.Price,
                        Order = GetMaxProductGiftOrder(product.ID),
                        Activity = true
                    });
                }
            }

            var listItem = ModProductGiftService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == product.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />
                                            <b>" + listItem[i].Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDeleteGift(GiftModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            var item = ModProductGiftService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.GiftID == model.GiftID)
                                        .ToSingle();

            if (item != null)
                ModProductGiftService.Instance.Delete(item);

            var listItem = ModProductGiftService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />
                                            <b>" + listItem[i].Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUpGift(GiftModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductGiftService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.GiftID == model.GiftID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Quà tặng không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductGiftService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModProductGiftService.Instance.Save(item, o => o.Order);

            itemPrev.Order = order;
            ModProductGiftService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModProductGiftService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />
                                            <b>" + listItem[i].Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDownGift(GiftModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductGiftService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.GiftID == model.GiftID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Quà tặng không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductGiftService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModProductGiftService.Instance.Save(item, o => o.Order);

            itemNext.Order = order;
            ModProductGiftService.Instance.Save(itemNext, o => o.Order);

            listItem = ModProductGiftService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />
                                            <b>" + listItem[i].Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downGift('" + listItem[i].GiftID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        #endregion Gift

        #region private MaxOrder
        private static int GetMaxNewsOrder()
        {
            return ModNewsService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxNewsProductOrder(int newsID)
        {
            return ModNewsProductService.Instance.CreateQuery()
                    .Where(o => o.NewsID == newsID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxProductOrder()
        {
            return ModProductService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxProductFileOrder(int productID)
        {
            return ModProductFileService.Instance.CreateQuery()
                    .Where(o => o.ProductID == productID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxProductGiftOrder(int productID)
        {
            return ModProductGiftService.Instance.CreateQuery()
                    .Where(o => o.ProductID == productID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxProductOtherOrder(int productID)
        {
            return ModProductOtherService.Instance.CreateQuery()
                    .Where(o => o.ProductID == productID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        private static int GetMaxProductOldOrder(int productID)
        {
            return ModProductOldService.Instance.CreateQuery()
                    .Where(o => o.ProductID == productID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        #endregion private MaxOrder

        #region File User
        public void ActionAddFileUser(FileUserModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên thành viên trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModWebUserEntity user = null;
            if (model.WebUserID > 0)
                user = ModWebUserService.Instance.GetByID(model.WebUserID);

            if (user == null)
            {
                //luu san pham
                user = new ModWebUserEntity()
                {
                    ID = 0,
                    Name = model.Name,
                    UserName = model.UserName,
                    Activity = false
                };

                ModWebUserService.Instance.Save(user);

                Json.Instance.Js = user.ID.ToString();
            }

            if (ModWebUserFileService.Instance.Exists(user.ID, model.File))
            {
                Json.Instance.Params = "Ảnh đã tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModWebUserFileService.Instance.Save(new ModWebUserFileEntity()
            {
                ID = 0,
                WebUserID = user.ID,
                File = model.File,
                Order = GetMaxWebUserFileOrder(user.ID),
                Activity = true
            });

            var listItem = ModWebUserFileService.Instance.CreateQuery()
                                            .Where(o => o.WebUserID == user.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string files = "";
            if (listItem == null || listItem.Count == 0)
            {
                files = "";
            }
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                files += (!string.IsNullOrEmpty(files) ? "|" : "") + listItem[i].File;
            }
            user.Files = files;
            ModWebUserService.Instance.Save(user, o => o.Files);
            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />

                                            <a href=""javascript:void(0)"" onclick=""deleteFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDeleteFileUser(FileUserModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var item = ModWebUserFileService.Instance.CreateQuery()
                                        .Where(o => o.WebUserID == model.WebUserID && o.File == model.File)
                                        .ToSingle();

            if (item != null)
                ModWebUserFileService.Instance.Delete(item);

            var listItem = ModWebUserFileService.Instance.CreateQuery()
                                            .Where(o => o.WebUserID == model.WebUserID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string files = "";
            if (listItem == null || listItem.Count == 0)
            {
                files = "";
            }
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                files += (!string.IsNullOrEmpty(files) ? "|" : "") + listItem[i].File;
            }
            var user = ModWebUserService.Instance.GetByID(model.WebUserID);
            user.Files = files;
            ModWebUserService.Instance.Save(user, o => o.Files);

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />

                                            <a href=""javascript:void(0)"" onclick=""deleteFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUpFileUser(FileUserModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (model.WebUserID < 1)
            {
                Json.Instance.Params = "Thành viên không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModWebUserFileService.Instance.CreateQuery()
                                        .Where(o => o.WebUserID == model.WebUserID && o.File == model.File)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModWebUserFileService.Instance.CreateQuery()
                                            .Where(o => o.WebUserID == model.WebUserID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModWebUserFileService.Instance.Save(item, o => o.Order);

            itemPrev.Order = order;
            ModWebUserFileService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModWebUserFileService.Instance.CreateQuery()
                                            .Where(o => o.WebUserID == model.WebUserID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string files = "";
            if (listItem == null || listItem.Count == 0)
            {
                files = "";
            }
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                files += (!string.IsNullOrEmpty(files) ? "|" : "") + listItem[i].File;
            }
            var user = ModWebUserService.Instance.GetByID(model.WebUserID);
            user.Files = files;
            ModWebUserService.Instance.Save(user, o => o.Files);

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />

                                            <a href=""javascript:void(0)"" onclick=""deleteFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDownFileUser(FileUserModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            if (model.WebUserID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModWebUserFileService.Instance.CreateQuery()
                                        .Where(o => o.WebUserID == model.WebUserID && o.File == model.File)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModWebUserFileService.Instance.CreateQuery()
                                            .Where(o => o.WebUserID == model.WebUserID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModWebUserFileService.Instance.Save(item, o => o.Order);

            itemNext.Order = order;
            ModWebUserFileService.Instance.Save(itemNext, o => o.Order);

            listItem = ModWebUserFileService.Instance.CreateQuery()
                                            .Where(o => o.WebUserID == model.WebUserID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string files = "";
            if (listItem == null || listItem.Count == 0)
            {
                files = "";
            }
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                files += (!string.IsNullOrEmpty(files) ? "|" : "") + listItem[i].File;
            }
            var user = ModWebUserService.Instance.GetByID(model.WebUserID);
            user.Files = files;
            ModWebUserService.Instance.Save(user, o => o.Files);

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li>
                                            <img src=""" + listItem[i].File + @""" />

                                            <a href=""javascript:void(0)"" onclick=""deleteFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFileUser('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        private static int GetMaxWebUserFileOrder(int WebUserID)
        {
            return ModWebUserFileService.Instance.CreateQuery()
                    .Where(o => o.WebUserID == WebUserID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        #endregion File User

        #region ProductOther
        public void ActionAddProductOtherMulti(ProductOtherModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }
            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }
            var ListProductOtherID = CPViewPage.PageViewState.GetValue("ListProductOtherID[]").ToInts();
            if (ListProductOtherID.Length > 0)
            {
                for (int i = 0; i < ListProductOtherID.Length; i++)
                {
                    int ProductOtherID = ListProductOtherID[i];
                    if (ProductOtherID < 1) continue;

                    var ProductOther = ModProductService.Instance.GetByID(ProductOtherID);
                    if (ProductOther == null)
                    {
                        continue;
                    }

                    if (ModProductOtherService.Instance.Exists(product.ID, ProductOtherID))
                    {
                        continue;
                    }

                    ModProductOtherService.Instance.Save(new ModProductOtherEntity()
                    {
                        ID = 0,
                        ProductID = product.ID,
                        ProductOtherID = ProductOtherID,
                        Order = GetMaxProductOtherOrder(product.ID),
                        Activity = true
                    });
                }
            }
            var listItem = ModProductOtherService.Instance.CreateQuery()
                                                    .Where(o => o.Activity == true && o.ProductID == product.ID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOther();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;
            Json.Create();
        }
        public void ActionAddProductOther(ProductOtherModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }

            if (model.ProductOtherID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại. Chọn sản phẩm khác";
                ViewBag.Model = model;

                Json.Create();
            }

            var ProductOther = ModProductService.Instance.GetByID(model.ProductOtherID);
            if (ProductOther == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại. Chọn sản phẩm khác";
                ViewBag.Model = model;

                Json.Create();
            }

            if (ModProductOtherService.Instance.Exists(product.ID, model.ProductOtherID))
            {
                Json.Instance.Params = "Sản phẩm đã tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductOtherService.Instance.Save(new ModProductOtherEntity()
            {
                ID = 0,
                ProductID = product.ID,
                ProductOtherID = model.ProductOtherID,
                Order = GetMaxProductOtherOrder(product.ID),
                Activity = true
            });

            var listItem = ModProductOtherService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == product.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOther();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDeleteProductOther(ProductOtherModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            var item = ModProductOtherService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ProductOtherID == model.ProductOtherID)
                                        .ToSingle();

            if (item != null)
                ModProductOtherService.Instance.Delete(item);

            var listItem = ModProductOtherService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOther();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUpProductOther(ProductOtherModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductOtherService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ProductOtherID == model.ProductOtherID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductOtherService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModProductOtherService.Instance.Save(item, o => o.Order);

            itemPrev.Order = order;
            ModProductOtherService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModProductOtherService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOther();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDownProductOther(ProductOtherModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductOtherService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ProductOtherID == model.ProductOtherID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductOtherService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModProductOtherService.Instance.Save(item, o => o.Order);

            itemNext.Order = order;
            ModProductOtherService.Instance.Save(itemNext, o => o.Order);

            listItem = ModProductOtherService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOther();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        #endregion ProductOther

        #region ProductOld
        public void ActionAddProductOldMulti(ProductOldModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }
            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }
            var ListProductOldID = CPViewPage.PageViewState.GetValue("ListProductOldID[]").ToInts();
            if (ListProductOldID.Length > 0)
            {
                for (int i = 0; i < ListProductOldID.Length; i++)
                {
                    int ProductOldID = ListProductOldID[i];
                    if (ProductOldID < 1) continue;

                    var ProductOld = ModProductService.Instance.GetByID(ProductOldID);
                    if (ProductOld == null)
                    {
                        continue;
                    }

                    if (ModProductOldService.Instance.Exists(product.ID, ProductOldID))
                    {
                        continue;
                    }

                    ModProductOldService.Instance.Save(new ModProductOldEntity()
                    {
                        ID = 0,
                        ProductID = product.ID,
                        ProductOldID = ProductOldID,
                        Order = GetMaxProductOldOrder(product.ID),
                        Activity = true
                    });
                }
            }
            var listItem = ModProductOldService.Instance.CreateQuery()
                                                    .Where(o => o.Activity == true && o.ProductID == product.ID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOld();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;
            Json.Create();
        }
        public void ActionAddProductOld(ProductOldModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }

            if (model.ProductOldID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại. Chọn sản phẩm khác";
                ViewBag.Model = model;

                Json.Create();
            }

            var ProductOld = ModProductService.Instance.GetByID(model.ProductOldID);
            if (ProductOld == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại. Chọn sản phẩm khác";
                ViewBag.Model = model;

                Json.Create();
            }

            if (ModProductOldService.Instance.Exists(product.ID, model.ProductOldID))
            {
                Json.Instance.Params = "Sản phẩm đã tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductOldService.Instance.Save(new ModProductOldEntity()
            {
                ID = 0,
                ProductID = product.ID,
                ProductOldID = model.ProductOldID,
                Order = GetMaxProductOldOrder(product.ID),
                Activity = true
            });

            var listItem = ModProductOldService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == product.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOld();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDeleteProductOld(ProductOldModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            var item = ModProductOldService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ProductOldID == model.ProductOldID)
                                        .ToSingle();

            if (item != null)
                ModProductOldService.Instance.Delete(item);

            var listItem = ModProductOldService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOld();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUpProductOld(ProductOldModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductOldService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ProductOldID == model.ProductOldID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductOldService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModProductOldService.Instance.Save(item, o => o.Order);

            itemPrev.Order = order;
            ModProductOldService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModProductOldService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOld();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDownProductOld(ProductOldModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductOldService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.ProductOldID == model.ProductOldID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductOldService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModProductOldService.Instance.Save(item, o => o.Order);

            itemNext.Order = order;
            ModProductOldService.Instance.Save(itemNext, o => o.Order);

            listItem = ModProductOldService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var producto = listItem[i].GetProductOld();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        #endregion ProductOld

        #region ProductVideo
        public void ActionAddProductVideoMulti(ProductVideoModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }
            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }
            var ListVideoID = CPViewPage.PageViewState.GetValue("ListVideoID[]").ToInts();
            if (ListVideoID.Length > 0)
            {
                for (int i = 0; i < ListVideoID.Length; i++)
                {
                    int VideoID = ListVideoID[i];
                    if (VideoID < 1) continue;

                    var Video = ModVideoService.Instance.GetByID(VideoID);
                    if (Video == null)
                    {
                        continue;
                    }

                    if (ModProductVideoService.Instance.Exists(product.ID, VideoID))
                    {
                        continue;
                    }

                    ModProductVideoService.Instance.Save(new ModProductVideoEntity()
                    {
                        ID = 0,
                        ProductID = product.ID,
                        VideoID = VideoID,
                        Order = GetMaxProductVideoOrder(product.ID),
                        Activity = true
                    });
                }
            }
            var listItem = ModProductVideoService.Instance.CreateQuery()
                                                    .Where(o => o.Activity == true && o.ProductID == product.ID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var video = listItem[i].GetVideo();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(video.Image) + @""" />
                                            <b>" + video.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;
            Json.Create();
        }
        public void ActionAddProductVideo(ProductVideoModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }

            if (model.VideoID < 1)
            {
                Json.Instance.Params = "Video không tồn tại. Chọn Video khác";
                ViewBag.Model = model;

                Json.Create();
            }

            var Video = ModVideoService.Instance.GetByID(model.VideoID);
            if (Video == null)
            {
                Json.Instance.Params = "Video không tồn tại. Chọn Video khác";
                ViewBag.Model = model;

                Json.Create();
            }

            if (ModProductVideoService.Instance.Exists(product.ID, model.VideoID))
            {
                Json.Instance.Params = "Sản phẩm đã tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductVideoService.Instance.Save(new ModProductVideoEntity()
            {
                ID = 0,
                ProductID = product.ID,
                VideoID = model.VideoID,
                Order = GetMaxProductVideoOrder(product.ID),
                Activity = true
            });

            var listItem = ModProductVideoService.Instance.CreateQuery()
                                                    .Where(o => o.Activity == true && o.ProductID == product.ID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var video = listItem[i].GetVideo();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(video.Image) + @""" />
                                            <b>" + video.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDeleteProductVideo(ProductVideoModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            var item = ModProductVideoService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.VideoID == model.VideoID)
                                        .ToSingle();

            if (item != null)
                ModProductVideoService.Instance.Delete(item);

            var listItem = ModProductVideoService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var video = listItem[i].GetVideo();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(video.Image) + @""" />
                                            <b>" + video.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUpProductVideo(ProductVideoModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductVideoService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.VideoID == model.VideoID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductVideoService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModProductVideoService.Instance.Save(item, o => o.Order);

            itemPrev.Order = order;
            ModProductVideoService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModProductVideoService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var video = listItem[i].GetVideo();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(video.Image) + @""" />
                                            <b>" + video.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDownProductVideo(ProductVideoModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductVideoService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.VideoID == model.VideoID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductVideoService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModProductVideoService.Instance.Save(item, o => o.Order);

            itemNext.Order = order;
            ModProductVideoService.Instance.Save(itemNext, o => o.Order);

            listItem = ModProductVideoService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var video = listItem[i].GetVideo();
                s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(video.Image) + @""" />
                                            <b>" + video.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductVideo('" + listItem[i].VideoID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        private static int GetMaxProductVideoOrder(int productID)
        {
            return ModProductVideoService.Instance.CreateQuery()
                    .Where(o => o.ProductID == productID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        #endregion ProductVideo

        #region ProductFAQ
        public void ActionAddProductFAQMulti(ProductFAQModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }
            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }
            var ListFAQID = CPViewPage.PageViewState.GetValue("ListFAQID[]").ToInts();
            if (ListFAQID.Length > 0)
            {
                for (int i = 0; i < ListFAQID.Length; i++)
                {
                    int FAQID = ListFAQID[i];
                    if (FAQID < 1) continue;

                    var FAQ = ModFAQService.Instance.GetByID(FAQID);
                    if (FAQ == null)
                    {
                        continue;
                    }

                    if (ModProductFAQService.Instance.Exists(product.ID, FAQID))
                    {
                        continue;
                    }

                    ModProductFAQService.Instance.Save(new ModProductFAQEntity()
                    {
                        ID = 0,
                        ProductID = product.ID,
                        FAQID = FAQID,
                        Order = GetMaxProductFAQOrder(product.ID),
                        Activity = true
                    });
                }
            }
            var listItem = ModProductFAQService.Instance.CreateQuery()
                                                    .Where(o => o.Activity == true && o.ProductID == product.ID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var FAQ = listItem[i].GetFAQ();
                s += @"<li>
                                            <img src=""/Content/images/hoi-dap.png"" />
                                            <b>" + FAQ.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;
            Json.Create();
        }
        public void ActionAddProductFAQ(ProductFAQModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tên sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.MenuID < 1)
            {
                Json.Instance.Params = "Chọn chuyên mục sản phẩm trước.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductEntity product = null;
            if (model.ProductID > 0)
                product = ModProductService.Instance.GetByID(model.ProductID);

            if (product == null)
            {
                //luu san pham
                product = new ModProductEntity()
                {
                    ID = 0,
                    MenuID = model.MenuID,
                    Name = model.Name,
                    Code = Data.GetCode(model.Name),
                    Published = DateTime.Now,
                    Updated = DateTime.Now,
                    Order = GetMaxProductOrder(),
                    Activity = false
                };

                ModProductService.Instance.Save(product);

                Json.Instance.Js = product.ID.ToString();
            }

            if (model.FAQID < 1)
            {
                Json.Instance.Params = "Hỏi đáp không tồn tại. Chọn Hỏi đáp khác";
                ViewBag.Model = model;

                Json.Create();
            }

            var FAQ = ModFAQService.Instance.GetByID(model.FAQID);
            if (FAQ == null)
            {
                Json.Instance.Params = "Hỏi đáp không tồn tại. Chọn Hỏi đáp khác";
                ViewBag.Model = model;

                Json.Create();
            }

            if (ModProductFAQService.Instance.Exists(product.ID, model.FAQID))
            {
                Json.Instance.Params = "Hỏi đáp đã tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            ModProductFAQService.Instance.Save(new ModProductFAQEntity()
            {
                ID = 0,
                ProductID = product.ID,
                FAQID = model.FAQID,
                Order = GetMaxProductFAQOrder(product.ID),
                Activity = true
            });

            var listItem = ModProductFAQService.Instance.CreateQuery()
                                                    .Where(o => o.Activity == true && o.ProductID == product.ID)
                                                    .OrderByAsc(o => o.Order)
                                                    .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var faq = listItem[i].GetFAQ();
                s += @"<li>
                                            <img src=""/Content/images/hoi-dap.png"" />
                                            <b>" + faq.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDeleteProductFAQ(ProductFAQModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            var item = ModProductFAQService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.FAQID == model.FAQID)
                                        .ToSingle();

            if (item != null)
                ModProductFAQService.Instance.Delete(item);

            var listItem = ModProductFAQService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var faq = listItem[i].GetFAQ();
                s += @"<li>
                                            <img src=""/Content/images/hoi-dap.png"" />
                                            <b>" + faq.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUpProductFAQ(ProductFAQModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductFAQService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.FAQID == model.FAQID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductFAQService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModProductFAQService.Instance.Save(item, o => o.Order);

            itemPrev.Order = order;
            ModProductFAQService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModProductFAQService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var faq = listItem[i].GetFAQ();
                s += @"<li>
                                            <img src=""/Content/images/hoi-dap.png"" />
                                            <b>" + faq.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDownProductFAQ(ProductFAQModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModProductFAQService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.FAQID == model.FAQID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModProductFAQService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModProductFAQService.Instance.Save(item, o => o.Order);

            itemNext.Order = order;
            ModProductFAQService.Instance.Save(itemNext, o => o.Order);

            listItem = ModProductFAQService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ProductID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var faq = listItem[i].GetFAQ();
                s += @"<li>
                                            <img src=""/Content/images/hoi-dap.png"" />
                                            <b>" + faq.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductFAQ('" + listItem[i].FAQID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;

            Json.Create();
        }
        private static int GetMaxProductFAQOrder(int productID)
        {
            return ModProductFAQService.Instance.CreateQuery()
                    .Where(o => o.ProductID == productID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        #endregion ProductFAQ

        public void ActionDeleteProductFromNews(ProductFromNewsModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            var item = ModNewsProductService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.NewsID == model.NewsID)
                                        .ToSingle();

            if (item != null)
                ModNewsProductService.Instance.Delete(item);

            var listItem = ModNewsProductService.Instance.CreateQuery()
                                          .Where(o => o.Activity == true && o.NewsID == model.NewsID)
                                          .OrderByAsc(o => o.Order)
                                          .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var product = listItem[i].GetProduct();
                s += @" <tr>
                                <td class=""text-center"">" + (i + 1) + @"</td>
                                <td>
                                    <a href=""/" + CPViewPage.CPPath + @"/ModProduct/Add.aspx/RecordID/" + listItem[i].ProductID + @""" target=""_blank"">" + product.Name + @"</a>
                                </td>
                                <td class=""text-center"">
                                    " + Utils.GetMedia(product.File, 40, 40) + @"
                                </td>
                                <td class=""text-center"">" + string.Format("{0:#,##0}", product.Price) + @"</td>
                                <td class=""text-center"">
                                    <a href=""javascript:void(0)"" onclick=""deleteProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""upProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""downProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                </td>
                            </tr>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionUpProductFromNews(ProductFromNewsModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.NewsID < 1)
            {
                Json.Instance.Params = "Bài viết không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModNewsProductService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.NewsID == model.NewsID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModNewsProductService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.NewsID == model.NewsID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModNewsProductService.Instance.Save(item, o => o.Order);

            itemPrev.Order = order;
            ModNewsProductService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModNewsProductService.Instance.CreateQuery()
                                         .Where(o => o.Activity == true && o.NewsID == model.NewsID)
                                         .OrderByAsc(o => o.Order)
                                         .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var product = listItem[i].GetProduct();
                s += @" <tr>
                                <td class=""text-center"">" + (i + 1) + @"</td>
                                <td>
                                    <a href=""/" + CPViewPage.CPPath + @"/ModProduct/Add.aspx/RecordID/" + listItem[i].ProductID + @""" target=""_blank"">" + product.Name + @"</a>
                                </td>
                                <td class=""text-center"">
                                    " + Utils.GetMedia(product.File, 40, 40) + @"
                                </td>
                                <td class=""text-center"">" + string.Format("{0:#,##0}", product.Price) + @"</td>
                                <td class=""text-center"">
                                    <a href=""javascript:void(0)"" onclick=""deleteProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""upProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""downProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                </td>
                            </tr>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionDownProductFromNews(ProductFromNewsModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.NewsID < 1)
            {
                Json.Instance.Params = "Bài viết không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var item = ModNewsProductService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.NewsID == model.NewsID)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModNewsProductService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.NewsID == model.NewsID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModNewsProductService.Instance.Save(item, o => o.Order);

            itemNext.Order = order;
            ModNewsProductService.Instance.Save(itemNext, o => o.Order);

            listItem = ModNewsProductService.Instance.CreateQuery()
                                         .Where(o => o.Activity == true && o.NewsID == model.NewsID)
                                         .OrderByAsc(o => o.Order)
                                         .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var product = listItem[i].GetProduct();
                s += @" <tr>
                                <td class=""text-center"">" + (i + 1) + @"</td>
                                <td>
                                    <a href=""/" + CPViewPage.CPPath + @"/ModProduct/Add.aspx/RecordID/" + listItem[i].ProductID + @""" target=""_blank"">" + product.Name + @"</a>
                                </td>
                                <td class=""text-center"">
                                    " + Utils.GetMedia(product.File, 40, 40) + @"
                                </td>
                                <td class=""text-center"">" + string.Format("{0:#,##0}", product.Price) + @"</td>
                                <td class=""text-center"">
                                    <a href=""javascript:void(0)"" onclick=""deleteProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""upProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""downProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                </td>
                            </tr>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }

        public void ActionDeleteProductFromPromotion(ProductFromPromotionModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            var item = ModPromotionProductService.Instance.CreateQuery()
                                        .Where(o => o.ProductID == model.ProductID && o.PromotionID == model.PromotionID)
                                        .ToSingle();

            if (item != null)
                ModPromotionProductService.Instance.Delete(item);

            var listItem = ModPromotionProductService.Instance.CreateQuery()
                                          .Where(o => o.PromotionID == model.PromotionID)
                                          .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                var product = listItem[i].GetProduct();
                s += @" <tr>
                                <td class=""text-center"">" + (i + 1) + @"</td>
                                <td>
                                    <a href=""/" + CPViewPage.CPPath + @"/ModProduct/Add.aspx/RecordID/" + listItem[i].ProductID + @""" target=""_blank"">" + product.Name + @"</a>
                                </td>
                                <td class=""text-center"">
                                    " + Utils.GetMedia(product.File, 40, 40) + @"
                                </td>
                                <td class=""text-center"">" + string.Format("{0:#,##0}", product.Price) + @"</td>
                                <td class=""text-center"">
                                    <a href=""javascript:void(0)"" onclick=""deleteProductFromPromotion('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                </td>
                            </tr>";
            }
            Json.Instance.Html = s;

            ViewBag.Model = model;

            Json.Create();
        }
        #region private func

        private void EndResponse()
        {
            var s = @"<Xml>
                      <Item>
                        <Html><![CDATA[" + ajaxModel.Html + @"]]></Html>
                        <Params><![CDATA[" + ajaxModel.Params + @"]]></Params>
                        <JS><![CDATA[" + ajaxModel.JS + @"]]></JS>
                      </Item>
                    </Xml>";
            CPViewPage.Response.ContentType = "text/xml";
            CPViewPage.Response.Write(s);
            CPViewPage.Response.End();
        }

        private AjaxModel ajaxModel = new AjaxModel() { Params = "", Html = "", JS = "" };

        #endregion private func
        public void ActionAddProductMulti(ProductModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (!CPViewPage.PageViewState.Exists("ListProductID[]"))
            {
                Json.Instance.Params = "Chọn sản phẩm.";
                ViewBag.Model = model;

                Json.Create();
            }
            var ListProductID = CPViewPage.PageViewState.GetValue("ListProductID[]").ToInts();
            if (ListProductID.Length < 1)
            {
                Json.Instance.Params = "Chọn sản phẩm.";
                ViewBag.Model = model;

                Json.Create();
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tiêu đề.";
                ViewBag.Model = model;

                Json.Create();
            }

            if (model.ID < 1)
            {
                if (model.Type == "News")
                {
                    //luu bai viet
                    var itemNews = new ModNewsEntity()
                    {
                        ID = 0,
                        MenuID = model.MenuID,
                        Name = model.Name,
                        Code = Data.GetCode(model.Name),
                        Published = DateTime.Now,
                        Updated = DateTime.Now,
                        Order = GetMaxNewsOrder(),
                        Activity = false
                    };

                    ModNewsService.Instance.Save(itemNews);
                    model.ID = itemNews.ID;
                    Json.Instance.Js = itemNews.ID.ToString();
                }
                else
                {
                    //luu san pham
                    var itemProduct = new ModProductEntity()
                    {
                        ID = 0,
                        MenuID = model.MenuID,
                        Name = model.Name,
                        Code = Data.GetCode(model.Name),
                        Published = DateTime.Now,
                        Updated = DateTime.Now,
                        Order = GetMaxProductOrder(),
                        Activity = false
                    };

                    ModProductService.Instance.Save(itemProduct);
                    model.ID = itemProduct.ID;
                    Json.Instance.Js = itemProduct.ID.ToString();
                }
            }

            if (model.Type == "News")
            {
                for (int i = 0; i < ListProductID.Length; i++)
                {
                    int ProductID = ListProductID[i];
                    if (ProductID < 1) continue;

                    var p = ModProductService.Instance.GetByID_Cache(ProductID);
                    if (p == null)
                    {
                        continue;
                    }

                    if (ModNewsProductService.Instance.Exists(model.ID, ProductID))
                    {
                        continue;
                    }

                    ModNewsProductService.Instance.Save(new ModNewsProductEntity()
                    {
                        ID = 0,
                        NewsID = model.ID,
                        ProductID = ProductID,
                        Order = GetMaxNewsProductOrder(model.ID),
                        Activity = true
                    });
                }

                var listItem = ModNewsProductService.Instance.CreateQuery()
                                           .Where(o => o.Activity == true && o.NewsID == model.ID)
                                           .OrderByAsc(o => o.Order)
                                           .ToList();

                string s = "";
                for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var product = listItem[i].GetProduct();
                    s += @" <tr>
                                <td class=""text-center"">" + (i + 1) + @"</td>
                                <td>
                                    <a href=""/" + CPViewPage.CPPath + @"/ModProduct/Add.aspx/RecordID/" + listItem[i].ProductID + @""" target=""_blank"">" + product.Name + @"</a>
                                </td>
                                <td class=""text-center"">
                                    " + Utils.GetMedia(product.File, 40, 40) + @"
                                </td>
                                <td class=""text-center"">" + string.Format("{0:#,##0}", product.Price) + @"</td>
                                <td class=""text-center"">
                                    <a href=""javascript:void(0)"" onclick=""deleteProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""upProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""downProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                </td>
                            </tr>";
                }
                Json.Instance.Html = s;
            }
            else if (model.Type == "ProductOld")
            {
                for (int i = 0; i < ListProductID.Length; i++)
                {
                    int ProductID = ListProductID[i];
                    if (ProductID < 1) continue;

                    var p = ModProductService.Instance.GetByID_Cache(ProductID);
                    if (p == null)
                    {
                        continue;
                    }

                    if (ModProductOldService.Instance.Exists(model.ID, ProductID))
                    {
                        continue;
                    }

                    ModProductOldService.Instance.Save(new ModProductOldEntity()
                    {
                        ID = 0,
                        ProductID = model.ID,
                        ProductOldID = ProductID,
                        Order = GetMaxProductOldOrder(model.ID),
                        Activity = true
                    });
                }

                var listItem = ModProductOldService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

                string s = "";
                for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var producto = listItem[i].GetProductOld();
                    s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
                }
            }
            else if (model.Type == "ProductOther")
            {
                for (int i = 0; i < ListProductID.Length; i++)
                {
                    int ProductID = ListProductID[i];
                    if (ProductID < 1) continue;

                    var p = ModProductService.Instance.GetByID_Cache(ProductID);
                    if (p == null)
                    {
                        continue;
                    }

                    if (ModProductOtherService.Instance.Exists(model.ID, ProductID))
                    {
                        continue;
                    }

                    ModProductOtherService.Instance.Save(new ModProductOtherEntity()
                    {
                        ID = 0,
                        ProductID = model.ID,
                        ProductOtherID = ProductID,
                        Order = GetMaxProductOtherOrder(model.ID),
                        Activity = true
                    });
                }

                var listItem = ModProductOtherService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

                string s = "";
                for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var producto = listItem[i].GetProductOther();
                    s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
                }
            }
            ViewBag.Model = model;
            Json.Create();
        }
        public void ActionAddProduct(ProductModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Chọn sản phẩm.";
                ViewBag.Model = model;

                Json.Create();
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                Json.Instance.Params = "Nhập tiêu đề.";
                ViewBag.Model = model;

                Json.Create();
            }
            if (model.ID < 1)
            {
                if (model.Type == "News")
                {
                    //luu bai viet
                    var itemNews = new ModNewsEntity()
                    {
                        ID = 0,
                        MenuID = model.MenuID,
                        Name = model.Name,
                        Code = Data.GetCode(model.Name),
                        Published = DateTime.Now,
                        Updated = DateTime.Now,
                        Order = GetMaxNewsOrder(),
                        Activity = false
                    };

                    ModNewsService.Instance.Save(itemNews);
                    model.ID = itemNews.ID;
                    Json.Instance.Js = itemNews.ID.ToString();
                }
                else if (model.Type == "Promotion")
                {
                    //luu bai viet
                    var itemPromotion = new ModPromotionEntity()
                    {
                        ID = 0,
                        Name = model.Name,
                        Created = DateTime.Now,
                        Order = GetMaxNewsOrder(),
                        Activity = false
                    };

                    ModPromotionService.Instance.Save(itemPromotion);
                    model.ID = itemPromotion.ID;
                    Json.Instance.Js = itemPromotion.ID.ToString();
                }
                else
                {
                    //luu san pham
                    var itemProduct = new ModProductEntity()
                    {
                        ID = 0,
                        MenuID = model.MenuID,
                        Name = model.Name,
                        Code = Data.GetCode(model.Name),
                        Published = DateTime.Now,
                        Updated = DateTime.Now,
                        Order = GetMaxProductOrder(),
                        Activity = false
                    };

                    ModProductService.Instance.Save(itemProduct);
                    model.ID = itemProduct.ID;
                    Json.Instance.Js = itemProduct.ID.ToString();
                }
            }

            if (model.Type == "News")
            {

                if (ModNewsProductService.Instance.Exists(model.ID, model.ProductID))
                {
                    Json.Instance.Params = "Sản phẩm đã tồn tại.";
                    ViewBag.Model = model;

                    Json.Create();
                }

                ModNewsProductService.Instance.Save(new ModNewsProductEntity()
                {
                    ID = 0,
                    NewsID = model.ID,
                    ProductID = model.ProductID,
                    Order = GetMaxNewsProductOrder(model.ID),
                    Activity = true
                });

                var listItem = ModNewsProductService.Instance.CreateQuery()
                                           .Where(o => o.Activity == true && o.NewsID == model.ID)
                                           .OrderByAsc(o => o.Order)
                                           .ToList();

                string s = "";
                for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var product = listItem[i].GetProduct();
                    s += @" <tr>
                                <td class=""text-center"">" + (i + 1) + @"</td>
                                <td>
                                    <a href=""/" + CPViewPage.CPPath + @"/ModProduct/Add.aspx/RecordID/" + listItem[i].ProductID + @""" target=""_blank"">" + product.Name + @"</a>
                                </td>
                                <td class=""text-center"">
                                    " + Utils.GetMedia(product.File, 40, 40) + @"
                                </td>
                                <td class=""text-center"">" + string.Format("{0:#,##0}", product.Price) + @"</td>
                                <td class=""text-center"">
                                    <a href=""javascript:void(0)"" onclick=""deleteProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""upProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                    <a href=""javascript:void(0)"" onclick=""downProductFromNews('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                </td>
                            </tr>";
                }
                Json.Instance.Html = s;
            }
            else if (model.Type == "Promotion")
            {

                if (ModPromotionProductService.Instance.Exists(model.ID, model.ProductID))
                {
                    Json.Instance.Params = "Sản phẩm đã tồn tại.";
                    ViewBag.Model = model;

                    Json.Create();
                }

                ModPromotionProductService.Instance.Save(new ModPromotionProductEntity()
                {
                    ID = 0,
                    PromotionID = model.ID,
                    ProductID = model.ProductID
                });

                var listItem = ModPromotionProductService.Instance.CreateQuery()
                                           .Where(o => o.PromotionID == model.ID)
                                           .ToList();

                string s = "";
                for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var product = listItem[i].GetProduct();
                    s += @" <tr>
                                <td class=""text-center"">" + (i + 1) + @"</td>
                                <td>
                                    <a href=""/" + CPViewPage.CPPath + @"/ModProduct/Add.aspx/RecordID/" + listItem[i].ProductID + @""" target=""_blank"">" + product.Name + @"</a>
                                </td>
                                <td class=""text-center"">
                                    " + Utils.GetMedia(product.File, 40, 40) + @"
                                </td>
                                <td class=""text-center"">" + string.Format("{0:#,##0}", product.Price) + @"</td>
                                <td class=""text-center"">
                                    <a href=""javascript:void(0)"" onclick=""deleteProductFromPromotion('" + listItem[i].ProductID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                </td>
                            </tr>";
                }
                Json.Instance.Html = s;
            }
            else if (model.Type == "ProductOld")
            {
                if (ModProductOldService.Instance.Exists(model.ID, model.ProductID))
                {
                    Json.Instance.Params = "Sản phẩm đã tồn tại.";
                    ViewBag.Model = model;

                    Json.Create();
                }

                ModProductOldService.Instance.Save(new ModProductOldEntity()
                {
                    ID = 0,
                    ProductID = model.ID,
                    ProductOldID = model.ProductID,
                    Order = GetMaxProductOldOrder(model.ID),
                    Activity = true
                });
                var listItem = ModProductOldService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

                string s = "";
                for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var producto = listItem[i].GetProductOld();
                    s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOld('" + listItem[i].ProductOldID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
                }
            }
            else if (model.Type == "ProductOther")
            {
                if (ModProductOtherService.Instance.Exists(model.ID, model.ProductID))
                {
                    Json.Instance.Params = "Sản phẩm đã tồn tại.";
                    ViewBag.Model = model;

                    Json.Create();
                }

                ModProductOtherService.Instance.Save(new ModProductOtherEntity()
                {
                    ID = 0,
                    ProductID = model.ID,
                    ProductOtherID = model.ProductID,
                    Order = GetMaxProductOtherOrder(model.ID),
                    Activity = true
                });
                var listItem = ModProductOtherService.Instance.CreateQuery()
                                            .Where(o => o.Activity == true && o.ProductID == model.ID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

                string s = "";
                for (var i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var producto = listItem[i].GetProductOther();
                    s += @"<li>
                                            <img src=""" + Utils.GetUrlFile(producto.File) + @""" />
                                            <b>" + producto.Name + @"</b>
                                            <a href=""javascript:void(0)"" onclick=""deleteProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downProductOther('" + listItem[i].ProductOtherID + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                        </li>";
                }
            }
            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionLoadTemp(ProductTempModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (string.IsNullOrEmpty(model.File)) model.File = "TempBienTap";
            var path = CPViewPage.Server.MapPath("~/CP/Template/" + model.File + ".html");
            if (File.Exists(path))
            {
                Json.Instance.Html = File.ReadText(path);
            }

            Json.Create();
        }
        public void ActionLoadThongSo()
        {
            var listFile = Utils.GetFile(CPViewPage.Request.Files, "File");
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            if (listFile == null) Json.Instance.Params = "Chưa có file nào được chọn";
            else
            {
                var file = listFile[0];
                string fileName = file.FileName;
                string fileExtension = System.IO.Path.GetExtension(fileName);
                if (!fileExtension.Contains("xls")) Json.Instance.Params = "File không đúng định dạng";
                else
                {
                    var workbook = new Workbook();
                    workbook.Open(file.InputStream);
                    var worksheet = workbook.Worksheets[0];
                    if (worksheet == null || worksheet.Cells.MaxRow < 1)
                    {
                        Json.Instance.Params = "File không hợp lệ. Yêu cầu file Excel!";
                    }
                    else
                    {
                        string s = @"<div id=""thongso"">";
                        s += @"<table>";
                        for (int i = 1; i <= worksheet.Cells.MaxRow; i++)
                        {
                            string Name = worksheet.Cells[i, 0].StringValue.Trim();
                            string Value = worksheet.Cells[i, 1].StringValue.Trim();
                            if (string.IsNullOrEmpty(Name)) break;
                            s += "<tr>";
                            s += "<td>" + Name + ":</td>";
                            s += "<td>" + (string.IsNullOrEmpty(Value) ? "" : Value) + "</td>";
                            s += "</tr>";
                        }
                        s += @"</table>";
                        s += @"</div>";
                        Json.Instance.Html = s;
                    }

                    file.InputStream.Close();
                    file.InputStream.Dispose();
                }
            }


            Json.Create();
        }
        public void ActionLoadSchema(SchemaModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            var path = CPViewPage.Server.MapPath("~/Schema.json");
            string content = "";
            if (File.Exists(path))
            {
                content = File.ReadText(path);
            }
            if (model.LoaiData == 1)
            {
                var CurrentLang = SysLangService.Instance.VSW_Core_GetByID(1);
                var ResourceService = new IniSqlResourceService(CurrentLang);
                bool flag4 = ResourceService != null && CPViewPage.CurrentLang != null;
                if (flag4)
                {
                    MatchCollection matchCollection = new Regex("\\{RS:[\\w]+\\}").Matches(content);
                    for (int i = 0; i < matchCollection.Count; i++)
                    {
                        string text2 = matchCollection[i].Value.Replace("{RS:", string.Empty).Replace("}", string.Empty);
                        content = content.Replace("{RS:" + text2 + "}", ResourceService.VSW_Core_GetByCode(text2, string.Empty));
                    }
                }
            }
            else
            {
                MatchCollection matchCollection = new Regex("\\{RS:[\\w]+\\}").Matches(content);
                for (int i = 0; i < matchCollection.Count; i++)
                {
                    string text2 = matchCollection[i].Value.Replace("{RS:", string.Empty).Replace("}", string.Empty);
                    content = content.Replace("{RS:" + text2 + "}", "");
                }
            }
            Json.Instance.Html = content;
            Json.Create();
        }
        public void ActionDeleteFileVote(FileVoteModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var item = ModVoteFileService.Instance.CreateQuery()
                                        .Where(o => o.CommentID == model.CommentID && o.File == model.File)
                                        .ToSingle();

            if (item != null)
                ModVoteFileService.Instance.Delete(item);

            var listItem = ModVoteFileService.Instance.CreateQuery()
                                            .Where(o => o.CommentID == model.CommentID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li data-src=""" + Utils.GetUrlFile(listItem[i].File) + @""">
                                            <img src=""" + Utils.GetUrlFile(listItem[i].File) + @""" data-src=""" + Utils.GetUrlFile(listItem[i].File) + @""" />
                                            <a href=""javascript:void(0)"" onclick=""deleteFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                            <label style=""display: inline-block; text-align: center; width: 40px;"" class=""itemCheckBox itemCheckBox-sm"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chọn hiển thị"">
                                                <input type=""checkbox"" onclick=""updateFileVote('" + listItem[i].File + @"', this)"" " + (listItem[i].Show ? "checked" : "") + @" value=""1"">
                                                <i class=""check-box""></i>
                                            </label>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;
            Json.Create();
        }
        public void ActionUpFileVote(FileVoteModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var item = ModVoteFileService.Instance.CreateQuery()
                                        .Where(o => o.CommentID == model.CommentID && o.File == model.File)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModVoteFileService.Instance.CreateQuery()
                                            .Where(o => o.CommentID == model.CommentID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexPrev = index == 0 ? listItem.Count - 1 : (index - 1);

            var itemPrev = listItem[indexPrev];

            var order = item.Order;

            item.Order = itemPrev.Order;
            ModVoteFileService.Instance.Save(item, o => o.Order);

            itemPrev.Order = order;
            ModVoteFileService.Instance.Save(itemPrev, o => o.Order);

            listItem = ModVoteFileService.Instance.CreateQuery()
                                            .Where(o => o.CommentID == model.CommentID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li data-src=""" + Utils.GetUrlFile(listItem[i].File) + @""">
                                            <img src=""" + Utils.GetUrlFile(listItem[i].File) + @""" data-src=""" + Utils.GetUrlFile(listItem[i].File) + @""" />
                                            <a href=""javascript:void(0)"" onclick=""deleteFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                            <label style=""display: inline-block; text-align: center; width: 40px;"" class=""itemCheckBox itemCheckBox-sm"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chọn hiển thị"">
                                                <input type=""checkbox"" onclick=""updateFileVote('" + listItem[i].File + @"', this)"" " + (listItem[i].Show ? "checked" : "") + @" value=""1"">
                                                <i class=""check-box""></i>
                                            </label>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;
            Json.Create();
        }
        public void ActionDownFileVote(FileVoteModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var item = ModVoteFileService.Instance.CreateQuery()
                                        .Where(o => o.CommentID == model.CommentID && o.File == model.File)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }

            var listItem = ModVoteFileService.Instance.CreateQuery()
                                            .Where(o => o.CommentID == model.CommentID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();

            var index = listItem.FindIndex(o => o.ID == item.ID);
            var indexNext = index == listItem.Count - 1 ? 0 : (index + 1);

            var itemNext = listItem[indexNext];

            var order = item.Order;

            item.Order = itemNext.Order;
            ModVoteFileService.Instance.Save(item, o => o.Order);

            itemNext.Order = order;
            ModVoteFileService.Instance.Save(itemNext, o => o.Order);

            listItem = ModVoteFileService.Instance.CreateQuery()
                                            .Where(o => o.CommentID == model.CommentID)
                                            .OrderByAsc(o => o.Order)
                                            .ToList();


            string s = "";
            for (var i = 0; listItem != null && i < listItem.Count; i++)
            {
                s += @"<li data-src=""" + Utils.GetUrlFile(listItem[i].File) + @""">
                                            <img src=""" + Utils.GetUrlFile(listItem[i].File) + @""" data-src=""" + Utils.GetUrlFile(listItem[i].File) + @""" />
                                            <a href=""javascript:void(0)"" onclick=""deleteFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Xóa""><i class=""fa fa-ban""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""upFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển lên trên""><i class=""fa fa-arrow-up""></i></a>
                                            <a href=""javascript:void(0)"" onclick=""downFileVote('" + listItem[i].File + @"')"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chuyển xuống dưới""><i class=""fa fa-arrow-down""></i></a>
                                            <label style=""display: inline-block; text-align: center; width: 40px;"" class=""itemCheckBox itemCheckBox-sm"" data-toggle=""tooltip"" data-placement=""bottom"" data-original-title=""Chọn hiển thị"">
                                                <input type=""checkbox"" onclick=""updateFileVote('" + listItem[i].File + @"', this)"" " + (listItem[i].Show ? "checked" : "") + @" value=""1"">
                                                <i class=""check-box""></i>
                                            </label>
                                        </li>";
            }
            Json.Instance.Html = s;
            ViewBag.Model = model;
            Json.Create();
        }
        public void ActionUpdateFileVote(FileVoteModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var item = ModVoteFileService.Instance.CreateQuery()
                                        .Where(o => o.CommentID == model.CommentID && o.File == model.File)
                                        .ToSingle();
            if (item == null)
            {
                Json.Instance.Params = "Ảnh không tồn tại.";
                ViewBag.Model = model;

                Json.Create();
            }
            item.Show = model.Status;
            ModVoteFileService.Instance.Save(item, o => o.Show);
            ViewBag.Model = model;
            Json.Create();
        }

        public void ActionLoadOptionMenu(LoadOptionMenuModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";
            
            if(model.Type == "Brand")
            {
                Json.Instance.Html = @"<option value=""0"">Root</option>" + Utils.ShowDdlMenuByType(model.Type, 1, model.Value);
            }
            else
            {
                Json.Instance.Html = @"<option value=""0"">Root</option>" + Utils.ShowDdlMenuByType(model.Type, 1, model.Value);
            }

            Json.Create();
        }

        public void ActionUpdateProperty(UpdatePropertyModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = "";

            var menu = WebMenuService.Instance.GetByID_Cache(model.MenuID);
            if(menu != null && model.Value > 0)
            {
                menu.PropertyID = model.Value;
                WebMenuService.Instance.Save(menu, o => o.PropertyID);
            }
            else
            {
                Json.Instance.Params = "Lỗi dữ liệu cập nhật.";
            }

            Json.Create();
        }
    }

    public class GetChildModel
    {
        public int ParentID { get; set; }
        public int SelectedID { get; set; }
    }

    public class PageGetControlModel
    {
        public int LangID { get; set; }
        public int PageID { get; set; }
        public string ModuleCode { get; set; }
    }

    public class SiteGetPageModel
    {
        public int LangID { get; set; }
        public int PageID { get; set; }
    }

    public class GetPropertiesModel
    {
        public int MenuID { get; set; }
        public int LangID { get; set; }
        public int ProductID { get; set; }
    }

    public class GetSizesModel
    {
        public int SizeID { get; set; }
        public int LangID { get; set; }
        public int ProductID { get; set; }
        public string Sizes { get; set; }
    }

    public class GetColorsModel
    {
        public int ColorID { get; set; }
        public int LangID { get; set; }
        public int ProductID { get; set; }
        public string Colors { get; set; }
    }
    #region File

    public class FileModel
    {
        public string Name { get; set; }
        public int MenuID { get; set; }

        public int ProductID { get; set; }
        public string File { get; set; }
    }
    public class FileProductSizeModel
    {
        public int ProductID { get; set; }
        public int FileID { get; set; }
        public int ProductSizeID { get; set; }
    }
    #endregion File

    #region Gift

    public class GiftModel
    {
        public string Name { get; set; }
        public int MenuID { get; set; }

        public int ProductID { get; set; }
        public int GiftID { get; set; }
    }

    #endregion Gift

    public class AjaxModel
    {
        public string Html { get; set; }
        public string Params { get; set; }
        public string JS { get; set; }
        public string Extension { get; set; }
    }

    #region FileUser
    public class FileUserModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public int WebUserID { get; set; }
        public string File { get; set; }
    }
    #endregion FileUser

    public class ProductModel
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int MenuID { get; set; }
        public string Type { get; set; }
    }
    #region ProductOther
    public class ProductOtherModel
    {
        public string Name { get; set; }
        public int MenuID { get; set; }
        public int ProductID { get; set; }
        public int ProductOtherID { get; set; }
    }
    #endregion ProductOther
    public class ProductOldModel
    {
        public string Name { get; set; }
        public int MenuID { get; set; }
        public int ProductID { get; set; }
        public int ProductOldID { get; set; }
    }
    public class ProductVideoModel
    {
        public string Name { get; set; }
        public int MenuID { get; set; }
        public int ProductID { get; set; }
        public int VideoID { get; set; }
    }
    public class ProductFAQModel
    {
        public string Name { get; set; }
        public int MenuID { get; set; }
        public int ProductID { get; set; }
        public int FAQID { get; set; }
    }
    public class ProductTempModel
    {
        public string File { get; set; }
    }

    public class SchemaModel
    {
        public int LoaiData { get; set; }
    }
    public class FileVoteModel
    {
        public int CommentID { get; set; }
        public string File { get; set; }
        public bool Status { get; set; }
    }
    public class ProductFromNewsModel
    {
        public int NewsID { get; set; }
        public int ProductID { get; set; }
    }

    public class ProductFromPromotionModel
    {
        public int PromotionID { get; set; }
        public int ProductID { get; set; }
    }

    public class LoadOptionMenuModel
    {
        public int Value { get; set; }
        public string Type { get; set; }
    }
    public class UpdatePropertyModel
    {
        public int Value { get; set; }
        public int MenuID { get; set; }
    }
}