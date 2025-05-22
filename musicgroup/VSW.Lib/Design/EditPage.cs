using System;
using VSW.Core.Design;
using VSW.Core.Web;
using VSW.Lib.Models;
using static VSW.Lib.Global.CPLogin;
using File = System.IO.File;

namespace VSW.Lib.Design
{
    public sealed class EditPage : ViewPageDesign
    {
        public EditPage()
        {
            PageService = SysPageService.Instance;
            TemplateService = SysTemplateService.Instance;
            ModuleService = SysModuleService.Instance;

            var recordId = HttpQueryString.GetValue("id").ToInt();

            if (recordId <= 0) return;

            CurrentPage = SysPageService.Instance.GetByID(recordId);

            if (CurrentPage != null)
                CurrentTemplate = SysTemplateService.Instance.GetByID(CurrentPage.TemplateID);
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (CurrentTemplate == null || CurrentPage == null)
            {
                Response.End();
                return;
            }

            if (CurrentUser?.IsAdministrator != true)
            {
                Response.End();
                return;
            }

            var masterPageFile = "~/Views/Design/" + CurrentTemplate.File;
            if (!File.Exists(Server.MapPath(masterPageFile)))
            {
                Response.End();
                return;
            }

            MasterPageFile = masterPageFile;

            base.OnPreInit(e);
        }
    }
}