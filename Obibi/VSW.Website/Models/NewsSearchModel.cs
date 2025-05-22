namespace VSW.Website.Models
{
    public class NewsSearchModel
    {
        private int _page = 1;
        public int Page
        {
            get { return _page; }
            set { _page = value; }
        }

        public int PageSize { get; set; }
        public int TotalRecord { get; set; }
    }
}
