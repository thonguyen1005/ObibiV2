using VSW.Lib.Global;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    public class FormTextController : CPController
    {
        public void ActionIndex(FormTextModel model)
        {
            if (!CPViewPage.IsPostBack && !string.IsNullOrEmpty(model.TextID))
                model.Value = Data.Base64Decode(Session.GetValue(model.TextID).ToString().Replace(" ", "+"));

            ViewBag.Model = model;
        }

        public void ActionApply(FormTextModel model)
        {
            if (string.IsNullOrEmpty(model.Value))
            {
                CPViewPage.Alert("Nhập nội dung");
                return;
            }

            var s = Data.Base64Encode(model.Value);

            Session.SetValue(model.TextID, s);

            CPViewPage.Script("Close", "Close('" + s + "')");
        }

        public override void ActionCancel()
        {
            CPViewPage.Script("Cancel", "Cancel()");
        }
    }

    public class FormTextModel : DefaultModel
    {
        public string TextID { get; set; }
        public string Value { get; set; }
    }
}