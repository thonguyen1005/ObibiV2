using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModUrlSeoDashboardEntity : EntityBase
    {
        #region Autogen by VSW
        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public string Url { get; set; }
        [DataInfo]
        public string UrlRedirect { get; set; }
        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public int CountSearch { get; set; }

        #endregion Autogen by VSW
    }

    public class ModUrlSeoDashboardService : ServiceBase<ModUrlSeoDashboardEntity>
    {
        #region Autogen by VSW

        public ModUrlSeoDashboardService() : base("[Mod_UrlSeo]")
        {
            DBConfigKey = "DBConnectionDashboard";
        }

        private static ModUrlSeoDashboardService _instance;
        public static ModUrlSeoDashboardService Instance => _instance ?? (_instance = new ModUrlSeoDashboardService());

        #endregion Autogen by VSW

        public ModUrlSeoDashboardEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModUrlSeoDashboardEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
        public ModUrlSeoDashboardEntity GetByUrl(string url)
        {
            return CreateQuery()
               .Where(o => o.Url == url)
               .ToSingle();
        }
        public ModUrlSeoDashboardEntity GetByUrlRedirect(string url)
        {
            return CreateQuery()
               .Where(o => o.UrlRedirect == url)
               .ToSingle();
        }
        public bool CheckUrl(string url)
        {
            return CreateQuery()
               .Where(o => o.Url == url)
               .Count().ToValue_Cache().ToInt() > 0;
        }
        public ModUrlSeoDashboardEntity GetByUrlCache(string url)
        {
            url = url.Replace(Core.Web.HttpRequest.Domain, "");
            if(url.StartsWith("/")) url = url.Remove(0, 1);
            return CreateQuery()
               .Where(o => o.Url == url || (o.UrlRedirect != "" && o.UrlRedirect == url))
               .ToSingle_Cache();
        }
    }
}