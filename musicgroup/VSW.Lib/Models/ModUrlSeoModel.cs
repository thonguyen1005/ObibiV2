using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModUrlSeoEntity : EntityBase
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

    public class ModUrlSeoService : ServiceBase<ModUrlSeoEntity>
    {
        #region Autogen by VSW

        public ModUrlSeoService() : base("[Mod_UrlSeo]")
        {
        }

        private static ModUrlSeoService _instance;
        public static ModUrlSeoService Instance => _instance ?? (_instance = new ModUrlSeoService());

        #endregion Autogen by VSW

        public ModUrlSeoEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModUrlSeoEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
        public ModUrlSeoEntity GetByUrl(string url)
        {
            return CreateQuery()
               .Where(o => o.Url == url)
               .ToSingle_Cache();
        }
        public ModUrlSeoEntity GetByUrlRedirect(string url)
        {
            return CreateQuery()
               .Where(o => o.UrlRedirect == url)
               .ToSingle_Cache();
        }
        public bool CheckUrl(string url)
        {
            return CreateQuery()
               .Where(o => o.Url == url)
               .Count().ToValue_Cache().ToInt() > 0;
        }
        public ModUrlSeoEntity GetByUrlCache(string url)
        {
            url = url.Replace(Core.Web.HttpRequest.Domain, "");
            if(url.StartsWith("/")) url = url.Remove(0, 1);
            return CreateQuery()
               .Where(o => o.Url == url || (o.UrlRedirect != "" && o.UrlRedirect == url))
               .ToSingle_Cache();
        }
    }
}