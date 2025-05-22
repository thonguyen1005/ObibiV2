using VSW.Lib.Global;

namespace VSW.Lib.MVC
{
    public class ViewControl : Core.MVC.ViewControl
    {
        private ViewPage _viewPageGet => Page as ViewPage;
        private ViewPage _viewPage { get; set; }
        public ViewPage ViewPage
        {
            get
            {
                return (_viewPage != null ? _viewPage : _viewPageGet);
            }
            set
            {
                _viewPage = value;
            }
        }


        protected string GetPagination(int pageIndex, int pageSize, int totalRecord)
        {
            return GetPagination(ViewPage.CurrentURL, pageIndex, pageSize, totalRecord);
        }

        protected string GetPagination(string url, int pageIndex, int pageSize, int totalRecord)
        {
            var pager = new Pager { Url = url, PageIndex = pageIndex, PageSize = pageSize, TotalRecord = totalRecord, IsMobile = ViewPage.MobileMode };

            pager.Update();
            
            return pager.Html;
        }
    }
}