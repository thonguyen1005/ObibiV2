using Elastic.Apm.Model;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using VSW.Core.Services;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Global;
using VSW.Website.Helpers;

namespace VSW.Website.Controllers
{
    public class AjaxController : BaseController
    {
        private SysPageRepository _pageRepository = null;
        private WebMenuRepository _menuRepository = null;
        private ModProductRepository _productRepository = null;
        private ModProductClassifyRepository _productClassifyRepository = null;
        private ModProductClassifyDetailRepository _productClassifyDetailRepository = null;
        private ModProductClassifyDetailPriceRepository _productClassifyDetailPriceRepository = null;
        public AjaxController(IWorkingContext<AjaxController> context) : base(context)
        {
            _pageRepository = new SysPageRepository(context: context);
            _menuRepository = new WebMenuRepository(context: context);
            _productRepository = new ModProductRepository(context: context);
            _productClassifyRepository = new ModProductClassifyRepository(context: context);
            _productClassifyDetailRepository = new ModProductClassifyDetailRepository(context: context);
            _productClassifyDetailPriceRepository = new ModProductClassifyDetailPriceRepository(context: context);
        }

        public IActionResult GetChild([FromQuery] int ParentID, [FromQuery] int SelectedID)
        {
            string html = "";

            if (ParentID > 0)
            {
                var listItem = _menuRepository.GetTable()
                                    .Where(o => o.Activity == true && o.ParentID == ParentID)
                                    .Select(o => new { o.ID, o.Name })
                                    .OrderBy(o => new { o.Name, o.ID })
                                    .ToList();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    html += @"<option value=""" + listItem[i].ID + @""" " + (listItem[i].ID == SelectedID ? @"selected=""selected""" : @"") + @">" + listItem[i].Name + @"</option>";
                }
            }

            return Success(html);
        }

        public async Task<IActionResult> AddToCart([FromQuery] int ProductID, [FromQuery] int Quantity, [FromQuery] int SizeID, [FromQuery] int ColorID)
        {
            var cart = new Cart();
            if (Quantity < 1) Quantity = 1;
            var _item = cart.Find(new CartItem() { ProductID = ProductID, SizeID = SizeID, ColorID = ColorID });
            if (_item == null)
            {
                cart.Add(new CartItem
                {
                    ProductID = ProductID,
                    Quantity = Quantity,
                    SizeID = SizeID,
                    ColorID = ColorID,
                });
            }
            else
            {
                _item.Quantity = Quantity;
            }
            cart.Save();

            long total = 0;
            string html = "";
            for (int i = 0; i < cart.Items.Count; i++)
            {
                var product = _productRepository.Get(cart.Items[i].ProductID);
                if (product == null) continue;
                var productPrice = _productClassifyDetailPriceRepository.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                long price = product.PriceView;
                if (productPrice != null)
                {
                    price = productPrice.Price;
                }
                total += price * cart.Items[i].Quantity;

                string url = GetUrl(product.Code);

                html += @"<li id=""item-cart-" + i + @""">
                            <div class=""dok-cart-img"">
                        <a href=""" + url + @""" title=""" + product.Name + @""">
                            <img class=""img-fluid""
                                src=""" + (await ImageHelper.ResizeToWebpAsync(product.File, 50, 50)) + @"""
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
                    html += @"<p class=""dok-cart-price"">Liên hệ</p>";
                }
                html += @"</div>
                    <a class=""dok-cart-trash"" href=""javascript:deleteCart(" + product.ID + @", " + i + @")"">
                        <i class=""far fa-trash-alt""></i>
                    </a>
                </li>";
            }

            return Success(new { Html = html, CartCount = Utils.FormatMoney(cart.Items.Count), CartSum = Utils.FormatMoney(total) });
        }

        public async Task<IActionResult> DeleteCart([FromQuery] int Index)
        {
            var cart = new Cart();
            var _item = cart.Items[Index];
            cart.Remove(_item);
            cart.Save();

            long total = 0;
            for (int i = 0; i < cart.Items.Count; i++)
            {
                var product = _productRepository.Get(cart.Items[i].ProductID);
                if (product == null) continue;
                var productPrice = _productClassifyDetailPriceRepository.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                long price = product.PriceView;
                if (productPrice != null)
                {
                    price = productPrice.Price;
                }
                total += price * cart.Items[i].Quantity;
            }
            return Success(new { CartCount = Utils.FormatMoney(cart.Items.Count), CartSum = Utils.FormatMoney(total) });
        }

        public async Task<IActionResult> UpdateCart([FromQuery] int Index, [FromQuery] int Quantity, [FromQuery] int SizeID, [FromQuery] int ColorID)
        {
            var cart = new Cart();
            if (Quantity < 1) Quantity = 1;
            cart.Items[Index].Quantity = Quantity;
            if (SizeID > 0) cart.Items[Index].SizeID = SizeID;
            if (ColorID > 0) cart.Items[Index].ColorID = ColorID;
            cart.Save();

            long total = 0;
            long totalByIndex = 0;
            string html = "";
            for (int i = 0; i < cart.Items.Count; i++)
            {
                var product = _productRepository.Get(cart.Items[i].ProductID);
                if (product == null) continue;
                var productPrice = _productClassifyDetailPriceRepository.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                long price = product.PriceView;
                if (productPrice != null)
                {
                    price = productPrice.Price;
                }
                total += price * cart.Items[i].Quantity;
                if (i == Index) totalByIndex = price * cart.Items[i].Quantity;

                string url = GetUrl(product.Code);

                html += @"<li id=""item-cart-" + i + @""">
                            <div class=""dok-cart-img"">
                        <a href=""" + url + @""" title=""" + product.Name + @""">
                            <img class=""img-fluid""
                                src=""" + (await ImageHelper.ResizeToWebpAsync(product.File, 50, 50)) + @"""
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
                    html += @"<p class=""dok-cart-price"">Liên hệ</p>";
                }
                html += @"</div>
                    <a class=""dok-cart-trash"" href=""javascript:deleteCart(" + product.ID + @", " + i + @")"">
                        <i class=""far fa-trash-alt""></i>
                    </a>
                </li>";
            }

            return Success(new { Html = html, CartCount = Utils.FormatMoney(cart.Items.Count), CartSum = Utils.FormatMoney(total), CartSumByIndex = Utils.FormatMoney(totalByIndex) });
        }

        public async Task<IActionResult> ActionLoadPropertyBuy(PropertyBuyModel model)
        {
            var item = await _productRepository.GetAsync(model.ProductID);
            var listProductClassify = await _productClassifyRepository.GetTable().Where(o => o.ProductID == model.ProductID).ToListAsync();
            var listProductClassifyDetail = await _productClassifyDetailRepository.GetTable().Where(o => o.ProductID == model.ProductID).ToListAsync();
            var listProductClassifyDetailPrice = await _productClassifyDetailPriceRepository.GetTable().Where(o => o.ProductID == model.ProductID).ToListAsync();

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
            if (item.PriceRoot > 0)
            {
                header += @"          <del><span>" + Utils.FormatMoney(item.PriceRoot) + " ₫</span></del>";
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
                        html += @"              <img title=""" + lstItem[z].Name + @""" alt=""" + lstItem[z].Name + @""" src=""" + await ImageHelper.ResizeToWebpAsync(lstItem[z].File) + @""">";
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

            return Success(new { Header = header, Html = html });
        }
    }

    public class PropertyBuyModel
    {
        public int ProductID { get; set; }
        public int SizeID { get; set; }
        public int ColorID { get; set; }
    }
}
