using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class WebMenuEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int LangID { get; set; }

        [DataInfo]
        public int ParentID { get; set; }

        [DataInfo]
        public int PropertyID { get; set; }

        [DataInfo]
        public int State { get; set; }

        [DataInfo]
        public string Type { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string CityCode { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public string Summary { get; set; }

        [DataInfo]
        public string Custom { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public int GHNID { get; set; }
        #endregion Autogen by VSW

        public bool Root => ParentID == 0;

        private WebMenuEntity _oParent;

        public WebMenuEntity Parent
        {
            get
            {
                if (_oParent == null)
                    _oParent = WebMenuService.Instance.GetByID_Cache(ParentID);

                return _oParent ?? (_oParent = new WebMenuEntity());
            }
        }
    }

    public class WebMenuService : ServiceBase<WebMenuEntity>
    {
        #region Autogen by VSW
        public WebMenuService() : base("[Web_Menu]")
        {
        }

        private static WebMenuService _instance;

        public static WebMenuService Instance => _instance ?? (_instance = new WebMenuService());

        #endregion Autogen by VSW

        public WebMenuEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public WebMenuEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }

        public WebMenuEntity GetByCode_Cache(string code)
        {
            return CreateQuery()
               .Where(o => o.Code == code)
               .ToSingle_Cache();
        }

        public WebMenuEntity GetByCode_Cache(string type, string code)
        {
            return CreateQuery()
               .Where(o => o.Code == code && o.Type == type)
               .ToSingle_Cache();
        }
        public WebMenuEntity GetByCode_Cache(string type, string code, int parent)
        {
            return CreateQuery()
               .Where(o => o.Code == code && o.Type == type && o.ParentID == parent)
               .ToSingle_Cache();
        }
        public List<WebMenuEntity> GetByParentID_Cache(int parentId)
        {
            return CreateQuery()
               .Where(o => o.ParentID == parentId)
               .OrderByAsc(o => new { o.Order, o.ID })
               .ToList_Cache();
        }
        public List<WebMenuEntity> GetByParentIDAndType_Cache(string type, int parentId)
        {
            return CreateQuery()
               .Where(o => o.ParentID == parentId && o.Type == type)
               .OrderByAsc(o => new { o.Order, o.ID })
               .ToList_Cache();
        }
        public List<WebMenuEntity> GetCity()
        {
            var root = CreateQuery()
                            .Where(o => o.Type == "City" && o.ParentID == 0)
                            .ToSingle_Cache();

            if (root != null)
            {
                return CreateQuery()
                               .Where(o => o.Type == "City" && o.ParentID == root.ID)
                               .OrderByAsc(o => new { o.Order, o.ID })
                               .ToList_Cache();
            }

            return null;
        }

        public List<WebMenuEntity> GetCity(int state)
        {
            var listItem = GetCity();

            return listItem?.FindAll(o => o.State == state);
        }

        public string GetChildIDForCP(int menuId, int langId)
        {
            return GetChildIDForCP(string.Empty, menuId, langId);
        }

        public string GetChildIDForCP(string type, int menuId, int langId)
        {
            var list = new List<int>();

            var listWebMenu = CreateQuery()
                      .Where(o => o.LangID == langId)
                      .Where(type != string.Empty, o => o.Type == type)
                      .Select(o => new { o.ID, o.ParentID })
                      .ToList();

            GetChildIDForCP(ref list, listWebMenu, menuId);

            return Core.Global.Array.ToString(list.ToArray());
        }

        private static void GetChildIDForCP(ref List<int> list, List<WebMenuEntity> listWebMenu, int menuId)
        {
            list.Add(menuId);

            if (listWebMenu == null)
                return;

            foreach (var t in listWebMenu.FindAll(o => o.ParentID == menuId))
            {
                GetChildIDForCP(ref list, listWebMenu, t.ID);
            }
        }

        public string GetChildIDForWeb_Cache(int menuId, int langId)
        {
            return GetChildIDForWeb_Cache(string.Empty, menuId, langId);
        }

        public string GetChildIDForWeb_Cache(string type, int menuId, int langId)
        {
            var keyCache = "Lib.App.WebMenu.GetChildIDForWeb." + type + "." + menuId + "." + langId;

            string cacheValue;
            var obj = Core.Web.Cache.GetValue(keyCache);
            if (obj != null)
            {
                cacheValue = obj.ToString();
            }
            else
            {
                var list = new List<int>();

                var listWebMenu = CreateQuery()
                                    .Where(o => o.Activity == true && o.LangID == langId)
                                    .Where(type != string.Empty, o => o.Type == type)
                                    .Select(o => new { o.ID, o.ParentID })
                                    .ToList_Cache();

                GetChildIDForWeb_Cache(ref list, listWebMenu, menuId);

                cacheValue = Core.Global.Array.ToString(list.ToArray());

                Core.Web.Cache.SetValue(keyCache, cacheValue);
            }

            return cacheValue;
        }

        private static void GetChildIDForWeb_Cache(ref List<int> list, List<WebMenuEntity> listWebMenu, int menuId)
        {
            list.Add(menuId);

            if (listWebMenu == null)
                return;

            var listMenu = listWebMenu.FindAll(o => o.ParentID == menuId);

            foreach (var t in listMenu)
            {
                GetChildIDForWeb_Cache(ref list, listWebMenu, t.ID);
            }
        }

        private string html_map = "";
        public string GetMapMenu(int MenuID)
        {
            if (MenuID <= 0) return string.Empty;

            html_map = "";

            GetMapValue(MenuID, "");

            return html_map;
        }
        private void GetMapValue(int MenuID, string map)
        {
            var item = WebMenuService.Instance.GetByID_Cache(MenuID);
            if (item != null)
            {
                if (item.ParentID > 0)
                {
                    GetMapValue(item.ParentID, item.Name + (!string.IsNullOrEmpty(map) ? "=>" : "") + map);
                }
                else
                {
                    html_map = item.Name + "=>" + map;
                }
            }
        }
    }
}