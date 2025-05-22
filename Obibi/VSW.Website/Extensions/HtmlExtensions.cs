using Elastic.Apm.Api;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using VSW.Core;
using VSW.Core.Services;
using VSW.Website.DataBase.Services;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.Extensions
{
    /// <summary>
    /// HTML extensions
    /// </summary>
    public static class HtmlExtensions
    {
        #region Common extensions

        /// <summary>
        /// Convert IHtmlContent to string
        /// </summary>
        /// <param name="htmlContent">HTML content</param>
        /// <returns>Result</returns>
        public static string RenderHtmlContent(this IHtmlContent htmlContent)
        {
            using var writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            var htmlOutput = writer.ToString();
            return htmlOutput;
        }

        /// <summary>
        /// Convert IHtmlContent to string
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <returns>String</returns>
        public static string ToHtmlString(this IHtmlContent tag)
        {
            using var writer = new StringWriter();
            tag.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        public static async Task<IHtmlContent> PartialForAsync<TModel>(this IHtmlHelper<TModel> helper,
                                                                     string propertyPath,
                                                                    string partialName, object model, ViewDataDictionary viewData = null, int? itemIndex = null
                                                                    )
        {
            string name = propertyPath;
            var parentViewData = helper.ViewData;
            var htmlPrefix = parentViewData.TemplateInfo.HtmlFieldPrefix;

            if (viewData == null)
            {
                viewData = new ViewDataDictionary(parentViewData);
            }

            if (name.IsNotEmpty())
                viewData.TemplateInfo.HtmlFieldPrefix += !Equals(htmlPrefix, string.Empty) ? $".{name}" : name;

            if (itemIndex != null)
            {
                viewData.TemplateInfo.HtmlFieldPrefix = HtmlPrefixOfIndex(viewData.TemplateInfo.HtmlFieldPrefix, itemIndex.Value);
            }

            return await helper.PartialAsync(partialName, model, viewData);
        }

        public static async Task<IHtmlContent> PartialForAsync<TModel, TProperty>(this IHtmlHelper<TModel> helper,
                                                                       Expression<Func<TModel, TProperty>> expression,
                                                                      string partialName, ViewDataDictionary viewData = null, int? itemIndex = null
                                                                      )
        {
            var model = expression.Compile()(helper.ViewData.Model);
            string name = "";
            if (expression.Body is MemberExpression)
            {
                name = (expression.Body as MemberExpression).FullName();
            }

            var parentViewData = helper.ViewData;
            var htmlPrefix = parentViewData.TemplateInfo.HtmlFieldPrefix;

            if (viewData == null)
            {
                viewData = new ViewDataDictionary(parentViewData);
            }

            if (name != "")
                viewData.TemplateInfo.HtmlFieldPrefix += !Equals(htmlPrefix, string.Empty) ? $".{name}" : name;
            else if (itemIndex != null)
                viewData.TemplateInfo.HtmlFieldPrefix = HtmlPrefixOfIndex(viewData.TemplateInfo.HtmlFieldPrefix, itemIndex.Value);

            return await helper.PartialAsync(partialName, model, viewData);
        }

        public static string HtmlFieldPrefix<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, int? itemIndex = null)
        {
            string name = "";
            if (expression.Body is MemberExpression)
            {
                name = (expression.Body as MemberExpression).FullName();
            }

            if (itemIndex != null)
                name = HtmlPrefixOfIndex(name, itemIndex.Value);

            return name;
        }

        public static string HtmlFieldPrefix<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression, int? itemIndex = null)
        {
            return HtmlFieldPrefix(expression, itemIndex);
        }

        public static string HtmlPrefixOfIndex(string parentPrefix, int itemIndex)
        {
            parentPrefix += $"[{itemIndex}]";
            return parentPrefix;
        }

        public static async Task<IHtmlContent> PartialCheckViewAsync(this IHtmlHelper helper,
                                                                      string partialName, string partialCommon = "", object model = null, ViewDataDictionary viewData = null)
        {
            var parentViewData = helper.ViewData;
            if (viewData == null)
            {
                viewData = new ViewDataDictionary(parentViewData);
            }
            var services = helper.ViewContext.HttpContext.RequestServices;
            var viewEngine = services.GetRequiredService<ICompositeViewEngine>();
            var result = viewEngine.GetView(null, partialName, true);
            if (!result.Success && partialCommon.IsNotEmpty())
                partialName = partialCommon;

            if (model == null)
            {
                return await helper.PartialAsync(partialName, viewData);
            }
            return await helper.PartialAsync(partialName, model, viewData);
        }
        public static IEnumerable<SelectListItem> GetEnumValueSelectList<TEnum>(this IHtmlHelper htmlHelper) where TEnum : struct
        {
            return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = x.GetDescription(),
                        Value = x.ToString()
                    }), "Value", "Text");
        }

        public static string Url(this IHtmlHelper htmlHelper, string module)
        {
            var appSetting = CoreService.GetConfigWithSection<AppsSetting>();
            var site = htmlHelper.ViewContext.HttpContext.Items["Site"] as ISiteInterface;
            var service = htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IPageServiceInterface>();

            var url = service.GetUrlByModule(module, site.LangID);
            if (appSetting.MultiSite)
            {
                return "/" + site.Code + "/" + (url ?? "");
            }
            return "/" + (url ?? "");
        }

        public static string GetUrl(this IHtmlHelper htmlHelper, string code)
        {
            var appSetting = CoreService.GetConfigWithSection<AppsSetting>();
            if (appSetting.MultiSite && code.IsNotEmpty())
            {
                var site = htmlHelper.ViewContext.HttpContext.Items["Site"] as ISiteInterface;
                return "/" + site.Code + "/" + code;
            }
            return "/" + code;
        }

        public static string GetUrlCurrentPage(this IHtmlHelper htmlHelper)
        {
            var page = htmlHelper.ViewContext.HttpContext.Items["Page"] as IPageInterface;
            return GetUrl(htmlHelper, page.Code);
        }

        public static IPageInterface CurrentPage(this IHtmlHelper htmlHelper)
        {
            var page = htmlHelper.ViewContext.HttpContext.Items["Page"] as IPageInterface;
            return page;
        }

        private static string BuildPageUrl(IHtmlHelper htmlHelper, string paramName, string urlRoot = "")
        {
            var request = htmlHelper.ViewContext.HttpContext.Request;

            var path = urlRoot;
            if (path.IsEmpty()) path = GetUrlCurrentPage(htmlHelper);

            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(request.QueryString.ToString());

            var updatedQuery = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var kv in query)
            {
                if (!string.Equals(kv.Key, paramName, StringComparison.OrdinalIgnoreCase))
                    updatedQuery[kv.Key] = kv.Value;
            }

            var newQueryString = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(path, updatedQuery);

            return newQueryString;
        }

        public static string GetPagination(this IHtmlHelper htmlHelper, int pageIndex, int pageSize, int totalRecord, string urlRoot = "")
        {
            pageIndex = pageIndex - 1;
            if (totalRecord < pageSize) return "";
            int pageMax = 10;
            var minPage = pageIndex / pageMax * pageMax;
            var maxPage = minPage + pageMax;

            var maxPageIndex = totalRecord / ((double)pageSize);

            string paramName = "Page";
            string url = BuildPageUrl(htmlHelper, paramName, urlRoot);
            if (url.EndsWith("/"))
                url = "?" + paramName;
            else if (url.Contains("?"))
                url += "&" + paramName;
            else
                url += "?" + paramName;

            string backText = "<i class=\"fa fa-caret-left\"></i>";
            string nextText = "<i class=\"fa fa-caret-right\"></i>";
            string backEndText = "<i class=\"fa fa-arrow-left\"></i>";
            string nextEndText = "<i class=\"fa fa-arrow-right\"></i>";

            string Html = "";
            if (!(maxPageIndex > 1)) return Html;

            if (maxPage > pageMax)
            {
                Html += @"<a href=""" + url + ("=") + 1 + @""" class=""page-numbers"">" + backEndText + @"</a>";
                Html += @"<a href=""" + url + ("=") + minPage + @""" class=""page-numbers"">" + backText + @"</a>";
            }

            for (var i = minPage; i < maxPage; i++)
            {
                if (i != pageIndex)
                {
                    if (i < maxPageIndex)
                    {
                        Html += @"<a href=""" + url + ("=") + (i + 1) + @""" class=""page-numbers"">" + (i + 1) + @"</a>";
                    }
                }
                else
                {
                    if (i < maxPageIndex)
                        Html += @"<a href=""javascript:void(0)"" class=""page-numbers current"">" + (i + 1) + @"</a>";
                }
            }

            if (maxPage < maxPageIndex)
            {
                Html += @"<a href=""" + url + ("=") + (maxPage + 1) + @""" class=""page-numbers"">" + nextText + @"</a>";
                Html += @"<a href=""" + url + ("=") + (maxPageIndex > (int)maxPageIndex ? (int)maxPageIndex + 1 : maxPageIndex) + @""" class=""page-numbers"">" + nextEndText + @"</a>";
            }

            if (pageIndex > 0)
            {
                Html += @"<link rel=""prev"" href=""" + url + ("=") + (pageIndex) + @""" />";
            }
            if (pageIndex < maxPageIndex)
            {
                Html += @"<link rel=""next"" href=""" + url + ("=") + (pageIndex + 2) + @""" />";
            }
            return Html;
        }
        #endregion
    }
}
