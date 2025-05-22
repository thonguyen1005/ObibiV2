using Microsoft.AspNetCore.Razor.TagHelpers;
using VSW.Website.Interface;

namespace VSW.Website.TagHelpers
{
    [HtmlTargetElement("vsw-rs")]
    public class ResourceTagHelper : TagHelper
    {
        [HtmlAttributeName("key")]
        public string Key { get; set; }

        private readonly IResourceServiceInterface _parser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResourceTagHelper(IResourceServiceInterface parser, IHttpContextAccessor httpContextAccessor)
        {
            _parser = parser;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            // Nếu bạn có hàm async
            string value = await _parser.ParseAsync(Key, httpContext);

            output.TagName = null; // loại bỏ thẻ <rs>
            output.Content.SetHtmlContent(value ?? "");
        }
    }
}
