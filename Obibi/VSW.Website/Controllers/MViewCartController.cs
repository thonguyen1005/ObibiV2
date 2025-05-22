using Elastic.Apm.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core;
using VSW.Core.Crypto;
using VSW.Core.Services;
using VSW.Core.Utils;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Global;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.Controllers
{
    public class MViewCartController : BaseController
    {
        private ModOrderRepository _orderRepository = null;
        private ModOrderDetailRepository _orderDetailRepository = null;
        private ModWebUserRepository _webUserRepository = null;
        private WebMenuRepository _menuRepository = null;
        private ModProductRepository _productRepository = null;
        private ModProductClassifyRepository _productClassifyRepository = null;
        private ModProductClassifyDetailRepository _productClassifyDetailRepository = null;
        private ModProductClassifyDetailPriceRepository _productClassifyDetailPriceRepository = null;
        public MViewCartController(IWorkingContext<MViewCartController> context) : base(context)
        {
            _orderRepository = new ModOrderRepository(context: context);
            _orderDetailRepository = new ModOrderDetailRepository(context: context);
            _webUserRepository = new ModWebUserRepository(context: context);
            _menuRepository = new WebMenuRepository(context: context);
            _productRepository = new ModProductRepository(context: context);
            _productClassifyRepository = new ModProductClassifyRepository(context: context);
            _productClassifyDetailRepository = new ModProductClassifyDetailRepository(context: context);
            _productClassifyDetailPriceRepository = new ModProductClassifyDetailPriceRepository(context: context);
        }
        public async Task<IActionResult> Index(MOD_ORDEREntity item)
        {
            MOrderModel model = new MOrderModel();
            model.lstCity.PrepareData(_menuRepository.ShowChildMenuByType("City", CurrentSite.LangID), "Name", "ID", true, "Chọn thành phố", "0");

            var cart = new Cart();
            model.TotalQuantity = 0;
            model.TotalPrice = 0;
            for (int i = 0; i < cart.Items.Count; i++)
            {
                var product = (await _productRepository.GetAsync(cart.Items[i].ProductID)).MapTo<ModProductModel>();
                if (product == null) continue;

                var size = _productClassifyDetailPriceRepository.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                long price = product.PriceView;
                if (size != null)
                {
                    price = size.Price;
                }
                product.PriceByCart = price;
                product.QuantityByCart = cart.Items[i].Quantity;
                product.SizeByCart = cart.Items[i].SizeID;
                product.ColorByCart = cart.Items[i].ColorID;
                long t = cart.Items[i].Quantity * product.PriceByCart;
                model.TotalPrice += t;
                model.TotalQuantity += cart.Items[i].Quantity;
                model.lstProduct.Add(product);
            }
            if (model.lstProduct.IsNotEmpty())
            {
                var lstProductId = model.lstProduct.Select(o => o.ID).ToList();
                model.lstClassify = _productClassifyRepository.GetTable().Where(o => lstProductId.Contains(o.ProductID)).ToList();
                model.lstClassifyDetail = _productClassifyDetailRepository.GetTable().Where(o => lstProductId.Contains(o.ProductID)).ToList();
                model.lstClassifyDetailPrice = _productClassifyDetailPriceRepository.GetTable().Where(o => lstProductId.Contains(o.ProductID)).ToList();
            }
            ViewBag.Model = model;
            return View(item);
        }
        /*
        public IActionResult Add(MViewCartModel model)
        {
            var cart = new Cart();
            if (model.SizeID < 0) model.SizeID = 0;
            if (model.ColorID < 0) model.ColorID = 0;
            if (model.Quantity <= 0) model.Quantity = 1;
            var _item = cart.Find(new CartItem() { ProductID = model.ProductID, SizeID = model.SizeID, ColorID = model.ColorID });
            if (_item == null)
            {
                cart.Add(new CartItem
                {
                    ProductID = model.ProductID,
                    SizeID = model.SizeID,
                    ColorID = model.ColorID,
                    Quantity = model.Quantity,
                });
            }
            else
            {
                _item.Quantity += model.Quantity;
            }

            cart.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(MViewCartModel model)
        {
            var cart = new Cart();

            cart.Remove(cart.Items[model.Index]);
            cart.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Update(MViewCartModel model)
        {
            var cart = new Cart();

            if (model.Quantity <= 0) model.Quantity = 1;
            cart.Items[model.Index].Quantity = model.Quantity;
            if (model.SizeID > 0) cart.Items[model.Index].SizeID = model.SizeID;
            if (model.ColorID > 0) cart.Items[model.Index].ColorID = model.ColorID;

            cart.Save();

            return RedirectToAction("Index");
        }
        */

        [HttpPost]
        public async Task<IActionResult> AddPOST([FromBody] MOD_ORDEREntity item)
        {
            var lstMessage = new List<string>();
            if (item.Name.Trim() == string.Empty)
                lstMessage.Add("Nhập: Họ và tên.");
            if (item.Phone.Trim() == string.Empty)
                lstMessage.Add("Nhập: Số điện thoại.");
            else if (!Global.Utils.IsPhone(item.Phone.Trim()))
                lstMessage.Add("Số điện thoại không đúng.");
            if (item.Email.IsNotEmpty())
            {
                if (!Utils.IsEmailAddress(item.Email.Trim()))
                    lstMessage.Add("Nhập: đúng đinh dạng Email.");
            }

            if (item.CityID < 1)
                lstMessage.Add("Chọn: Tỉnh/Thành Phố.");
            if (item.DistrictID < 1)
                lstMessage.Add("Chọn: Quận/Huyện.");
            if (item.WardID < 1)
                lstMessage.Add("Chọn: Phường/Xã.");
            if (item.Address.Trim() == string.Empty)
                lstMessage.Add("Nhập: Số nhà, tòa nhà, tên thôn xóm, tên đường...");

            if (item.Invoice && item.CompanyName.Trim() == string.Empty)
                lstMessage.Add("Nhập: tên công ty.");
            if (item.Invoice && item.CompanyTax.Trim() == string.Empty)
                lstMessage.Add("Nhập: mã số thuế.");

            item.StatusPay = false;

            //hien thi thong bao loi
            if (lstMessage.IsNotEmpty())
            {
                return Error(string.Join("<br />", lstMessage));
            }
            else
            {
                var cart = new Cart();
                item.IP = HttpContext.Connection.RemoteIpAddress?.ToString();
                item.Created = DateTime.Now;
                item.StatusID = 11977;
                item.OrderNews = true;
                _orderRepository.Insert(item);
                item.Code = "DH" + string.Format("{0:ddMMyyyy}", item.Created) + GetOrder(item.ID);

                long total = 0;
                long weight = 0;
                for (var i = 0; i < cart.Count; i++)
                {
                    var product = _productRepository.Get(cart.Items[i].ProductID);
                    if (product == null) continue;
                    long wp = product.Weight.ToLong();
                    var size = _productClassifyDetailPriceRepository.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                    long price = product.PriceView;
                    if (size != null)
                    {
                        price = size.Price;
                    }
                    weight += (wp * cart.Items[i].Quantity);
                    long t = cart.Items[i].Quantity * price;
                    total += t;
                    string other = "";
                    var detail = new MOD_ORDERDETAILEntity()
                    {
                        ID = 0,
                        OrderID = item.ID,
                        ProductID = product.ID,
                        SizeID = cart.Items[i].SizeID,
                        ColorID = cart.Items[i].ColorID,
                        Quantity = cart.Items[i].Quantity,
                        Price = price.ToInt(),
                        PriceAll = t,
                        Other = other,
                        Created = item.Created,
                    };
                    _orderDetailRepository.Insert(detail);
                }
                item.SaleMoney = 0;
                item.SalePercent = 0;
                item.SaleCode = "";
                item.Total = total;
                item.Fee = 0;

                var w = new MOD_WEBUSEREntity();
                {
                    if (!string.IsNullOrEmpty(item.Email))
                    {
                        w = _webUserRepository.GetTable().Where(o=> o.Email == item.Email).FirstOrDefault();
                        if (w == null)
                        {
                            w = _webUserRepository.GetTable().Where(o => o.Phone == item.Phone).FirstOrDefault();
                        }
                    }
                    else if (!string.IsNullOrEmpty(item.Phone))
                    {
                        w = _webUserRepository.GetTable().Where(o => o.Phone == item.Phone).FirstOrDefault();
                    }
                    if (w == null)
                    {
                        w = new MOD_WEBUSEREntity();
                        w.UserName = (!string.IsNullOrEmpty(item.Email) ? item.Email : item.Phone);
                        if (string.IsNullOrEmpty(w.Password))
                        {
                            w.Password = EncryptionHelper.ComputeMD5("Abc@12345");
                        }
                        w.Name = item.Name;
                        w.Email = item.Email;
                        w.Phone = item.Phone;
                        w.Address = item.Address;
                        w.Type = 2;
                        w.Type2 = 1;
                        w.CityID = item.CityID;
                        w.DistrictID = item.DistrictID;
                        w.WardID = item.WardID;
                        w.CompanyName = item.CompanyName;
                        w.CompanyAddress = item.CompanyAddress;
                        w.CompanyTax = item.CompanyTax;
                        w.IP = HttpContext.Connection.RemoteIpAddress?.ToString();
                        w.Created = DateTime.Now;
                        w.Activity = true;
                        _webUserRepository.Insert(w);
                    }
                    item.WebUserID = w.ID;
                }
                item.SaleMoneyPoint = 0;
                item.Point = 0;
                item.SaleMoney = item.SaleMoney < 0 ? 0 : item.SaleMoney;
                item.SaleCustomer = item.SaleCustomer < 0 ? 0 : item.SaleCustomer;
                item.SaleMoneyPoint = item.SaleMoneyPoint < 0 ? 0 : item.SaleMoneyPoint;
                item.Fee = item.Fee < 0 ? 0 : item.Fee;
                _orderRepository.Update(item);

                cart.RemoveAll();
                cart.Save();

                return Success(item.ID);
            }
        }

        private static string GetOrder(int orderid)
        {
            if (orderid <= 1) return "0000001";

            var result = string.Empty;
            for (var i = 1; i <= (7 - orderid.ToString().Length); i++)
            {
                result += "0";
            }

            return result + (orderid + 1);
        }
    }
}
