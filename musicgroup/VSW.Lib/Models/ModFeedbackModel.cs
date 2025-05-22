using System;

using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModFeedbackEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Email { get; set; }

        [DataInfo]
        public string Phone { get; set; }

        [DataInfo]
        public string Address { get; set; }

        [DataInfo]
        public string Title { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public string IP { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }
        [DataInfo]
        public string Company { get; set; }
        [DataInfo]
        public string DiaChiThue { get; set; }
        [DataInfo]
        public string SoNguoi { get; set; }
        [DataInfo]
        public DateTime FromDate { get; set; }
        [DataInfo]
        public DateTime ToDate { get; set; }
        [DataInfo]
        public int ThietBiID { get; set; }
        [DataInfo]
        public int MoiTruongID { get; set; }
        #endregion Autogen by VSW

        public string Time
        {
            get
            {
                if ((DateTime.Now - Created).TotalDays >= 365) return (DateTime.Now - Created).Days / 365 + " năm trước.";
                if ((DateTime.Now - Created).TotalDays >= 30) return (DateTime.Now - Created).Days / 30 + " tháng trước.";
                if ((DateTime.Now - Created).TotalDays >= 7) return (DateTime.Now - Created).Days / 7 + " tuần trước.";
                if ((DateTime.Now - Created).TotalDays >= 1) return (DateTime.Now - Created).Days + " ngày trước.";
                if ((DateTime.Now - Created).TotalHours >= 1) return (DateTime.Now - Created).Hours + " giờ trước.";
                if ((DateTime.Now - Created).TotalMinutes >= 1) return (DateTime.Now - Created).Minutes + " phút trước.";
                return (DateTime.Now - Created).Seconds + " giây trước.";
            }
        }
    }

    public class ModFeedbackService : ServiceBase<ModFeedbackEntity>
    {
        #region Autogen by VSW

        public ModFeedbackService() : base("[Mod_Feedback]")
        {
        }

        private static ModFeedbackService _instance;
        public static ModFeedbackService Instance => _instance ?? (_instance = new ModFeedbackService());

        #endregion Autogen by VSW

        public ModFeedbackEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
    }
}