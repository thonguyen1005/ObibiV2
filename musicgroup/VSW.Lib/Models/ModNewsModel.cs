using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModNewsEntity : EntityBase
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
        public string Custom { get; set; }
        [DataInfo]
        public string Tags { get; set; }
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
        [DataInfo]
        public bool HasFeedback { get; set; }
        [DataInfo]
        public int CPUserID { get; set; }
        [DataInfo]
        public string SchemaJson { get; set; }
        [DataInfo]
        public int AuthorID { get; set; }
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
            ModNewsService.Instance.Save(this, o => o.View);
        }

        private List<string> _ogetTag = null;
        public List<string> getTag()
        {
            if (_ogetTag == null)
            {
                string[] arrTag = this.Tags.Split(',');
                _ogetTag = new List<string>();

                for (int i = 0; arrTag != null && i < arrTag.Length; i++)
                    if (!string.IsNullOrEmpty(arrTag[i]))
                        _ogetTag.Add(arrTag[i]);
            }

            if (_ogetTag == null)
                _ogetTag = new List<string>();

            return _ogetTag;
        }

        private WebMenuEntity _oMenu;
        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }

        private ModAuthorEntity _oAuthor;
        public ModAuthorEntity GetAuthor()
        {
            if (_oAuthor == null && AuthorID > 0)
                _oAuthor = ModAuthorService.Instance.GetByID_Cache(AuthorID);

            return _oAuthor ?? (_oAuthor = new ModAuthorEntity());
        }

        private List<ModProductEntity> _oGetProduct;
        public List<ModProductEntity> GetProduct()
        {
            _oGetProduct = new List<ModProductEntity>();
            if ((_oGetProduct == null || _oGetProduct.Count < 1) && ID > 0)
            {
                var listItem = ModNewsProductService.Instance.CreateQuery()
                                                .Select(o => o.ProductID)
                                                .Where(o => o.NewsID == ID)
                                                .OrderByAsc(o => new { o.Order, o.ID })
                                                .ToList_Cache();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var item = ModProductService.Instance.GetDataSelectByID_Cache(listItem[i].ProductID);
                    if (item != null)
                        _oGetProduct.Add(item);
                }
            }

            return _oGetProduct;
        }
    }

    public class ModNewsService : ServiceBase<ModNewsEntity>
    {
        #region Autogen by VSW

        public ModNewsService() : base("[Mod_News]")
        {
        }

        private static ModNewsService _instance;
        public static ModNewsService Instance => _instance ?? (_instance = new ModNewsService());

        #endregion Autogen by VSW

        public ModNewsEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public ModNewsEntity GetByID_Cache(int id)
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