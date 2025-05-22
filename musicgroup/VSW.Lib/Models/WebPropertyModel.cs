using System.Collections.Generic;
using VSW.Core.Global;
using VSW.Core.Models;
using VSW.Core.Web;

namespace VSW.Lib.Models
{
    public class WebPropertyEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int LangID { get; set; }

        [DataInfo]
        public int ParentID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public bool Multiple { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public bool ShowFilterFast { get; set; }
        [DataInfo]
        public bool IsMenu { get; set; }

        #endregion Autogen by VSW

        public int Count { get; set; }
        public bool Selected { get; set; }

        public bool Root { get { return ParentID == 0; } }

        private WebPropertyEntity _oParent;

        public WebPropertyEntity Parent
        {
            get
            {
                if (_oParent == null)
                    _oParent = WebPropertyService.Instance.GetByID_Cache(ParentID);

                return _oParent ?? (_oParent = new WebPropertyEntity());
            }
        }
    }

    public class WebPropertyService : ServiceBase<WebPropertyEntity>
    {
        #region Autogen by VSW

        public WebPropertyService() : base("[Web_Property]")
        {
        }

        private static WebPropertyService _instance;
        public static WebPropertyService Instance => _instance ?? (_instance = new WebPropertyService());

        #endregion Autogen by VSW

        public WebPropertyEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public WebPropertyEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
        public WebPropertyEntity GetByID_Cache(int id, int langID)
        {
            return CreateQuery()
               .Where(o => o.ID == id && o.LangID == langID)
               .ToSingle_Cache();
        }

        public List<WebPropertyEntity> GetByParent_Cache(int parentID)
        {
            if (GetAll_Cache() == null) return null;

            var list = GetAll_Cache().FindAll(o => o.ParentID == parentID);

            if (list.Count == 0) return null;

            list.Sort((o1, o2) => o1.Order.CompareTo(o2.Order));

            return list;
        }

        public string GetChildIDForCP(int propertyID, int langID)
        {
            var list = new List<int>();

            var listAllProperty = CreateQuery()
                                    .Where(o => o.LangID == langID)
                                    .Select(o => new { o.ID, o.ParentID })
                                    .ToList();

            GetChildIDForCP(ref list, listAllProperty, propertyID);

            return Array.ToString(list.ToArray());
        }

        private static void GetChildIDForCP(ref List<int> list, List<WebPropertyEntity> listAllProperty, int propertyID)
        {
            list.Add(propertyID);

            if (listAllProperty == null) return;

            foreach (var t in listAllProperty.FindAll(o => o.ParentID == propertyID))
            {
                GetChildIDForCP(ref list, listAllProperty, t.ID);
            }
        }

        public string GetChildIDForWeb_Cache(int propertyID, int langID)
        {
            var keyCache = "Lib.App.WebProperty.GetChildIDForWeb." + propertyID + "." + langID;

            string cacheValue;
            var obj = Cache.GetValue(keyCache);
            if (obj != null)
            {
                cacheValue = obj.ToString();
            }
            else
            {
                var list = new List<int>();

                var listAllProperty = CreateQuery()
                                        .Where(o => o.Activity == true && o.LangID == langID)
                                        .Select(o => new { o.ID, o.ParentID })
                                        .ToList_Cache();

                GetChildIDForWeb_Cache(ref list, listAllProperty, propertyID, langID);

                cacheValue = Array.ToString(list.ToArray());

                Cache.SetValue(keyCache, cacheValue);
            }

            return cacheValue;
        }

        private static void GetChildIDForWeb_Cache(ref List<int> list, List<WebPropertyEntity> listAllProperty, int propertyID, int langID)
        {
            list.Add(propertyID);

            if (listAllProperty == null)
                return;

            foreach (var t in listAllProperty.FindAll(o => o.ParentID == propertyID && o.LangID == langID))
            {
                GetChildIDForWeb_Cache(ref list, listAllProperty, t.ID, langID);
            }
        }

        private List<WebPropertyEntity> GetAll_Cache()
        {
            return CreateQuery().Where(o => o.Activity == true).ToList_Cache();
        }
    }
}