using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.Interface;

namespace VSW.Website.Controllers
{
    public class MEmptyController : BaseController
    {
        public MEmptyController(IWorkingContext<MEmptyController> context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
