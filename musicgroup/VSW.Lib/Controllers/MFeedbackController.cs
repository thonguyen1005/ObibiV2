using System;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO: Liên hệ", Code = "MFeedback", Order = 12)]
    public class MFeedbackController : Controller
    {
        public void ActionIndex(ModFeedbackEntity item, MFeedbackModel model)
        {
            ViewBag.Data = item;
            ViewBag.Model = model;

            //SEO
            ViewPage.CurrentPage.PageURL = ViewPage.CurrentURL;
            ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(ViewPage.CurrentPage.File);
        }

        public void ActionAddPOST(ModFeedbackEntity item, MFeedbackModel model)
        {
            if (item.Name.Trim() == string.Empty)
                ViewPage.Message.ListMessage.Add("Nhập: Họ và tên.");
            if (item.Phone.Trim() == string.Empty)
                ViewPage.Message.ListMessage.Add("Nhập: Số điện thoại.");
            if (item.Content.Trim() == string.Empty)
                ViewPage.Message.ListMessage.Add("Nhập: Nội dung liên hệ.");

            //hien thi thong bao loi
            if (ViewPage.Message.ListMessage.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < ViewPage.Message.ListMessage.Count; i++)
                    message += ViewPage.Message.ListMessage[i] + "<br />";

                ViewPage.Alert(message);
            }
            else
            {
                item.ID = 0;
                item.IP = Core.Web.HttpRequest.IP;
                item.Created = DateTime.Now;

                ModFeedbackService.Instance.Save(item);

                //xoa trang
                item = new ModFeedbackEntity();
                ViewPage.Alert("Cảm ơn bạn đã liên hệ với chúng tôi.<br /> Chúng tôi sẽ phản hồi lại trong thời gian sớm nhất.");
            }

            ViewBag.Data = item;
            ViewBag.Model = model;
        }
    }

    public class MFeedbackModel
    {
        public string ValidCode { get; set; }
    }
}