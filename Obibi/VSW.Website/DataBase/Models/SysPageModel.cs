namespace VSW.Website.DataBase.Models
{
    public class SysPageModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public string File { get; set; }
        public string Summary { get; set; }
        public int MenuID { get; set; }
        public int BrandID { get; set; }
        public int ParentID { get; set; }
        public int Order { get; set; }
        public int Level { get; set; }

    }
}
