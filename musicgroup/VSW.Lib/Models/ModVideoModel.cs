using System;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModVideoEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public int State { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string Summary { get; set; }

        [DataInfo]
        public string PageTitle { get; set; }

        [DataInfo]
        public string PageDescription { get; set; }

        [DataInfo]
        public string PageKeywords { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public string Custom { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public int View { get; set; }

        [DataInfo]
        public DateTime Published { get; set; }

        [DataInfo]
        public DateTime Updated { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        public bool Check { get; set; }
        [DataInfo]
        public string Image { get; set; }
        #endregion Autogen by VSW

        //private string _thumbnail;
        //public string Thumbnail
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_thumbnail) && !string.IsNullOrEmpty(File))
        //        {
        //            string[] _ArrCode = System.Text.RegularExpressions.Regex.Split(File, "v=");
        //            if (_ArrCode.Length > 1) _thumbnail = "https://img.youtube.com/vi/" + _ArrCode[1] + "/0.jpg";
        //        }

        //        return _thumbnail;
        //    }
        //}

        public void UpView()
        {
            View++;
            ModVideoService.Instance.Save(this, o => o.View);
        }

        private WebMenuEntity _oMenu;

        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }
    }

    public class ModVideoService : ServiceBase<ModVideoEntity>
    {
        #region Autogen by VSW

        public ModVideoService()
            : base("[Mod_Video]")
        {
        }

        private static ModVideoService _instance;

        public static ModVideoService Instance
        {
            get { return _instance ?? (_instance = new ModVideoService()); }
        }

        #endregion Autogen by VSW

        public ModVideoEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModVideoEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
        public bool Exists(string query)
        {
            return CreateQuery()
                           .Where(query)
                           .Count()
                           .ToValue()
                           .ToBool();
        }
    }
}