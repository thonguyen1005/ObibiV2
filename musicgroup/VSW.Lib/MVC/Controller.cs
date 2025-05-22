namespace VSW.Lib.MVC
{
    public class Controller : Core.MVC.Controller
    {
        public ViewPage ViewPage => ViewPageBase as ViewPage;
        public new ViewControl ViewControl => base.ViewControl as ViewControl;
    }
}