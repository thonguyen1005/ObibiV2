using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Reflection;
using VSW.Website.Interface;
using VSW.Core.Utils;
using VSW.Core;

namespace VSW.Website.TagHelpers
{
    public abstract class BaseTagHelper : TagHelper
    {
        protected IHttpContextAccessor _httpContextAccessor;
        [HtmlAttributeName("id")]
        public virtual string Id { get; set; }
        [HtmlAttributeName("layout")]
        public virtual string ViewLayout { get; set; }
        [HtmlAttributeName("default-layout")]
        public string DefaultLayout { get; set; }

        protected readonly IViewRenderService _viewRenderService;
        public BaseTagHelper(IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _viewRenderService = viewRenderService;
        }
        protected ISiteInterface CurrentSite;
        protected ITemplateInterface CurrentTemplate;
        protected IPageInterface CurrentPage;
        public override void Init(TagHelperContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            CurrentSite = httpContext.Items["Site"] as ISiteInterface;
            CurrentTemplate = httpContext.Items["Template"] as ITemplateInterface;
            CurrentPage = httpContext.Items["Page"] as IPageInterface;

            var custom = CurrentTemplate.Items.GetCustom(Id);

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
