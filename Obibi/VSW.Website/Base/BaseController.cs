using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSW.Core;
using VSW.Core.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using VSW.Website.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VSW.Website.Helpers;
using System.Reflection;
using VSW.Core.Utils;

namespace VSW.Website
{
    //[Authorize]
    [RequestFormLimits(ValueCountLimit = int.MaxValue)]
    public abstract class BaseController : Controller
    {
        protected IWorkingContext Context { get; set; }

        protected string AppCode { get; set; }
        protected AppsSetting _appSetting { get; set; }

        protected IWebSession Session { get { return Context == null ? null : (Context.Session as IWebSession); } }

        protected ILogger Logger { get { return Context == null ? null : Context.Logger; } }

        public BaseController(IWorkingContext context)
        {
            Context = context;
            _appSetting = CoreService.GetConfigWithSection<AppsSetting>();
            AppCode = _appSetting.Code;
        }

        protected virtual JsonResult ToJsonResult(Result rs)
        {
            return Json(rs);
        }

        protected virtual JsonResult Success()
        {
            return Json(Result.Ok());
        }

        protected virtual JsonResult Success<TData>(TData data)
        {
            return Json(Result.Ok(data));
        }

        protected virtual JsonResult Error(string code, string message)
        {
            var rs = Result.Error(code, message);
            return ToJsonResult(rs);
        }

        protected virtual JsonResult Error<TData>(string message, TData data)
        {
            var rs = Result.Error(ResultExtensions.UNKNOW_CODE, message, data);
            return ToJsonResult(rs);
        }

        protected virtual JsonResult Error<TData>(string code, string message, TData data)
        {
            var rs = Result.Error(code, message, data);
            return ToJsonResult(rs);
        }

        protected virtual JsonResult Error(string message)
        {
            var rs = Result.Error(message);
            return ToJsonResult(rs);
        }

        protected virtual JsonResult Exception(string message, Exception ex)
        {
            var rs = Result.Exception(message, ex);
            if (Logger != null)
            {
                Logger.LogError(ex, message);
            }

            return ToJsonResult(rs);
        }


        protected virtual string RenderHtml(ViewResult result)
        {
            var feature = HttpContext.Features.Get<IRoutingFeature>();
            var routeData = feature.RouteData;
            var viewName = result.ViewName ?? routeData.Values["action"] as string;
            var actionContext = new ActionContext(HttpContext, routeData, new ControllerActionDescriptor());
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<MvcViewOptions>>();
            var htmlHelperOptions = options.Value.HtmlHelperOptions;
            var viewEngineResult = result.ViewEngine?.FindView(actionContext, viewName, true) ?? options.Value.ViewEngines.Select(x => x.FindView(actionContext, viewName, true)).FirstOrDefault(x => x != null);
            var view = viewEngineResult.View;
            var builder = new StringBuilder();

            using (var output = new StringWriter(builder))
            {
                var viewContext = new ViewContext(actionContext, view, result.ViewData, result.TempData, output, htmlHelperOptions);

                view.RenderAsync(viewContext)
                    .GetAwaiter()
                    .GetResult();
            }

            return builder.ToString();
        }

        protected virtual Result<string> RenderPartialHtml<TModel>(string viewNamePath, TModel model)
        {
            var success = RenderPartialHtml(viewNamePath, model, out string html);
            if (!success)
            {
                return Result.Error<string>("Có lỗi trong quá trình RenderPartialHtml");
            }
            return Result.Ok(html);
        }

        protected virtual bool RenderPartialHtml<TModel>(string viewNamePath, TModel model, out string html)
        {
            if (string.IsNullOrEmpty(viewNamePath))
            {
                viewNamePath = ControllerContext.ActionDescriptor.ActionName;
            }

            ViewData.Model = model;
            html = "";

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                    {
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    }
                    else
                    {
                        viewResult = viewEngine.FindView(ControllerContext, viewNamePath, false);
                    }

                    if (!viewResult.Success)
                    {
                        return false;
                    }

                    ViewContext viewContext = new ViewContext(
                        ControllerContext,
                        viewResult.View,
                        ViewData,
                        TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    viewResult.View.RenderAsync(viewContext)
                                 .GetAwaiter()
                                 .GetResult();

                    html = writer.GetStringBuilder().ToString();
                    return true;
                }
                catch (Exception exc)
                {
                    Context.Logger.LogError($"[RenderPartialHtml]: Failed - {exc.Message}");
                    return false;
                }
            }
        }

        protected virtual string GetUrl(string code)
        {
            if (_appSetting.MultiSite && code.IsNotEmpty())
            {
                var site = HttpContext.Items["Site"] as ISiteInterface;
                return "/" + site.Code + "/" + code;
            }
            return "/" + code;
        }
        protected virtual string GetUrlCurrentPage { get => GetUrl(CurrentPage.Code); }
        protected virtual IPageInterface CurrentPage { get => HttpContext.Items["Page"] as IPageInterface; }

        protected virtual ITemplateInterface CurrentTemplate { get => HttpContext.Items["Template"] as ITemplateInterface; }

        protected virtual ISiteInterface CurrentSite { get => HttpContext.Items["Site"] as ISiteInterface; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var template = CurrentTemplate;
            if (template != null)
            {
                ViewBag.Layout = template.File.Replace(".Master", "");
            }
            var page = CurrentPage;
            if (page != null)
            {
                ViewData["Title"] = (page.PageTitle.IsEmpty() ? page.Name : page.PageTitle);
                ViewData["Keyword"] = page.PageKeywords;
                ViewData["Description"] = page.PageDescription;
                ViewData["Canonical"] = Request.Scheme + "://" + Request.Host + GetUrlCurrentPage;

                ViewData["Image"] = Request.Scheme + "://" + Request.Host + ImageHelper.ResizeToWebp(page.File);
                ViewData["Image_Url"] = Request.Scheme + "://" + Request.Host + ImageHelper.ResizeToWebp(page.File);
            }
            base.OnActionExecuting(context);
            if(page != null)
            {
                var custom = page.Items;

                var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite && custom.Exists(p.Name));

                foreach (var prop in props)
                {
                    var rawValue = custom.GetValue(prop.Name);
                    try
                    {
                        if (prop.PropertyType != typeof(bool))
                        {
                            object convertedValue = System.Convert.ChangeType(rawValue, prop.PropertyType);
                            prop.SetValue(this, convertedValue);
                        }
                        else
                        {
                            prop.SetValue(this, rawValue.ToBool());
                        }
                    }
                    catch
                    {
                    }
                }
            }
           
        }
    }
}
