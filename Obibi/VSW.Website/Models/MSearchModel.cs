namespace VSW.Website.Models
{
    public class MSearchModel
    {
        private int _page = 1;
        public int Page
        {
            get { return _page; }
            set { _page = value; }
        }
        public int TotalRecord { get; set; }
        public int PageSize { get; set; }
        public string keyword { get; set; }
        public int searchmenuid { get; set; }
    }
}
