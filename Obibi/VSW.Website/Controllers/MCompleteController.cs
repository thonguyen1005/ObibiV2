using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.Controllers
{
    public class MCompleteController : BaseController
    {
        private ModOrderRepository _orderRepository = null;
        public MCompleteController(IWorkingContext<MCompleteController> context) : base(context)
        {
            _orderRepository = new ModOrderRepository(context: context);
        }
        public async Task<IActionResult> Index(MCompleteModel model)
        {
            var order = await _orderRepository.GetAsync(model.OrderID);
            ViewBag.Model = model;
            return View(order);
        }
    }
}
