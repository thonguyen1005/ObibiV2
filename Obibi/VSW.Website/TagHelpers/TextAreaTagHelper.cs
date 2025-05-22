using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VSW.Core;
using VSW.Website.Extensions;

namespace VSW.Website.TagHelpers
{
    /// <summary>
    /// bic-textarea tag helper
    /// </summary>
    [HtmlTargetElement("vsw-textarea", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class TextAreaTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.TextAreaTagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string RequiredAttributeName = "asp-required";
        private const string DisabledAttributeName = "asp-disabled";
        private const string ErrorMessageAttributeName = "asp-message";
        private const string IconLeftAttributeName = "asp-icon-left";
        private const string IconRightAttributeName = "asp-icon-right";
        private const string ButtonTextAttributeName = "asp-button-text";
        private const string ButtonIconAttributeName = "asp-button-icon";
        private const string ButtonIconAlignAttributeName = "asp-button-icon-align";
        private const string ButtonActionAttributeName = "asp-button-action";
        private const string ButtonDisabledAttributeName = "asp-button-disabled";
        private const string ButtonIdAttributeName = "asp-button-id";
        private const string GroupTextAttributeName = "asp-group-text";

        /// <summary>
        /// Indicates whether the input is disabled
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public string IsDisabled { set; get; }

        /// <summary>
        /// Indicates whether the field is required
        /// </summary>
        [HtmlAttributeName(RequiredAttributeName)]
        public string IsRequired { set; get; }
        /// <summary>
        /// Error message display
        /// </summary>
        [HtmlAttributeName(ErrorMessageAttributeName)]
        public string ErrorMessage { set; get; }
        /// <summary>
        /// Icon
        /// </summary>
        [HtmlAttributeName(IconLeftAttributeName)]
        public string IconLeft { get; set; }
        /// <summary>
        /// Icon
        /// </summary>
        [HtmlAttributeName(IconRightAttributeName)]
        public string IconRight { get; set; }
        /// <summary>
        /// ButtonText
        /// </summary>
        [HtmlAttributeName(ButtonTextAttributeName)]
        public string ButtonText { get; set; }
        /// <summary>
        /// ButtonIcon
        /// </summary>
        [HtmlAttributeName(ButtonIconAttributeName)]
        public string ButtonIcon { get; set; }
        /// <summary>
        /// ButtonIconAlign
        /// </summary>
        [HtmlAttributeName(ButtonIconAlignAttributeName)]
        public string ButtonIconAlign { get; set; }
        /// <summary>
        /// ButtonAction
        /// </summary>
        [HtmlAttributeName(ButtonActionAttributeName)]
        public string ButtonAction { get; set; }

        /// <summary>
        /// ButtonId
        /// </summary>
        [HtmlAttributeName(ButtonIdAttributeName)]
        public string ButtonId { get; set; }
        /// <summary>
        /// (ButtonDisabled
        /// </summary>
        [HtmlAttributeName(ButtonDisabledAttributeName)]
        public string ButtonDisabled { get; set; }

        /// <summary>
        /// GroupText
        /// </summary>
        [HtmlAttributeName(GroupTextAttributeName)]
        public string GroupText { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="generator">HTML generator</param>
        public TextAreaTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        /// <summary>
        /// Process
        /// </summary>
        /// <param name="context">Context</param>
        /// <param name="output">Output</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            //tag details
            output.TagName = "textarea";
            output.TagMode = TagMode.StartTagAndEndTag;

            //merge classes
            var classValue = output.Attributes.ContainsName("class")
                ? $"{output.Attributes["class"].Value} form-control"
                : "form-control";
            output.Attributes.SetAttribute("class", classValue);

            //add disabled attribute
            bool.TryParse(IsDisabled, out bool disabled);
            if (disabled)
            {
                output.Attributes.Add(new TagHelperAttribute("disabled", "disabled"));
                output.Attributes.Add(new TagHelperAttribute("readonly", "readonly"));
            }

            //additional parameters
            //var rowsNumber = output.Attributes.ContainsName("rows") ? output.Attributes["rows"].Value : 4;
            //output.Attributes.SetAttribute("rows", rowsNumber);
            //var colsNumber = output.Attributes.ContainsName("cols") ? output.Attributes["cols"].Value : 20;
            //output.Attributes.SetAttribute("cols", colsNumber);

            //required asterisk
            var validatorMetadata = new List<RequiredAttribute>();
            if (For.Metadata.ValidatorMetadata.Count > 0)
            {
                validatorMetadata = For.Metadata.ValidatorMetadata.Where(o => o.GetType() == typeof(RequiredAttribute)).Cast<RequiredAttribute>().ToList();
            }
            bool.TryParse(IsRequired, out bool required);
            if (!required && For.Metadata.ValidatorMetadata.Count > 0)
            {
                required = !validatorMetadata.FirstOrDefault()?.AllowEmptyStrings ?? false;
            }
            if (required)
            {
                output.Attributes.Add(new TagHelperAttribute("required", "true"));
                output.Attributes.Add("data-rule-required", "true");
                //output.PreElement.SetHtmlContent("<div class='input-group input-group-required'>");
                //output.PostElement.SetHtmlContent("<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>");
            }
            string labelErrorMessage = "";
            if (ErrorMessage.IsEmpty() && For.Metadata.ValidatorMetadata.Count > 0)
            {
                ErrorMessage = (validatorMetadata.IsNotEmpty() ? validatorMetadata.Select(o => o.ErrorMessage).ToArray().Join(",") : "");
            }
            if (ErrorMessage.IsNotEmpty())
            {
                output.Attributes.Add("data-msg-required", ErrorMessage);
                labelErrorMessage = @"<div class=""invalid-feedback"">" + ErrorMessage + "</div>";
            }
            //thêm tag datalabel
            if (For.Metadata.DisplayName.IsNotEmpty() && !output.Attributes.ContainsName("data-label"))
            {
                output.Attributes.Add("data-label", For.Metadata.DisplayName);
            }
            string iconleftcontent = "";
            if (IconLeft.IsNotEmpty())
            {
                iconleftcontent += @"<span class=""input-group-prepend""><span class=""input-group-text""><i class=""" + IconLeft + @"""></i></span></span>";
            }
            string iconrightcontent = "";
            if (IconRight.IsNotEmpty())
            {
                iconrightcontent += @"<span class=""input-group-append""><span class=""input-group-text""><i class=""" + IconRight + @"""></i></span></span>";
            }
            string button = "";
            if (ButtonText.IsNotEmpty())
            {
                string button_icon = "";
                if (ButtonIcon.IsNotEmpty())
                {
                    button_icon = @"<i class=""" + ButtonIcon + @"""></i>";
                }
                bool.TryParse(ButtonDisabled, out var button_disabled);
                button += @"<span class=""input-group-append"">
                            <button type=""button"" " + (button_disabled ? "disabled" : "") + @" class=""btn bg-primary"" " + (ButtonId.IsNotEmpty() ? "id=\"" + ButtonId + "\" " : "") + (ButtonAction.IsNotEmpty() ? "onclick=\"" + ButtonAction + "\"" : "") + @">" + (ButtonIconAlign.IsNotEmpty() && ButtonIconAlign.ToLower() == "left" ? button_icon : "") + ButtonText + ((ButtonIconAlign.IsNotEmpty() && ButtonIconAlign.ToLower() == "right") || ButtonIconAlign.IsEmpty() ? button_icon : "") + "</button></span>";
            }
            string grouptext = "";
            if (GroupText.IsNotEmpty())
            {
                grouptext += @"<span class=""input-group-append"">
                            <span class=""input-group-text"">" + GroupText + "</span></span>";
            }

            //string html = iconleftcontent + output.Content.RenderHtmlContent() + labelErrorMessage + iconrightcontent + button + grouptext;
            //output.PostElement.SetHtmlContent(html);
            output.PreElement.AppendHtml(iconleftcontent);
            output.PostElement.AppendHtml(iconrightcontent + button + grouptext + labelErrorMessage);
            //output.PostElement.AppendHtml(labelErrorMessage + iconrightcontent + button + grouptext);
            //base.Process(context, output);
        }
    }
}
