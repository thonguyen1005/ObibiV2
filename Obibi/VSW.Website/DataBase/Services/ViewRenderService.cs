using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using VSW.Website.Interface;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace VSW.Website.DataBase.Services
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ViewRenderService(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> PartialAsync(string viewPath, object model, ViewDataDictionary viewData = null)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var actionContext = new ActionContext(
                httpContext,
                httpContext.GetRouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            using var sw = new StringWriter();
            ViewEngineResult viewResult;
            if (viewPath.StartsWith("/"))
            {
                viewResult = _viewEngine.GetView(null, viewPath, isMainPage: false);
            }
            else
            {
                viewResult = _viewEngine.FindView(actionContext, viewPath, isMainPage: false);
            }
            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"{viewPath} không tìm thấy.");
            }

            var viewDictionary = viewData ?? new ViewDataDictionary(
                                new EmptyModelMetadataProvider(),
                                new ModelStateDictionary());

            viewDictionary.Model = model;

            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                viewDictionary,
                new TempDataDictionary(httpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
