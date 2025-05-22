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
        private ModProductClassifyDetailPriceRepository _productclassifydetailpriceRepository = null;
        public AjaxController(IWorkingContext<AjaxController> context) : base(context)
        {
            _pageRepository = new SysPageRepository(context: context);
            _menuRepository = new WebMenuRepository(context: context);
            _productRepository = new ModProductRepository(context: context);
            _productclassifydetailpriceRepository = new ModProductClassifyDetailPriceRepository(context: context);
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

        public IActionResult DeleteCart([FromQuery] int ProductID, [FromQuery] int Index)
        {
            var cart = new Cart();
            var _item = cart.Items[Index];
            cart.Remove(_item);
            cart.Save();

            return Success();
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
                var productPrice = _productclassifydetailpriceRepository.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
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

            return Success(new { Html = html, CartCount = Utils.FormatMoney(cart.Items.Count) });
        }
    }
}
