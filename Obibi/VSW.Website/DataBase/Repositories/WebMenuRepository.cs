using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Core.Caching;
using VSW.Core;

namespace VSW.Website.DataBase.Repositories
{
    public class WebMenuRepository : SqlRepository<WEB_MENUEntity, int>
    {
        private ILocalCache _cache;
        public WebMenuRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
            _cache = CoreService.LocalCache;
        }

        public List<WEB_MENUEntity> ShowChildMenuByType(string type, int langId)
        {
            var keyCache = type + "." + langId;
            if (_cache != null && _cache.HasKey(keyCache))
            {
                return _cache.Get<List<WEB_MENUEntity>>(keyCache);
            }

            var list = this.GetTable()
                        .Where(o => o.ParentID == 0 && o.LangID == langId && o.Type == type)
                        .OrderBy(o => o.Order)
                        .ToList();

            if (list.IsEmpty()) return null;

            var lstData = this.GetTable()
                            .Where(o => o.ParentID == list.FirstOrDefault().ID)
                            .OrderBy(o => o.Order)
                            .ToList();

            _cache.Set(keyCache, lstData);

            return lstData;
        }

        public List<WEB_MENUEntity> ShowAllMenuByType(string type, int langId)
        {
            var keyCache = type + "." + langId + ".Multi";
            if (_cache != null && _cache.HasKey(keyCache))
            {
                return _cache.Get<List<WEB_MENUEntity>>(keyCache);
            }

            string sql = @"
                        WITH MenuCTE AS (
                            SELECT 
                                [ID]
                                ,[Name]
                                ,[Code]
                                ,ParentID
                                ,0 AS Level
                            FROM [Web_Menu]
                            WHERE ParentID = 0 AND Activity = 1 AND LangID = " + langId + @" And Type = '" + type + @"'

	                        UNION ALL

                            SELECT m.[ID]
                                ,m.[Name]
                                ,m.[Code]
                                ,m.ParentID
                                ,1 AS Level
                            FROM [Web_Menu] m
                            INNER JOIN MenuCTE cte ON m.[ParentID] = cte.id
                        )
                        SELECT * FROM MenuCTE
                        ";

            var lstData = this.WithSqlText(sql).Query<WEB_MENUEntity>();

            _cache.Set(keyCache, lstData);

            return lstData;
        }

        public List<int> GetChildIDForWeb_Cache(string type, int menuId, int langId)
        {
            var keyCache = "Lib.App.WebMenu.GetChildIDForWeb." + type + "." + menuId + "." + langId;

            if (_cache != null && _cache.HasKey(keyCache))
            {
                return _cache.Get<List<int>>(keyCache);
            }

            var list = new List<int>();

            var listWebMenu = this.GetTable()
                                .Where(o => o.Activity == true && o.LangID == langId)
                                .Where(o => o.Type == type)
                                .Select(o => new WEB_MENUEntity { ID = o.ID, ParentID = o.ParentID })
                                .ToList();

            GetChildIDForWeb_Cache(ref list, listWebMenu, menuId);

            return list;
        }
        private void GetChildIDForWeb_Cache(ref List<int> list, List<WEB_MENUEntity> listWebMenu, int menuId)
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
    }
}

