using Elastic.Apm.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core;
using VSW.Core.Services;
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
        private WebMenuRepository _menuRepository = null;
        private ModProductRepository _productRepository = null;
        private ModProductClassifyRepository _productClassifyRepository = null;
        private ModProductClassifyDetailRepository _productClassifyDetailRepository = null;
        private ModProductClassifyDetailPriceRepository _productClassifyDetailPriceRepository = null;
        public MViewCartController(IWorkingContext<MViewCartController> context) : base(context)
        {
            _menuRepository = new WebMenuRepository(context: context);
            _productRepository =  new ModProductRepository(context: context);
            _productClassifyRepository = new ModProductClassifyRepository(context: context);
            _productClassifyDetailRepository = new ModProductClassifyDetailRepository(context: context);
            _productClassifyDetailPriceRepository= new ModProductClassifyDetailPriceRepository(context: context);
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


    }
}
