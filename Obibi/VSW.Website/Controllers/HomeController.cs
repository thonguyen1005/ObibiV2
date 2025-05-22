using Microsoft.AspNetCore.Mvc;
using VSW.Core.Services;

namespace VSW.Website.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IWorkingContext<HomeController> context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            int statusCode = Response.StatusCode;
            string statusMessage = Global.Error.getError(statusCode);

            ViewBag.statusCode = statusCode;
            ViewBag.statusMessage = statusMessage;
            ViewBag.Layout = "Error";
            return View();
        }
    }
}
