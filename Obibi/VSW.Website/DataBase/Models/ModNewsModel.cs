namespace VSW.Website.DataBase.Models
{
    public class ModNewsModel
    {
        public int ID { get; set; }
        public int MenuID { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Summary { get; set; }
        public string Tags { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeywords { get; set; }
        public string TopContent { get; set; }
        public string Content { get; set; }
        public string File { get; set; }
        public int View { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public int Order { get; set; }
        public bool Activity { get; set; }
        public int CPUserID { get; set; }
        public int AuthorID { get; set; }
        public string AuthorCode { get; set; }
        public string AuthorName { get; set; }
        public int TotalCount { get; set; }
        
    }
}
