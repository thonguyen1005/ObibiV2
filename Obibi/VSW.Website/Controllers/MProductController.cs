using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.Interface;

namespace VSW.Website.Controllers
{
    public class MProductController : BaseController
    {
        public MProductController(IWorkingContext<MProductController> context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Detail([FromRoute] string id)
        {
            ViewBag.Layout = "Detail";
            return View();
        }
    }
}
