using System.Collections.Generic;
using System.Data;
using VSW.Core.Global;

namespace VSW.Lib.Global.ListItem
{
    public static class List
    {
        public static List<Item> GetListByConfigkey(string configkey)
        {
            return GetListByText(Config.GetValue(configkey).ToString());
        }

        public static List<Item> GetListByText(string listText)
        {
            var list = new List<Item>();

            var items = listText.Split(',');
            foreach (var t in items)
            {
                if (t.IndexOf('|') == -1)
                    list.Add(new Item(t, t));
                else
                {
                    var name = t.Split('|')[0];
                    var value = t.Split('|')[1];

                    list.Add(new Item(name, value));
                }
            }

            return list;
        }

        public static int GetLevel(string text)
        {
            var level = 0;
            foreach (var t in text)
            {
                if (t == 45)
                    level++;
                else
                    break;
            }
            return level / 4;
        }

        public static List<Item> GetListForEdit(List<Item> list, int parentId)
        {
            var items = new List<Item>();

            var found = false;
            var level = 0;
            foreach (var t in list)
            {
                if (found && level < GetLevel(t.Name))
                    continue;

                if (!found && t.Value == parentId.ToString())
                {
                    found = true;
                    level = GetLevel(t.Name);

                    continue;
                }

                items.Add(t);
            }

            return items;
        }

        public static Item FindByName(List<Item> list, string name)
        {
            var obj = list.Find(s => s.Name == name);

            return obj ?? new Item(string.Empty, string.Empty);
        }

        private static List<Item> _list;

        public static List<Item> GetList(object serviceBase)
        {
            return GetList(serviceBase, 0, string.Empty, string.Empty);
        }

        public static List<Item> GetList(object serviceBase, int langId)
        {
            return GetList(serviceBase, langId, string.Empty, string.Empty);
        }

        public static List<Item> GetList(object serviceBase, int langId, string type)
        {
            return GetList(serviceBase, langId, type, string.Empty);
        }

        public static List<Item> GetList(dynamic serviceBase, int langId, string type, string where)
        {
            _list = new List<Item>();

            var langUnAbc = Config.GetValue("Mod.LangUnABC").ToBool();

            string sSql = "SELECT [ID],[Name] " + (langUnAbc ? "+ ' [' + ISNULL([Code],'-') + ']' AS [Name]" : "") + ",[ParentID],[Order] FROM " + serviceBase.TableName + " WHERE 1=1";

            if (langId > 0)
                sSql += " AND [LangID]=" + langId;

            if (type != string.Empty)
                sSql += " AND [Type]='" + type + "'";

            if (where != string.Empty)
                sSql += " AND " + where;

            sSql += " ORDER BY [ID]";

            var cacheKey = Core.Web.Cache.CreateKey(Security.Md5(sSql));
            var obj = Core.Web.Cache.GetValue(cacheKey);
            if (obj != null)
                return obj as List<Item>;

            var dtData = serviceBase.ExecuteDataTable(sSql);

            BuildItem(dtData, 0, string.Empty);

            Core.Web.Cache.SetValue(cacheKey, _list);

            return _list;
        }

        private static void BuildItem(DataTable dtData, int parentId, string space)
        {
            foreach (var t in dtData.Select("ParentID=" + parentId, "Order"))
            {
                var text = space + " " + t["Name"].ToString().Trim();

                parentId = Convert.ToInt(t["ID"]);

                _list.Add(new Item(text, parentId.ToString()));

                BuildItem(dtData, parentId, space + "----");
            }
        }

        public static List<Item> GetListCode(object serviceBase, int langId, string type)
        {
            return GetListCode(serviceBase, langId, type, string.Empty);
        }

        public static List<Item> GetListCode(dynamic serviceBase, int langId, string type, string where)
        {
            _list = new List<Item>();

            var langUnAbc = Config.GetValue("Mod.LangUnABC").ToBool();

            string sSql = "SELECT [ID],[Code],[Name] " + (langUnAbc ? "+ ' [' + ISNULL([Code],'-') + ']' AS [Name]" : "") + ",[ParentID],[Order] FROM " + serviceBase.TableName + " WHERE 1=1";

            if (langId > 0)
                sSql += " AND [LangID]=" + langId;

            if (type != string.Empty)
                sSql += " AND [Type]='" + type + "'";

            if (where != string.Empty)
                sSql += " AND " + where;

            sSql += " ORDER BY [ID]";

            var cacheKey = Core.Web.Cache.CreateKey(Security.Md5(sSql));
            var obj = Core.Web.Cache.GetValue(cacheKey);
            if (obj != null)
                return obj as List<Item>;

            var dtData = serviceBase.ExecuteDataTable(sSql);

            BuildItemCode(dtData, 0, string.Empty);

            Core.Web.Cache.SetValue(cacheKey, _list);

            return _list;
        }

        private static void BuildItemCode(DataTable dtData, int parentId, string space)
        {
            foreach (var t in dtData.Select("ParentID=" + parentId, "Order"))
            {
                var text = space + " " + t["Name"].ToString().Trim();
                var code = space + " " + t["Code"].ToString().Trim();

                parentId = Convert.ToInt(t["ID"]);
                _list.Add(new Item(text, code));
                BuildItem(dtData, parentId, space + "----");
            }
        }
    }
}