using System;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace VSW.Lib.Global
{
    public static class Static
    {
        public static string Tag(string path)
        {
            if (HttpRuntime.Cache[path] != null) return HttpRuntime.Cache[path] as string;

            var absolute = HostingEnvironment.MapPath("~" + path);
            if (!File.Exists(absolute)) return string.Empty;

            var date = System.IO.File.GetLastWriteTime(absolute ?? throw new InvalidOperationException());
            var index = path.LastIndexOf('/');

            var result = path.Insert(index, "/v-" + date.Ticks);
            HttpRuntime.Cache.Insert(path, result, new CacheDependency(absolute));

            return HttpRuntime.Cache[path] as string;
        }
    }
}