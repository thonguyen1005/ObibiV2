using Microsoft.AspNetCore.Mvc;
using VSW.Core.Services;

namespace VSW.Website.Controllers
{
    public class MFeedbackController : BaseController
    {
        public MFeedbackController(IWorkingContext<MFeedbackController> context) : base(context)
        {
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
