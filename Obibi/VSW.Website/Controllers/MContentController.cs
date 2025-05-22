using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.Interface;

namespace VSW.Website.Controllers
{
    public class MContentController : BaseController
    {
        public MContentController(IWorkingContext<MContentController> context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
