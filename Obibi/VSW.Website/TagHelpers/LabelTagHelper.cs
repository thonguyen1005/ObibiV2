using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using VSW.Core;

namespace VSW.Website.TagHelpers
{
    /// <summary>
    /// bic-label tag helper
    /// </summary>
    [HtmlTargetElement("vsw-label", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class LabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string InForAttributeName = "asp-infor";
        private const string DisplayHintAttributeName = "asp-display-hint";
        private const string PostfixAttributeName = "asp-postfix";


        /// <summary>
        /// HtmlGenerator
        /// </summary>
        protected IHtmlGenerator Generator { get; set; }

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Indicates whether the hint should be displayed
        /// </summary>
        [HtmlAttributeName(DisplayHintAttributeName)]
        public bool DisplayHint { get; set; } = true;

        /// <summary>
        /// Postfix
        /// </summary>
        [HtmlAttributeName(PostfixAttributeName)]
        public string Postfix { set; get; }
        /// <summary>
        /// Postfix
        /// </summary>
        [HtmlAttributeName(InForAttributeName)]
        public string Info { set; get; }
        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="generator">HTML generator</param>
        public LabelTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            //var requiredText = "";
            ////required asterisk
            //bool.TryParse(IsRequired, out var required);
            //if (required)
            //{
            //    requiredText = System.Net.WebUtility.HtmlEncode(": <span class='text-danger'>*</span>");
            //    //output.Content.AppendHtml(": <span class='text-danger'>*</span>");
            //}

            //generate label
            var tagBuilder = Generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, null, new { @class = "d-block" });
            if (tagBuilder != null)
            {
                //set postfix if exists
                if (!string.IsNullOrEmpty(Postfix))
                {
                    if (Postfix == "*")
                        tagBuilder.InnerHtml.AppendHtml("<span class=\"text-danger\">*</span>");
                    else
                        tagBuilder.InnerHtml.AppendHtml("<i class=\"text-danger float-right font9\">" + Postfix + "</i>");
                }
                //set info if exists
                if (Info.IsNotEmpty())
                {
                    tagBuilder.InnerHtml.AppendHtml(@"<span data-tooltip=""tipsy"" original-title=""" + Info + @""" data-position=""top"" class=""text-primary""><i class=""icon-info22""></i></span>");
                }

                //output.PostContent.AppendHtml(WebUtility.HtmlEncode(": <span class='text-danger'>*</span>"));
                //merge classes
                //var classValue = output.Attributes.ContainsName("class")
                //                    ? $"{output.Attributes["class"].Value} d-block"
                //                    : "d-block";
                //output.Attributes.SetAttribute("class", classValue);

                //add label
                output.Content.SetHtmlContent(tagBuilder);
                //create a label wrapper
                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;
                //merge classes
                var classValue = output.Attributes.ContainsName("class")
                                    ? $"{output.Attributes["class"].Value} form-label"
                                    : "form-label";
                output.Attributes.SetAttribute("class", classValue);

                ////required asterisk
                //bool.TryParse(IsRequired, out var required);
                //if (required)
                //{
                //    output.Content.AppendHtml(": <span class='text-danger'>*</span>");
                //}

            }
        }
    }
}
