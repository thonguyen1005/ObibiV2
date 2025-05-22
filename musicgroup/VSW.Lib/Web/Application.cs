using System;
using System.Collections.Generic;

using System.Reflection;
using System.Web;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Web
{

    public class Application : HttpApplication
    {
        #region private

        private void Redirection()
        {
            var absoluteUri = Request.Url.AbsoluteUri.ToLower();
            if (absoluteUri.Length >= 260) Response.Redirect(Core.Web.HttpRequest.Domain);

            var listRedirection = WebRedirectionService.Instance.CreateQuery().ToList_Cache();

            if (listRedirection == null) return;
            var index = listRedirection.FindIndex(o => o.Url == absoluteUri);
            if (index <= -1 || string.IsNullOrEmpty(listRedirection[index].Redirect)) return;

            Core.Web.HttpRequest.Redirect301(listRedirection[index].Redirect);
        }

        #endregion private

        public static List<CPModuleInfo> CPModules { get; set; }
        public new static List<ModuleInfo> Modules { get; set; }

        protected void Application_Start(object sender, EventArgs e)
        {
            //license excel
            var licenseFile = HttpContext.Current.Server.MapPath("~/bin/Aspose.Cells.lic");
            if (System.IO.File.Exists(licenseFile))
            {
                Aspose.Cells.License license = new Aspose.Cells.License();
                license.SetLicense(licenseFile);
            }

            if (CPModules != null) return;

            CPModules = new List<CPModuleInfo>();
            Modules = new List<ModuleInfo>();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attributes = type.GetCustomAttributes(typeof(CPModuleInfo), true);
                if (attributes.GetLength(0) == 0)
                {
                    attributes = type.GetCustomAttributes(typeof(ModuleInfo), true);
                    if (attributes.GetLength(0) == 0)
                        continue;

                    if (attributes[0] is ModuleInfo moduleInfo && Modules.Find(o => o.Code == moduleInfo.Code) == null)
                    {
                        moduleInfo.ModuleType = type;

                        Modules.Add(moduleInfo);
                    }

                    continue;
                }

                {
                    if (!(attributes[0] is CPModuleInfo moduleInfo)) continue;

                    if (CPModules.Find(o => o.Code == moduleInfo.Code) == null)
                        CPModules.Add(moduleInfo);
                }
            }
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            Core.Web.Application.PreRequestHandlerExecute(sender as HttpApplication);
            //HttpApplication app = (HttpApplication)sender;
            //string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            //System.IO.Stream prevUncompressedStream = app.Response.Filter;

            //if (acceptEncoding == null || acceptEncoding.Length == 0)
            //    return;

            //acceptEncoding = acceptEncoding.ToLower();

            //if (acceptEncoding.Contains("gzip"))
            //{
            //    // gzip
            //    app.Response.Filter = new System.IO.Compression.GZipStream(prevUncompressedStream,
            //        System.IO.Compression.CompressionMode.Compress);
            //    app.Response.AppendHeader("Content-Encoding",
            //        "gzip");
            //}
            //else if (acceptEncoding.Contains("deflate"))
            //{
            //    // defalte
            //    app.Response.Filter = new System.IO.Compression.DeflateStream(prevUncompressedStream,
            //        System.IO.Compression.CompressionMode.Compress);
            //    app.Response.AppendHeader("Content-Encoding",
            //        "deflate");
            //}
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Redirection();
            Core.Web.Application.BeginRequest();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Global.Application.OnError();
        }
    }
}