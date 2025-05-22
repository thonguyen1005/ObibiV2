using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModCongTrinhEntity : EntityBase
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
        public string File { get; set; }

        [DataInfo]
        public string Summary { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public long View { get; set; }
        [DataInfo]
        public string PageTitle { get; set; }

        [DataInfo]
        public string PageDescription { get; set; }

        [DataInfo]
        public string PageKeywords { get; set; }

        [DataInfo]
        public DateTime Published { get; set; }

        [DataInfo]
        public DateTime Updated { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

        #endregion Autogen by VSW

        public string Time
        {
            get
            {
                var ts = DateTime.Now - Published;

                if (ts.TotalDays >= 365) return ts.Days / 365 + " năm trước.";
                if (ts.TotalDays >= 30) return ts.Days / 30 + " tháng trước.";
                if (ts.TotalDays >= 7) return ts.Days / 7 + " tuần trước.";
                if (ts.TotalDays >= 1) return ts.Days + " ngày trước.";
                if (ts.TotalHours >= 1) return ts.Hours + " giờ trước.";
                if (ts.TotalMinutes >= 1) return ts.Minutes + " phút trước.";
                return ts.Seconds + " giây trước.";
            }
        }

        public void UpView()
        {
            View++;
            ModCongTrinhService.Instance.Save(this, o => o.View);
        }

        private WebMenuEntity _oMenu;

        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }
    }

    public class ModCongTrinhService : ServiceBase<ModCongTrinhEntity>
    {
        #region Autogen by VSW

        public ModCongTrinhService() : base("[Mod_CongTrinh]")
        {
        }

        private static ModCongTrinhService _instance;
        public static ModCongTrinhService Instance => _instance ?? (_instance = new ModCongTrinhService());

        #endregion Autogen by VSW

        public ModCongTrinhEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public ModCongTrinhEntity GetByID_Cache(int id)
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