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
    [HtmlTargetElement("vsw-textarea-label", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class TextAreaLabelTagHelper : Microsoft.AspNetCore.Mvc.TagHelpers.TextAreaTagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string RequiredAttributeName = "asp-required";
        private const string DisabledAttributeName = "asp-disabled";
        private const string ErrorMessageAttributeName = "asp-message";
        private const string LabelAttributeName = "asp-label";
        private const string LabelClassAttributeName = "asp-label-class";
        private const string LabelIdAttributeName = "asp-label-id";
        private const string InfoAttributeName = "asp-info";
        private const string IconLeftAttributeName = "asp-icon-left";
        private const string IconRightAttributeName = "asp-icon-right";
        private const string ButtonTextAttributeName = "asp-button-text";
        private const string ButtonIconAttributeName = "asp-button-icon";
        private const string ButtonIconAlignAttributeName = "asp-button-icon-align";
        private const string ButtonActionAttributeName = "asp-button-action";
        private const string ButtonDisabledAttributeName = "asp-button-disabled";
        private const string ButtonIdAttributeName = "asp-button-id";
        private const string GroupTextAttributeName = "asp-group-text";
        private const string PostfixAttributeName = "asp-postfix";

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
        /// Label for a dropdown list
        /// </summary>
        [HtmlAttributeName(LabelAttributeName)]
        public string Label { get; set; }
        /// <summary>
        /// Class for a Label
        /// </summary>
        [HtmlAttributeName(LabelClassAttributeName)]
        public string LabelClass { get; set; }
        /// <summary>
        /// ID for a Label
        /// </summary>
        [HtmlAttributeName(LabelIdAttributeName)]
        public string LabelId { get; set; }
        /// <summary>
        /// Info for a dropdown list
        /// </summary>
        [HtmlAttributeName(InfoAttributeName)]
        public string Info { get; set; }
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
        /// Postfix
        /// </summary>
        [HtmlAttributeName(PostfixAttributeName)]
        public string Postfix { set; get; }
        /// <summary>
        /// HtmlGenerator
        /// </summary>
        protected IHtmlGenerator Generator { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="generator">HTML generator</param>
        public TextAreaLabelTagHelper(IHtmlGenerator generator) : base(generator)
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
                // Đổi lại thành readonly để thay đổi cursor khi hover
                // CORE-453 Gxx - Với tất cả các trường dữ liệu (textbox, text area) chỉ cho view, không cho nhập thì khi hover chuột sẽ cho hiển thị hình cấm
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
            string labelcontent = "";
            bool.TryParse(Label, out bool label);
            if (label)
            {
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
                    if (required && (string.IsNullOrEmpty(Postfix) || Postfix != "*"))
                    {
                        tagBuilder.InnerHtml.AppendHtml("<span class=\"text-danger\">*</span>");
                    }
                    //set info if exists
                    if (Info.IsNotEmpty())
                    {
                        tagBuilder.InnerHtml.AppendHtml(@"<span data-tooltip=""tipsy"" original-title=""" + Info + @""" data-position=""top"" class=""text-primary""><i class=""icon-info22""></i></span>");
                    }
                    if (LabelClass.IsNotEmpty())
                        tagBuilder.AddCssClass(LabelClass);
                    if (LabelId.IsNotEmpty())
                        tagBuilder.MergeAttribute("id", LabelId);
                    labelcontent = tagBuilder.ToHtmlString();
                }
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
            //string html = "<div class=\"form-group\">" + labelcontent + "<div class=\"input-group\">" + iconleftcontent + output.RenderHtmlContent() + labelErrorMessage + iconrightcontent + button + grouptext + "</div></div>";
            //output.Content.SetHtmlContent(html);
            output.PreElement.AppendHtml("<div class=\"form-group\">" + labelcontent + "<div class=\"input-group\">" + iconleftcontent);
            output.PostElement.AppendHtml(labelErrorMessage + iconrightcontent + button + grouptext + "</div></div>");
        }
    }
}
