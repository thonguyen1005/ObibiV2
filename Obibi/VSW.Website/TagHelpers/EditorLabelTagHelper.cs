using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using VSW.Core;
using VSW.Website.Extensions;

namespace VSW.Website.TagHelpers
{
    /// <summary>
    /// bic-editor tag helper
    /// </summary>
    [HtmlTargetElement("vsw-editor-label", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class EditorLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string PasswordAttributeName = "asp-password";
        private const string DecimalDigitAttributeName = "asp-decimal-digit";
        private const string DisabledAttributeName = "asp-disabled";
        private const string RequiredAttributeName = "asp-required";
        private const string RenderFormControlClassAttributeName = "asp-render-form-control-class";
        private const string TemplateAttributeName = "asp-template";
        private const string PostfixAttributeName = "asp-postfix";
        private const string ValueAttributeName = "asp-value";
        private const string PlaceholderAttributeName = "placeholder";
        private const string ErrorMessageAttributeName = "asp-message";
        private const string LabelAttributeName = "asp-label";
        private const string LabelClassAttributeName = "asp-label-class";
        private const string LabelIdAttributeName = "asp-label-id";
        private const string InfoAttributeName = "asp-info";
        private const string IconLeftAttributeName = "asp-icon-left";
        private const string IconRightAttributeName = "asp-icon-right";
        private const string ButtonTextAttributeName = "asp-button-text";
        private const string ButtonIconAttributeName = "asp-button-icon";
        private const string ButtonActionAttributeName = "asp-button-action";
        private const string ButtonDisabledAttributeName = "asp-button-disabled";
        private const string ButtonIconAlignAttributeName = "asp-button-icon-align";
        private const string ButtonIdAttributeName = "asp-button-id";
        private const string GroupTextAttributeName = "asp-group-text";

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Indicates whether the field is disabled
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
        /// Placeholder for the field
        /// </summary>
        [HtmlAttributeName(PlaceholderAttributeName)]
        public string Placeholder { set; get; }

        /// <summary>
        /// Indicates whether the "form-control" class shold be added to the input
        /// </summary>
        [HtmlAttributeName(RenderFormControlClassAttributeName)]
        public string RenderFormControlClass { set; get; }

        /// <summary>
        /// Editor template for the field
        /// </summary>
        [HtmlAttributeName(TemplateAttributeName)]
        public string Template { set; get; }

        /// <summary>
        /// Postfix
        /// </summary>
        [HtmlAttributeName(PostfixAttributeName)]
        public string Postfix { set; get; }

        [HtmlAttributeName(DecimalDigitAttributeName)]
        public int? DecimalDigit { get; set; }

        /// <summary>
        /// The value of the element
        /// </summary>
        [HtmlAttributeName(ValueAttributeName)]
        public string Value { set; get; }
        /// <summary>
        /// Type password
        /// </summary>
        [HtmlAttributeName(PasswordAttributeName)]
        public string Password { set; get; }
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
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        /// <summary>
        /// HtmlGenerator
        /// </summary>
        protected IHtmlGenerator Generator { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        public EditorLabelTagHelper(IHtmlHelper htmlHelper, IHtmlGenerator generator)
        {
            _htmlHelper = htmlHelper;
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

            //clear the output
            output.SuppressOutput();

            //container for additional attributes
            var htmlAttributes = new Dictionary<string, object>();

            //set placeholder if exists
            if (!string.IsNullOrEmpty(Placeholder))
                htmlAttributes.Add("placeholder", Placeholder);



            //set value if exists
            if (!string.IsNullOrEmpty(Value))
                htmlAttributes.Add("value", Value);

            //disabled attribute
            bool.TryParse(IsDisabled, out var disabled);
            if (disabled)
            {
                //htmlAttributes.Add("disabled", "true");
                htmlAttributes.Add("readonly", "readonly");
            }

            //required asterisk
            var validatorMetadata = new List<RequiredAttribute>();
            if (For.Metadata.ValidatorMetadata.Count > 0)
            {
                validatorMetadata = For.Metadata.ValidatorMetadata.Where(o => o.GetType() == typeof(RequiredAttribute)).Cast<RequiredAttribute>().ToList();
            }

            bool.TryParse(IsRequired, out var required);
            if (!required && For.Metadata.ValidatorMetadata.Count > 0)
            {
                required = !validatorMetadata.FirstOrDefault()?.AllowEmptyStrings ?? false;
            }
            if (required)
            {
                htmlAttributes.Add("required", "true");
                htmlAttributes.Add("data-rule-required", "true");


                // bổ sung validate > 0 đổi với các editor dạng số
                if (Template == "Decimal" || Template == "Int32" || Template == "Double")
                {
                    htmlAttributes.Add("data-rule-greaterthanzero", "true");
                    htmlAttributes.Add("data-msg-greaterthanzero", "Giá trị phải lớn hơn 0");
                }

                // bổ sung validate format đối với các editor dạng Ngày tháng
                if (Template == "Date")
                {
                    htmlAttributes.Add("data-rule-dateformat", "true");
                    htmlAttributes.Add("data-msg-dateformat", "Định dạng ngày không hợp lệ");
                }


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
                htmlAttributes.Add("data-msg-required", ErrorMessage);
                labelErrorMessage = @"<div class=""invalid-feedback"">" + ErrorMessage + "</div>";
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
            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //generate editor
            if (!_htmlHelper.ViewData.ContainsKey(For.Name) && For.Model != null && string.IsNullOrEmpty(For.Metadata.TemplateHint))
                _htmlHelper.ViewData.Add(For.Name, For.Model);

            var attributes = context.AllAttributes;
            foreach (var attribute in attributes)
            {
                if (!attribute.Name.Equals(ForAttributeName) &&
                    !attribute.Name.Equals(PasswordAttributeName) &&
                    !attribute.Name.Equals(DecimalDigitAttributeName) &&
                    !attribute.Name.Equals(DisabledAttributeName) &&
                    !attribute.Name.Equals(RequiredAttributeName) &&
                    !attribute.Name.Equals(RenderFormControlClassAttributeName) &&
                    !attribute.Name.Equals(TemplateAttributeName) &&
                    !attribute.Name.Equals(PostfixAttributeName) &&
                    !attribute.Name.Equals(ValueAttributeName) &&
                    !attribute.Name.Equals(PlaceholderAttributeName) &&
                    !attribute.Name.Equals(ErrorMessageAttributeName) &&
                    !attribute.Name.Equals(LabelAttributeName) &&
                    !attribute.Name.Equals(LabelClassAttributeName) &&
                    !attribute.Name.Equals(LabelIdAttributeName) &&
                    !attribute.Name.Equals(InfoAttributeName) &&
                    !attribute.Name.Equals(IconLeftAttributeName) &&
                    !attribute.Name.Equals(IconRightAttributeName) &&
                    !attribute.Name.Equals(ButtonTextAttributeName) &&
                    !attribute.Name.Equals(ButtonActionAttributeName) &&
                    !attribute.Name.Equals(ButtonIconAlignAttributeName) &&
                    !attribute.Name.Equals(ButtonIconAttributeName) &&
                    !attribute.Name.Equals(ButtonIdAttributeName) &&
                    !attribute.Name.Equals(GroupTextAttributeName)
                    )
                {
                    htmlAttributes.Add(attribute.Name, attribute.Value);
                }
            }
            //thêm tag datalabel
            if (For.Metadata.DisplayName.IsNotEmpty() && !htmlAttributes.ContainsKey("data-label"))
            {
                htmlAttributes.Add("data-label", For.Metadata.DisplayName);
            }
            //add form-control class
            bool.TryParse(RenderFormControlClass, out var renderFormControlClass);
            if (string.IsNullOrEmpty(RenderFormControlClass) && For.Metadata.ModelType.Name.Equals("String") || renderFormControlClass)
            {
                if (htmlAttributes.ContainsKey("class"))
                    htmlAttributes["class"] += " form-control";
                else
                    htmlAttributes.Add("class", "form-control");
            }

            var decimalDigit = 0;

            if (For.Metadata.ModelType == typeof(decimal) || For.Metadata.ModelType == typeof(decimal?))
            {
                decimalDigit = !DecimalDigit.HasValue ? 2 : DecimalDigit.Value;
            }

            IHtmlContent htmlOutput;
            bool.TryParse(Password, out var password);
            if (password)
            {
                htmlOutput = _htmlHelper.Password(For.Name, Value, htmlAttributes);
            }
            else
            {
                htmlOutput = _htmlHelper.Editor(For.Name, Template, new { htmlAttributes = htmlAttributes, postfix = Postfix, decimalDigit = decimalDigit });
            }
            string html = "<div class=\"form-group\">" + labelcontent + "<div class=\"input-group\">" + iconleftcontent + htmlOutput.RenderHtmlContent() + iconrightcontent + button + grouptext + labelErrorMessage + "</div></div>";
            output.Content.SetHtmlContent(html);
        }
    }
}