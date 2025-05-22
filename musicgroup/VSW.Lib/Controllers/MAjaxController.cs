using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using VSW.Core.Global;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;
using Newtonsoft.Json;
using System.Drawing;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO : MAjax", Code = "MAjax", Order = 10)]
    public class MAjaxController : Controller
    {
        public void ActionIndex()
        {
        }
        public void ActionSecurity(SecurityModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;

            Captcha.CaseSensitive = false;
            var ci = new Captcha(175, 35);

            ViewPage.Response.Clear();
            ViewPage.Response.ContentType = "image/jpeg";

            ci.Image.Save(ViewPage.Response.OutputStream, ImageFormat.Jpeg);
            ci.Dispose();

            ViewBag.Model = model;

            ViewPage.Response.End();
        }
        public void ActionGetChild(GetChildModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            if (model.ParentID > 0)
            {
                var listItem = WebMenuService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name })
                                    .Where(o => o.Activity == true && o.ParentID == model.ParentID)
                                    .OrderByAsc(o => new { o.Name, o.ID })
                                    .ToList_Cache();

                Json.Instance.Html = "";
                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    Json.Instance.Html += @"<option value=""" + listItem[i].ID + @""" " + (listItem[i].ID == model.SelectedID ? @"selected=""selected""" : @"") + @">" + listItem[i].Name + @"</option>";
                }
            }

            ViewBag.Model = model;

            Json.Create();
        }
        public void ActionAddToCart(AddToCartModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            var cart = new Cart();
            if (model.Quantity < 1) model.Quantity = 1;
            var _item = cart.Find(new CartItem() { ProductID = model.ProductID, SizeID = model.SizeID, ColorID = model.ColorID });
            if (_item == null)
            {
                cart.Add(new CartItem
                {
                    ProductID = model.ProductID,
                    Quantity = model.Quantity,
                    SizeID = model.SizeID,
                    ColorID = model.ColorID,
                });
            }
            else
            {
                _item.Quantity = model.Quantity;
            }
            cart.Save();
            long total = 0;
            string html = "";
            for (int i = 0; i < cart.Items.Count; i++)
            {
                var product = ModProductService.Instance.GetByID_Cache(cart.Items[i].ProductID);
                if (product == null) continue;
                var productPrice = ModProductClassifyDetailPriceService.Instance.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                long price = product.PriceView;
                if (productPrice != null)
                {
                    price = productPrice.Price;
                }
                total += price * cart.Items[i].Quantity;

                string url = ViewPage.GetURL(product.MenuID, product.Code);

                html += @"<li id=""item-cart-" + i + @""">
                            <div class=""dok-cart-img"">
                        <a href=""" + url + @""" title=""" + product.Name + @""">
                            <img class=""img-fluid""
                                src=""" + Utils.GetWebPFile(product.File, 4, 50, 50) + @"""
                                alt=""" + product.Name + @""">
                        </a>
                    </div>
                    <div class=""dok-cart-info"">
                        <p class=""dok-cart-title mb-0"">
                            " + product.Name + @"
                        </p>";
                if (product.PriceView > 0)
                {
                    html += @"<p class=""dok-cart-price"">" + Utils.FormatMoney(product.PriceView) + "đ</p>";
                }
                else
                {
                    html += @"<p class=""dok-cart-price"">" + VSW.Lib.Global.WebResource.GetValue("Lienhe") + "</p>";
                }
                html += @"</div>
                    <a class=""dok-cart-trash"" href=""javascript:deleteCart(" + product.ID + @", " + i + @")"">
                        <i class=""far fa-trash-alt""></i>
                    </a>
                </li>";
            }
            Json.Instance.Html = html;
            Json.Instance.Params = Utils.FormatMoney(cart.Items.Count);
            Json.Create();
        }
        public void ActionDeleteCart(DeleteCartModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            var cart = new Cart();
            var _item = cart.Items[model.Index];
            cart.Remove(_item);
            cart.Save();

            long total = 0;
            string html = "";
            for (int i = 0; i < cart.Items.Count; i++)
            {
                var product = ModProductService.Instance.GetByID_Cache(cart.Items[i].ProductID);
                if (product == null) continue;
                var productPrice = ModProductClassifyDetailPriceService.Instance.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                long price = product.PriceView;
                if (productPrice != null)
                {
                    price = productPrice.Price;
                }
                total += price * cart.Items[i].Quantity;

                string url = ViewPage.GetURL(product.MenuID, product.Code);

                html += @"<li id=""item-cart-" + i + @""">
                            <div class=""dok-cart-img"">
                        <a href=""" + url + @""" title=""" + product.Name + @""">
                            <img class=""img-fluid""
                                src=""" + Utils.GetWebPFile(product.File, 4, 50, 50) + @"""
                                alt=""" + product.Name + @""">
                        </a>
                    </div>
                    <div class=""dok-cart-info"">
                        <p class=""dok-cart-title mb-0"">
                            " + product.Name + @"
                        </p>";
                if (product.PriceView > 0)
                {
                    html += @"<p class=""dok-cart-price"">" + Utils.FormatMoney(product.PriceView) + "đ</p>";
                }
                else
                {
                    html += @"<p class=""dok-cart-price"">" + VSW.Lib.Global.WebResource.GetValue("Lienhe") + "</p>";
                }
                html += @"</div>
                    <a class=""dok-cart-trash"" href=""javascript:deleteCart(" + product.ID + @", " + i + @")"">
                        <i class=""far fa-trash-alt""></i>
                    </a>
                </li>";
            }
            Json.Instance.Html = html;
            Json.Instance.Params = Utils.FormatMoney(cart.Items.Count);

            Json.Create();
        }
        public void ActionUpdateCart(UpdateCartModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            if (model.Quantity < 1) model.Quantity = 1;
            var cart = new Cart();
            cart.Items[model.Index].Quantity = model.Quantity;
            cart.Save();
            long total = 0;
            long quantity = 0;
            for (int i = 0; i < cart.Items.Count; i++)
            {
                var product = ModProductService.Instance.GetByID_Cache(cart.Items[i].ProductID);
                if (product == null) continue;
                var productPrice = ModProductClassifyDetailPriceService.Instance.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                long price = product.PriceView;
                if (productPrice != null)
                {
                    price = productPrice.Price;
                }
                total += price * cart.Items[i].Quantity;

                quantity += cart.Items[i].Quantity;
            }
            Json.Instance.Html = total.ToString();
            Json.Instance.Js = VSW.Lib.Global.Utils.FormatMoney(total) + " ₫";

            Json.Create();
        }
        public void ActionLoadProperty(PropertyModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            string brandlist = "";
            string atrbrand = model.b;
            if (!string.IsNullOrEmpty(atrbrand))
            {
                int[] atrbrandID = Core.Global.Array.ToInts(atrbrand.Split('-'));
                for (int i = 0; i < atrbrandID.Length; i++)
                {
                    var pid = atrbrandID[i];
                    if (pid < 1) continue;
                    string sub = WebMenuService.Instance.GetChildIDForWeb_Cache("Brand", pid, ViewPage.CurrentLang.ID);
                    brandlist += (!string.IsNullOrEmpty(brandlist) ? "," : "") + sub;
                }
            }

            var dbQuery = ModProductService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion, o.Summary })
                                    .Where(o => o.Activity == true)
                                    .Where(model.state > 0, o => (o.State & model.state) == model.state);

            string menulist = WebMenuService.Instance.GetChildIDForWeb_Cache("Product", model.menuID, ViewPage.CurrentLang.ID);
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
            dbQuery.Where(model.menuID > 0, wherein)
            .Where(model.brandID > 0 && string.IsNullOrEmpty(brandlist), o => o.BrandID == model.brandID)
            .WhereIn(!string.IsNullOrEmpty(brandlist), o => o.BrandID, brandlist);

            string atr = model.c;
            if (!string.IsNullOrEmpty(atr))
            {
                int[] arrID = Core.Global.Array.ToInts(atr.Split('-'));
                for (int i = 0; i < arrID.Length; i++)
                {
                    var pid = arrID[i];
                    if (pid < 1) continue;
                    dbQuery.WhereIn(o => o.ID, ModPropertyService.Instance.CreateQuery().Select(o => o.ProductID).Distinct().WhereIn(o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("Product", model.menuID, ViewPage.CurrentLang.ID)).Where(o => o.PropertyValueID == pid));
                }
            }

            int count = dbQuery.Count().ToValue_Cache().ToInt();
            Json.Instance.Html = count.ToString();
            string urlRidirect = "";
            if (count > 0)
            {
                urlRidirect = model.pageCode;

                int maxCount = 2;
                if (count >= 5) maxCount = 3;
                if (!string.IsNullOrEmpty(atrbrand))
                {
                    int[] atrbrandID = Core.Global.Array.ToInts(atrbrand.Split('-'));
                    for (int i = 0; i < atrbrandID.Length; i++)
                    {
                        var pid = atrbrandID[i];
                        if (pid < 1) continue;
                        var brand = WebMenuService.Instance.GetByID_Cache(pid);
                        if (brand == null) continue;
                        urlRidirect += "-" + brand.Code;
                    }
                    maxCount--;
                }
                if (!string.IsNullOrEmpty(atr))
                {
                    int[] arrID = Core.Global.Array.ToInts(atr.Split('-'));
                    for (int i = 0; i < arrID.Length; i++)
                    {
                        if (maxCount == 0) break;
                        var pid = arrID[i];
                        if (pid < 1) continue;
                        var p = WebPropertyService.Instance.GetByID_Cache(pid);
                        if (p == null) continue;
                        urlRidirect += "-" + p.Code;
                        maxCount--;
                    }
                }
                urlRidirect = (urlRidirect + Setting.Sys_PageExt);
                string pr = "";
                if (!string.IsNullOrEmpty(atrbrand))
                {
                    pr += "#b=" + atrbrand;
                }
                if (!string.IsNullOrEmpty(atr))
                {
                    pr += (!string.IsNullOrEmpty(pr) ? "&c=" : "#c=") + atr;
                }
                Json.Instance.Params = "/" + urlRidirect + pr;
            }
            Json.Create();
        }
        public void ActionLoadProduct(PropertyModel model)
        {
            string brandlist = "";
            string atrbrand = model.b;
            if (!string.IsNullOrEmpty(atrbrand))
            {
                int[] atrbrandID = Core.Global.Array.ToInts(atrbrand.Split('-'));
                for (int i = 0; i < atrbrandID.Length; i++)
                {
                    var pid = atrbrandID[i];
                    if (pid < 1) continue;
                    string sub = WebMenuService.Instance.GetChildIDForWeb_Cache("Brand", pid, ViewPage.CurrentLang.ID);
                    brandlist += (!string.IsNullOrEmpty(brandlist) ? "," : "") + sub;
                }
            }

            var dbQuery = ModProductService.Instance.CreateQuery()
                                    .Select(o => new { o.ID, o.Name, o.PhienBan, o.MenuID, o.BrandID, o.State, o.Code, o.Model, o.File, o.Price, o.Price2, o.View, o.Published, o.Order, o.Activity, o.DatePromotion, o.PricePromotion, o.Summary })
                                    .Where(o => o.Activity == true)
                                    .Where(model.state > 0, o => (o.State & model.state) == model.state);

            //string menulist = WebMenuService.Instance.GetChildIDForWeb_Cache("Product", model.menuID, ViewPage.CurrentLang.ID);
            //string wherein = "";
            //if (!string.IsNullOrEmpty(menulist))
            //{
            //    string[] arrmenu = menulist.Split(',');
            //    for (int i = 0; arrmenu != null && i < arrmenu.Length; i++)
            //    {
            //        if (string.IsNullOrEmpty(arrmenu[i].Trim())) continue;
            //        wherein += (!string.IsNullOrEmpty(wherein) ? " or " : "") + " CHARINDEX('" + arrmenu[i].Trim() + "', [MenuListID]) > 0 ";
            //    }
            //}
            //dbQuery.Where(model.menuID > 0, wherein)
            dbQuery.WhereIn(model.menuID > 0, o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("Product", model.menuID, ViewPage.CurrentLang.ID))

            .Where(model.brandID > 0 && string.IsNullOrEmpty(brandlist), o => o.BrandID == model.brandID)
            .WhereIn(!string.IsNullOrEmpty(brandlist), o => o.BrandID, brandlist)
            .Take(model.pageSize)
            .Skip(model.pageSize * model.page);

            string sort = model.sort;
            if (sort == "new_asc") dbQuery.OrderByDesc(o => o.Published);
            else if (sort == "price_asc") dbQuery.OrderByAsc(o => o.Price);
            else if (sort == "price_desc") dbQuery.OrderByDesc(o => o.Price);
            else if (sort == "view_desc") dbQuery.OrderByDesc(o => o.View);
            else dbQuery.OrderByAsc(o => new { o.Order, o.ID });

            string atr = model.c;
            int[] arrID = Core.Global.Array.ToInts(atr.Split('-'));
            for (int i = 0; i < arrID.Length; i++)
            {
                var pid = arrID[i];
                if (pid < 1) continue;
                dbQuery.WhereIn(o => o.ID, ModPropertyService.Instance.CreateQuery().Select(o => o.ProductID).Distinct().WhereIn(o => o.MenuID, WebMenuService.Instance.GetChildIDForWeb_Cache("Product", model.menuID, ViewPage.CurrentLang.ID)).Where(o => o.PropertyValueID == pid));
            }
            var lstData = dbQuery.ToList_Cache();
            ViewBag.Data = lstData;
            int total = dbQuery.TotalRecord;
            string pathView = string.Concat(new string[]
            {
                "~/Views/",
                "MProduct",
                "/",
                "IndexAjax",
                ".ascx"
            });
            string html = ViewPage.GetHtmlControl(this, pathView, "IndexAjax");

            Json.Instance.Html = html;
            Json.Instance.Params = Utils.FormatMoney(total);
            Json.Instance.Js = (lstData != null && lstData.Count == model.pageSize ? "1" : "0");
            Json.Create();
        }
        public void ActionLoadBreadcrumb(PropertyModel model)
        {
            var url = model.Url;
            string urlRidirect = model.pageCode;
            string urlName = "";
            var page = SysPageService.Instance.GetByCode(model.pageCode);
            string s = Utils.GetMapPage(page);
            string titleAdd = "";
            string atrbrand = model.b;
            urlName = page.Name;
            if (!string.IsNullOrEmpty(atrbrand))
            {
                int[] atrbrandID = Core.Global.Array.ToInts(atrbrand.Split('-'));
                for (int i = 0; i < atrbrandID.Length; i++)
                {
                    var pid = atrbrandID[i];
                    if (pid < 1) continue;
                    var brand = WebMenuService.Instance.GetByID_Cache(pid);
                    if (brand != null)
                    {
                        titleAdd += " " + brand.Name;
                        urlRidirect += "-" + brand.Code;
                        urlName = page.Name + (" " + brand.Name);
                        s += @"<span class=""mx-2""><i class=""fa fa-chevron-right fz-12""></i></span>";
                        s += @"<a href=""" + (model.pageCode + "-" + brand.Code + Setting.Sys_PageExt) + @""" title=""" + urlName + @""">" + urlName + @"</a>";
                    }
                }
            }
            if (!string.IsNullOrEmpty(model.c))
            {
                int[] arrID = Core.Global.Array.ToInts(model.c.Split('-'));
                for (int i = 0; i < arrID.Length; i++)
                {
                    var pid = arrID[i];
                    if (pid < 1) continue;
                    var p = WebPropertyService.Instance.GetByID_Cache(pid);
                    if (p == null) continue;
                    titleAdd += (i > 0 ? " và " : " ") + p.Name;
                    urlRidirect += "-" + p.Code;
                    urlName += (" " + p.Name);
                    s += @"<span class=""mx-2""><i class=""fa fa-chevron-right fz-12""></i></span>";
                    s += @"<a href=""" + (urlRidirect + Setting.Sys_PageExt) + @""" title=""" + urlName + @""">" + urlName + @"</a>";

                    if (urlRidirect == url) break;
                }
            }
            Json.Instance.Html = s;
            Json.Instance.Params = titleAdd;
            Json.Create();
        }
        public void ActionLoadProductDetail(LoadProductAjaxModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            Json.Instance.Html = "";
            var item = ModProductService.Instance.GetByID_Cache(model.ProductID);
            var list = new List<ProductAjaxModel>();
            if (item != null)
            {
                var listGift = item.GetGift();
                var listVideo = item.GetVideo();
                var listFAQ = item.GetFAQ();
                if (listGift != null)
                {
                    list.Add(new ProductAjaxModel("gift", listGift));
                }
                if (listVideo != null)
                {
                    list.Add(new ProductAjaxModel("video", listVideo));
                }
                if (listFAQ != null)
                {
                    list.Add(new ProductAjaxModel("faq", listFAQ));
                }
                var listVoteLike = ModVoteLikeService.Instance.CreateQuery()
                                    .Where(o => o.ProductID == model.ProductID)
                                    .ToList_Cache();

                if (listVoteLike != null)
                {
                    var listVoteLikeGroup = listVoteLike.GroupBy(n => new { n.CommentID }).Select(g => new { CommentID = g.Key.CommentID }).ToList();
                    var listLike = new List<ModVoteLikeGroupByCommentEntity>();
                    string IP = Core.Web.HttpRequest.IP;
                    foreach (var d in listVoteLikeGroup)
                    {
                        var comment = ModVoteService.Instance.GetByID_Cache(d.CommentID);
                        if (comment == null) continue;
                        listLike.Add(
                            new ModVoteLikeGroupByCommentEntity()
                            {
                                CommentID = d.CommentID,
                                Count = listVoteLike.Where(o => o.CommentID == d.CommentID && o.Like == true).Count(),
                                CountUnLike = listVoteLike.Where(o => o.CommentID == d.CommentID && o.UnLike == true).Count(),
                                IsLike = listVoteLike.Where(o => o.CommentID == d.CommentID && o.Like == true && o.IP == IP).Count() > 0,
                                IsUnLike = listVoteLike.Where(o => o.CommentID == d.CommentID && o.UnLike == true && o.IP == IP).Count() > 0,
                                IsVote = comment.Vote > 0 ? false : true
                            }
                        );
                    }
                    if (listLike != null)
                    {
                        list.Add(new ProductAjaxModel("like", listLike));
                    }
                }
                try
                {
                    string CustomerJson = Cookies.GetValue("Customer").ToString();
                    var customer = new CustomerModel();
                    if (!string.IsNullOrEmpty(CustomerJson)) customer = JsonConvert.DeserializeObject<CustomerModel>(CustomerJson);
                    if (!string.IsNullOrEmpty(customer?.Name))
                    {
                        list.Add(new ProductAjaxModel("customer", customer));
                    }
                }
                catch (Exception ex)
                { }
                Json.Instance.Html = JsonConvert.SerializeObject(list);
            }
            Json.Create();
        }
        public void ActionLikeRating(LikeRatingModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            if (model.ProductID < 1)
                ViewPage.Message.ListMessage.Add("Sản phẩm không tồn tại.");
            if (model.CommentID < 1)
                ViewPage.Message.ListMessage.Add("Đánh giá không tồn tại.");

            if (ViewPage.Message.ListMessage.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < ViewPage.Message.ListMessage.Count; i++)
                    message += ViewPage.Message.ListMessage[i] + "<br />";

                Json.Instance.Params = message;
            }
            else
            {
                ModVoteLikeService.Instance.Save(
                    new ModVoteLikeEntity()
                    {
                        ID = 0,
                        ProductID = model.ProductID,
                        CommentID = model.CommentID,
                        IP = Core.Web.HttpRequest.IP,
                        Like = true
                    }
                );
            }
            Json.Create();
        }
        public void ActionUnLikeRating(LikeRatingModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            if (model.ProductID < 1)
                ViewPage.Message.ListMessage.Add("Sản phẩm không tồn tại.");
            if (model.CommentID < 1)
                ViewPage.Message.ListMessage.Add("Đánh giá không tồn tại.");

            if (ViewPage.Message.ListMessage.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < ViewPage.Message.ListMessage.Count; i++)
                    message += ViewPage.Message.ListMessage[i] + "<br />";

                Json.Instance.Params = message;
            }
            else
            {
                ModVoteLikeService.Instance.Save(
                    new ModVoteLikeEntity()
                    {
                        ID = 0,
                        ProductID = model.ProductID,
                        CommentID = model.CommentID,
                        IP = Core.Web.HttpRequest.IP,
                        UnLike = true
                    }
                );
            }
            Json.Create();
        }
        public void ActionCommentRating(CommentRatingModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            string CustomerJson = Cookies.GetValue("Customer").ToString();
            if (string.IsNullOrEmpty(model.VoteName) && !string.IsNullOrEmpty(CustomerJson))
            {
                var customer = new CustomerModel();
                if (!string.IsNullOrEmpty(CustomerJson)) customer = JsonConvert.DeserializeObject<CustomerModel>(CustomerJson);
                if (!string.IsNullOrEmpty(customer?.Name))
                {
                    model.VoteName = customer?.Name;
                    model.VoteEmail = customer?.Email;
                    model.VotePhone = customer?.Phone;
                }
            }
            var listFile = Utils.GetFile(ViewPage.Request.Files, "FileVote");
            var listFileUpdate = new List<string>();
            ModOrderEntity order = null;
            for (var i = 0; listFile != null && i < (listFile.Count > 3 ? 3 : listFile.Count); i++)
            {
                if (listFile[i].ContentLength > 3 * 1024 * 1024)
                {
                    ViewPage.Message.ListMessage.Add("Dung lượng ảnh không được lớn hơn 5Mb.");
                }
                else
                {
                    try
                    {
                        new Bitmap(listFile[i].InputStream);
                        var path = ViewPage.Server.MapPath("~/Data/upload/images/Rating_" + (string.Format("{0:dd_MM_yyyy}", DateTime.Now)) + "/");

                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);

                        var fileName = listFile[i].FileName;
                        var dbFile = "~/Data/upload/images/Rating_" + (string.Format("{0:dd_MM_yyyy}", DateTime.Now)) + "/" + Data.GetCode(System.IO.Path.GetFileNameWithoutExtension(fileName)) + "_" + DateTime.Now.Ticks + System.IO.Path.GetExtension(fileName);
                        var saveFile = ViewPage.Server.MapPath(dbFile);

                        listFile[i].SaveAs(saveFile);
                        listFileUpdate.Add(saveFile);
                    }
                    catch
                    {
                        ViewPage.Message.ListMessage.Add("Avatar tải lên không phải ảnh.");
                    }
                }
            }
            if (model.ProductVoteID < 1)
                ViewPage.Message.ListMessage.Add("Sản phẩm không tồn tại.");
            if (model.StarVote < 1)
                ViewPage.Message.ListMessage.Add("Bạn phải chọn số sao.");
            if (string.IsNullOrEmpty(model.VoteOrderCode))
                ViewPage.Message.ListMessage.Add("Bạn phải nhập mã đơn hàng đã mua.");
            else
            {
                order = ModOrderService.Instance.GetByCode(model.VoteOrderCode);
                if (order == null)
                    ViewPage.Message.ListMessage.Add("Mã đơn hàng không tồn tại.");
            }
            if (string.IsNullOrEmpty(model.VoteName.Trim()))
                ViewPage.Message.ListMessage.Add("Bạn phải nhập tên.");
            if (string.IsNullOrEmpty(model.VotePhone.Trim()))
                ViewPage.Message.ListMessage.Add("Bạn phải nhập số điện thoại.");
            if (string.IsNullOrEmpty(model.VoteContent.Trim()))
                ViewPage.Message.ListMessage.Add("Bạn phải nhập nội dung.");

            if (ViewPage.Message.ListMessage.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < ViewPage.Message.ListMessage.Count; i++)
                    message += ViewPage.Message.ListMessage[i] + "<br />";

                Json.Instance.Params = message;
            }
            else
            {
                if (string.IsNullOrEmpty(CustomerJson))
                {
                    CustomerModel c = new CustomerModel();
                    c.Name = model.VoteName;
                    c.Email = model.VoteEmail;
                    c.Phone = model.VotePhone;
                    c.AnhChi = true;
                    Cookies.SetValue("Customer", JsonConvert.SerializeObject(c));
                }

                var commnet = new ModVoteEntity()
                {
                    ID = 0,
                    ProductID = model.ProductVoteID,
                    Name = model.VoteName,
                    Email = model.VoteEmail,
                    Phone = model.VotePhone,
                    Vote = model.StarVote,
                    Type = model.VoteType,
                    IP = Core.Web.HttpRequest.IP,
                    Created = DateTime.Now,
                    ParentID = 0,
                    Content = model.VoteContent,
                    Activity = false,
                    GioiThieu = model.VoteGioiThieu,
                    DaMua = (order != null ? true : false),
                    NgayMua = (order != null ? order.Created : DateTime.MinValue)
                };
                ModVoteService.Instance.Save(commnet);

                for (int i = 0; listFileUpdate != null && i < (listFileUpdate.Count > 5 ? 5 : listFileUpdate.Count); i++)
                {
                    if (string.IsNullOrEmpty(listFileUpdate[i].Trim())) continue;
                    ModVoteFileService.Instance.Save(
                        new ModVoteFileEntity()
                        {
                            ID = 0,
                            ProductID = model.ProductVoteID,
                            CommentID = commnet.ID,
                            File = listFileUpdate[i],
                            Order = GetMaxVoteFileOrder(model.ProductVoteID),
                            Show = true,
                            Activity = false
                        }
                    );
                }

            }
            Json.Create();
        }
        public void ActionAddComment(AddCommentModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;
            string CustomerJson = Cookies.GetValue("Customer").ToString();
            if (string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(CustomerJson))
            {
                var customer = new CustomerModel();
                if (!string.IsNullOrEmpty(CustomerJson)) customer = JsonConvert.DeserializeObject<CustomerModel>(CustomerJson);
                if (!string.IsNullOrEmpty(customer?.Name))
                {
                    model.Name = customer?.Name;
                    model.Email = customer?.Email;
                    model.Phone = customer?.Phone;
                }
            }
            if (model.ParentID < 0) model.ParentID = 0;
            if (model.Vote < 0) model.Vote = 0;
            if (model.ParentID > 0) model.Vote = 0;
            if (model.ProductID < 1)
                ViewPage.Message.ListMessage.Add("Sản phẩm không tồn tại.");
            if (string.IsNullOrEmpty(model.Name))
                ViewPage.Message.ListMessage.Add("Nhập: Họ và tên.");
            if (string.IsNullOrEmpty(model.Content))
                ViewPage.Message.ListMessage.Add("Nhập: Nội dung.");
            //if (model.CommentID == 0 && model.Vote < 1)
            //    ViewPage.Message.ListMessage.Add("Hãy chọn số sao đánh giá.");

            if (ViewPage.Message.ListMessage.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < ViewPage.Message.ListMessage.Count; i++)
                    message += ViewPage.Message.ListMessage[i] + "<br />";

                Json.Instance.Params = message;
            }
            else
            {
                if (string.IsNullOrEmpty(CustomerJson))
                {
                    CustomerModel c = new CustomerModel();
                    c.Name = model.Name;
                    c.Email = model.Email;
                    c.Phone = model.Phone;
                    c.AnhChi = true;
                    Cookies.SetValue("Customer", JsonConvert.SerializeObject(c));
                }
                ModVoteEntity item = new ModVoteEntity();
                item.ID = 0;
                item.IP = Core.Web.HttpRequest.IP;
                item.Created = DateTime.Now;
                item.Activity = false;
                item.ParentID = model.ParentID;
                item.Name = model.Name;
                item.Phone = model.Phone;
                item.Email = model.Email;
                item.Content = model.Content;
                item.Vote = model.Vote;
                item.Type = model.Type;
                //item.WebUserID = WebLogin.WebUserID;
                item.ProductID = model.ProductID;
                Cookies.SetValue("CommentName", item.Name);
                Cookies.SetValue("CommentEmail", item.Email);
                try
                {
                    ModVoteService.Instance.Save(item);

                    Json.Instance.Html = "Bạn đã bình luận thành công. Chúng tôi sẽ phản hồi lại cho quý khách trong thời gian sớm nhất.";
                }
                catch (Exception ex)
                {
                    Json.Instance.Params = ex.Message;
                }
            }
            Json.Create();
        }
        private static int GetMaxVoteFileOrder(int ProductID)
        {
            return ModVoteFileService.Instance.CreateQuery()
                    .Where(o => o.ProductID == ProductID)
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
        public void ActionConfirmUser(ConfirmUserModel model)
        {
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = string.Empty;

            if (string.IsNullOrEmpty(model.cfmName.Trim()))
                ViewPage.Message.ListMessage.Add("Nhập: Họ và tên.");
            if (!string.IsNullOrEmpty(model.cfmEmail.Trim()))
            {
                if (!Utils.IsEmailAddress(model.cfmEmail.Trim()))
                    ViewPage.Message.ListMessage.Add("Nhập: Địa chỉ email của bạn.");
            }

            if (ViewPage.Message.ListMessage.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < ViewPage.Message.ListMessage.Count; i++)
                    message += ViewPage.Message.ListMessage[i] + "<br />";

                Json.Instance.Params = message;
            }
            else
            {

                try
                {
                    CustomerModel item = new CustomerModel();
                    item.Name = model.cfmName;
                    item.Email = model.cfmEmail;
                    item.Phone = model.cfmPhone;
                    item.AnhChi = model.cfmGender;
                    Cookies.SetValue("Customer", JsonConvert.SerializeObject(item));
                    Json.Instance.Html = "Bạn cập nhật thông tin thành công.";
                }
                catch (Exception ex)
                {
                    Json.Instance.Params = ex.Message;
                }
            }
            Json.Create();
        }
        public void ActionChangePrice(ChangePriceModel model)
        {
            if (model.SizeID < 0) model.SizeID = 0;
            if (model.ColorID < 0) model.ColorID = 0;
            Json.Instance.Html = Json.Instance.Params = Json.Instance.Js = Json.Instance.Extension = Json.Instance.Extension2 = Json.Instance.Extension3 = Json.Instance.Extension4 = Json.Instance.Extension5 = string.Empty;
            Json.Instance.Html = "";
            if (model.ProductID < 1)
            {
                Json.Instance.Params = "Sản phẩm không tồn tại";
            }
            else
            {
                var product = ModProductService.Instance.GetByID_Cache(model.ProductID);
                if (product == null)
                {
                    Json.Instance.Params = "Sản phẩm không tồn tại";
                }
                else
                {
                    var item = ModProductClassifyDetailPriceService.Instance.GetByProperty(model.ProductID, model.ColorID, model.SizeID);

                    long PriceView = product.PriceView;
                    long PriceView2 = product.PriceView2;
                    long SellOff = product.SellOff;
                    long SellOffPercent = product.SellOffPercent;
                    if (item != null)
                    {
                        PriceView = item.Price;
                        if (item.Price2 > 0)
                        {
                            PriceView2 = item.Price2;
                            SellOff = item.SellOff;
                            SellOffPercent = item.SellOffPercent;
                        }
                    }
                    string html = "";
                    if (PriceView > 0)
                    {
                        html += "<span>" + Utils.FormatMoney(PriceView) + "₫</span>";
                    }
                    else
                    {
                        html += "<span>Liên hệ</span>";
                    }
                    if (PriceView > 0)
                    {
                        html += "<del><span>" + Utils.FormatMoney(PriceView2) + "₫</span></del>";
                    }
                    Json.Instance.Html = html;
                }
            }

            Json.Create();
        }
        public void ActionLoadPropertyBuy(PropertyBuyModel model)
        {
            var item = ModProductService.Instance.GetByID_Cache(model.ProductID);
            var listProductClassify = item.GetProductClassify();
            var listProductClassifyDetail = item.GetProductClassifyDetail();
            var listProductClassifyDetailPrice = item.GetProductClassifyDetailPrice();
            string header = "";
            header += @"  <div>";
            header += @"    <div class=""box-product-name"">";
            header += @"        <h3 class=""fz-16 fw-medium"">" + item.Name + "</h3>";
            header += @"    </div>";
            header += @"    <div class=""box-product-price p-0"">";
            header += @"        <strong class=""price"" id=""PriceFromProduct"">";
            if (item.PriceView > 0)
            {
                header += @"          <span>" + Utils.FormatMoney(item.PriceView) + " ₫</span>";
            }
            else
            {
                header += @"          <span>Liên hệ</span>";
            }
            if (item.PriceView2 > 0)
            {
                header += @"          <del><span>" + Utils.FormatMoney(item.PriceView2) + " ₫</span></del>";
            }
            header += @"        </strong>";
            header += @"    </div>";
            header += @"  </div>";
            string html = "";
            for (int i = 0; i < listProductClassify.Count; i++)
            {
                var lstItem = listProductClassifyDetail.Where(o => o.ClassifyID == listProductClassify[i].ID).ToList();

                html += @"<div class=""box-product-filter"">";
                html += @"   <p class=""fz-14 fw-medium mb-3"">" + listProductClassify[i].Name + "</p>";
                html += @"   <div class=""product-filter-list"">";
                for (int z = 0; lstItem != null && z < lstItem.Count; z++)
                {
                    html += @"       <label>";
                    html += @"           <input type=""radio"" name=""" + (i == 0 ? "ColorFromProduct" : "SizeFromProduct") + @""" value=""" + lstItem[z].ID + @""" " + ((i == 0 && lstItem[z].ID == model.ColorID) || (i == 1 && lstItem[z].ID == model.SizeID) ? "checked" : "") + @" onclick=""ChangePriceFromProduct();"">";
                    html += @"           <span class=""radio-btn"">";
                    if (!string.IsNullOrEmpty(lstItem[z].File))
                    {
                        html += @"              <img title=""" + lstItem[z].Name + @""" alt=""" + lstItem[z].Name + @""" src=""" + Utils.GetWebPFile(lstItem[z].File, 4, 30, 30) + @""">";
                    }
                    html += @"              <span class=""d-flex text-start flex-column"">";
                    html += @"                   <b class=""text"">" + lstItem[z].Name + "</b>";
                    html += @"              </span>";
                    html += @"           </span>";
                    html += @"       </label>";
                }
                html += @"   </div>";
                html += @"</div>";
            }
            Json.Instance.Params = header;
            Json.Instance.Html = html;
            Json.Create();
        }
        #region private func

        private void EndResponse()
        {
            var s = @" < Xml>
                          <Item>
                            <Html><![CDATA[" + _ajaxModel.Html + @"]]></Html>
                            <Params><![CDATA[" + _ajaxModel.Params + @"]]></Params>
                            <JS><![CDATA[" + _ajaxModel.Js + @"]]></JS>
                          </Item>
                        </Xml>";
            ViewPage.Response.ContentType = "text/xml";
            ViewPage.Response.Write(s);
            ViewPage.Response.End();
        }

        private readonly MAjaxModel _ajaxModel = new MAjaxModel { Params = "", Html = "", Js = "" };

        #endregion private func
    }

    public class MAjaxModel
    {
        public string Html { get; set; }
        public string Params { get; set; }
        public string Js { get; set; }
    }

    public class SecurityModel
    {
        public string Code { get; set; }
    }
    public class GetChildModel
    {
        public int ParentID { get; set; }
        public int SelectedID { get; set; }
    }
    public class DeleteCartModel
    {
        public int ProductID { get; set; }
        public int Index { get; set; }
    }
    public class AddToCartModel
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public int SizeID { get; set; }
        public int ColorID { get; set; }
    }
    public class UpdateCartModel
    {
        public int Index { get; set; }
        public int Quantity { get; set; }
    }
    public class PropertyModel
    {
        public int menuID { get; set; }
        public int brandID { get; set; }
        public string pageCode { get; set; }
        public string b { get; set; }
        public string c { get; set; }
        public int state { get; set; }
        public string sort { get; set; }
        public int pageSize { get; set; }
        public int page { get; set; }
        public string Url { get; set; }
    }
    public class LoadProductAjaxModel
    {
        public int ProductID { get; set; }
    }
    public class AddCommentModel
    {
        public int ProductID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public double Vote { get; set; }
        public string Type { get; set; }
    }
    public class LikeRatingModel
    {
        public int ProductID { get; set; }
        public int CommentID { get; set; }
    }
    public class CommentRatingModel
    {
        public int ProductVoteID { get; set; }
        public int StarVote { get; set; }
        public string VoteOrderCode { get; set; }
        public string VoteName { get; set; }
        public string VotePhone { get; set; }
        public string VoteEmail { get; set; }
        public string VoteContent { get; set; }
        public string VoteType { get; set; }
        public bool VoteGioiThieu { get; set; }

    }
    public class ConfirmUserModel
    {
        public bool cfmGender { get; set; }
        public string cfmName { get; set; }
        public string cfmEmail { get; set; }
        public string cfmPhone { get; set; }
    }

    public class ChangePriceModel
    {
        public int SizeID { get; set; }
        public int ColorID { get; set; }
        public int ProductID { get; set; }
    }
    public class PropertyBuyModel
    {
        public int ProductID { get; set; }
        public int SizeID { get; set; }
        public int ColorID { get; set; }
    }
}