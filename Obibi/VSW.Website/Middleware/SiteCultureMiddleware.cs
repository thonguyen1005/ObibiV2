using LinqToDB.Common;
using VSW.Core;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.Interface;

namespace VSW.Website.Middleware
{
    public class SiteCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public SiteCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILangServiceInterface langService, ISiteServiceInterface siteService, IPageServiceInterface pageService)
        {
            var path = context.Request.Path.Value;
            var endpoint = context.GetEndpoint();
            if (endpoint != null && endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>() != null)
            {
                var routeValues = context.Request.RouteValues;
                var controller = routeValues["controller"]?.ToString();
                var action = routeValues["action"]?.ToString();
                var siteCode = routeValues["site"]?.ToString();

                if (action != "Error")
                {
                    var appSetting = CoreService.GetConfigWithSection<AppsSetting>();
                    if (!appSetting.MultiSite)
                    {
                        var site = siteService.VSW_Core_GetDefault();
                        context.Items["Site"] = site;
                    }
                    else
                    {
                        if (siteCode.IsEmpty())
                        {
                            var site = siteService.VSW_Core_GetDefault();
                            context.Items["Site"] = site;
                        }
                        else
                        {
                            var site = siteService.VSW_Core_GetByCode(siteCode);
                            context.Items["Site"] = site;
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
