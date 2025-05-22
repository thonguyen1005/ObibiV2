using VSW.Core.Services;
using VSW.Core;
using System.Linq;
using VSW.Website.Models;
using VSW.Website.Interface;

namespace VSW.Website.Middleware
{
    public class DynamicRouteMiddleware
    {
        private readonly RequestDelegate _next;

        public DynamicRouteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ISiteServiceInterface siteService, IPageServiceInterface pageService, ITemplateServiceInterface templateService)
        {
            var appSetting = CoreService.GetConfigWithSection<AppsSetting>();
            var endpoint = context.GetEndpoint();
            if (endpoint != null && endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>() != null)
            {
                var routeValues = context.Request.RouteValues;
                string controller = routeValues["controller"]?.ToString();
                string action = routeValues["action"]?.ToString();
                string siteCode = routeValues["site"]?.ToString();

                if (action != "Error")
                {
                    ISiteInterface siteInterface = null;
                    if (appSetting.MultiSite && siteCode.IsNotEmpty())
                    {
                        siteInterface = siteService.VSW_Core_GetByCode(siteCode);
                    }

                    if (siteInterface == null) siteInterface = siteService.VSW_Core_GetDefault();
                    context.Items["Site"] = siteInterface;
                }
            }
            else
            {
                var path = context.Request.Path.Value;
                if (!IsStaticFile(path, appSetting.StaticFileExtensions))
                {
                    var vqs = new VQS(path);
                    ISiteInterface siteInterface = null;
                    string siteCode = "";
                    if (appSetting.MultiSite)
                    {
                        siteCode = vqs.BeginCode;
                        siteInterface = siteService.VSW_Core_GetByCode(siteCode);
                    }
                    if (siteInterface == null) siteInterface = siteService.VSW_Core_GetDefault();
                    context.Items["Site"] = siteInterface;
                    context.Items["VQS"] = vqs;

                    Tuple<IPageInterface, string> tuple = null;
                    IPageInterface pageInterface = null;
                    if (vqs.EndCode.IsNotEmpty())
                    {
                        tuple = pageService.VSW_Core_CurrentPage(vqs, siteInterface.LangID);
                    }
                    else
                    {
                        tuple = pageService.VSW_Core_GetByID(siteInterface.PageID);
                    }

                    pageInterface = tuple.Item1;
                    if(pageInterface != null)
                    {
                        pageInterface.Items = new Custom(pageInterface.Custom);
                        context.Items["Page"] = pageInterface;

                        ITemplateInterface templateInterface = templateService.VSW_Core_GetByID(pageInterface.TemplateID);
                        templateInterface.Items = new Custom(templateInterface.Custom);
                        context.Items["Template"] = templateInterface;

                        if (appSetting.MultiSite)
                        {
                            if (!templateInterface.Custom.Contains("VSWMODULE"))
                            {
                                context.Request.Path = "/" + siteInterface.Code + "/MEmpty/Index";
                            }
                            else
                            {
                                context.Request.Path = "/" + siteInterface.Code + tuple.Item2;
                            }
                        }
                        else
                        {
                            if (!templateInterface.Custom.Contains("VSWMODULE"))
                            {
                                context.Request.Path = "/MEmpty/Index";
                            }
                            else
                            {
                                context.Request.Path = tuple.Item2;
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
        private bool IsStaticFile(string path, string[] arrExtensions)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return arrExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
    }
}
