using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModUrlSeoActivityEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public string Url { get; set; }

        [DataInfo]
        public string UrlRedirect { get; set; }

        [DataInfo]
        public string PageTitle { get; set; }

        [DataInfo]
        public string PageDescription { get; set; }

        [DataInfo]
        public string PageKeywords { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public int CountSearch { get; set; }
        [DataInfo]
        public string File { get; set; }
        [DataInfo]
        public string SchemaJson { get; set; }

        #endregion Autogen by VSW
    }

    public class ModUrlSeoActivityService : ServiceBase<ModUrlSeoActivityEntity>
    {
        #region Autogen by VSW

        public ModUrlSeoActivityService() : base("[Mod_UrlSeoActivity]")
        {
        }

        private static ModUrlSeoActivityService _instance;
        public static ModUrlSeoActivityService Instance => _instance ?? (_instance = new ModUrlSeoActivityService());

        #endregion Autogen by VSW

        public ModUrlSeoActivityEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModUrlSeoActivityEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
        public ModUrlSeoActivityEntity GetByUrl(string url)
        {
            return CreateQuery()
               .Where(o => o.Url == url)
               .ToSingle_Cache();
        }
        public bool CheckUrl(string url)
        {
            return CreateQuery()
               .Where(o => o.Url == url)
               .Count().ToValue_Cache().ToInt() > 0;
        }
        public bool CheckUrlRedirect(string urlRedirect)
        {
            return CreateQuery()
               .Where(o => o.UrlRedirect == urlRedirect)
               .Count().ToValue_Cache().ToInt() > 0;
        }
        public ModUrlSeoActivityEntity GetByUrlRedirect(string url)
        {
            return CreateQuery()
               .Where(o => o.UrlRedirect == url)
               .ToSingle_Cache();
        }
        public ModUrlSeoActivityEntity GetByUrlCache(string url)
        {
            url = url.Replace(Core.Web.HttpRequest.Domain, "");
            if (url.StartsWith("/")) url = url.Remove(0, 1);
            return CreateQuery()
               .Where(o => o.Url == url || (o.UrlRedirect != "" && o.UrlRedirect == url))
               .ToSingle_Cache();
        }
        public bool CheckByUrlCache(string url)
        {
            url = url.Replace(Core.Web.HttpRequest.Domain, "");
            if (url.StartsWith("/")) url = url.Remove(0, 1);
            return CreateQuery()
               .Where(o => o.Url == url || (o.UrlRedirect != "" && o.UrlRedirect == url))
               .Count().ToValue_Cache().ToInt() > 0;
        }
    }
}