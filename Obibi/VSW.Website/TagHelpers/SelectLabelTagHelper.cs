using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using VSW.Website.Extensions;
using VSW.Core;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace VSW.Website.TagHelpers
{
    /// <summary>
    /// bic-select tag helper
    /// </summary>
    [HtmlTargetElement("vsw-select-label", TagStructure = TagStructure.WithoutEndTag)]
    public class SelectLabelTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string NameAttributeName = "asp-for-name";
        private const string ItemsAttributeName = "asp-items";
        private const string DisabledAttributeName = "asp-disabled";
        private const string MultipleAttributeName = "asp-multiple";
        private const string RequiredAttributeName = "asp-required";
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

        private readonly IHtmlHelper _htmlHelper;

        /// <summary>
        /// An expression to be evaluated against the current model
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Name for a dropdown list
        /// </summary>
        [HtmlAttributeName(NameAttributeName)]
        public string Name { get; set; }

        /// <summary>
        /// Items for a dropdown list
        /// </summary>
        [HtmlAttributeName(ItemsAttributeName)]
        public IEnumerable<SelectListItem> Items { set; get; } = new List<SelectListItem>();

        /// <summary>
        /// Indicates whether the field is required
        /// </summary>
        [HtmlAttributeName(RequiredAttributeName)]
        public string IsRequired { set; get; }
        /// <summary>
        /// Indicates whether the field is disabled
        /// </summary>
        [HtmlAttributeName(DisabledAttributeName)]
        public string IsDisabled { set; get; }
        /// <summary>
        /// Error message display
        /// </summary>
        [HtmlAttributeName(ErrorMessageAttributeName)]
        public string ErrorMessage { set; get; }
        /// <summary>
        /// Indicates whether the input is multiple
        /// </summary>
        [HtmlAttributeName(MultipleAttributeName)]
        public string IsMultiple { set; get; }
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
        public SelectLabelTagHelper(IHtmlHelper htmlHelper, IHtmlGenerator generator)
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

            //get htmlAttributes object
            var htmlAttributes = new Dictionary<string, object>();

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
            //disabled asterisk
            bool.TryParse(IsDisabled, out bool disabled);
            if (disabled)
            {
                htmlAttributes.Add("disabled", "disabled");
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

            var attributes = context.AllAttributes;
            foreach (var attribute in attributes)
            {
                if (!attribute.Name.Equals(ForAttributeName) &&
                    !attribute.Name.Equals(NameAttributeName) &&
                    !attribute.Name.Equals(ItemsAttributeName) &&
                    !attribute.Name.Equals(DisabledAttributeName) &&
                    !attribute.Name.Equals(MultipleAttributeName) &&
                    !attribute.Name.Equals(RequiredAttributeName) &&
                    !attribute.Name.Equals(ErrorMessageAttributeName) &&
                    !attribute.Name.Equals(LabelAttributeName) &&
                    !attribute.Name.Equals(LabelClassAttributeName) &&
                    !attribute.Name.Equals(LabelIdAttributeName) &&
                    !attribute.Name.Equals(InfoAttributeName) &&
                    !attribute.Name.Equals(IconLeftAttributeName) &&
                    !attribute.Name.Equals(IconRightAttributeName) &&
                    !attribute.Name.Equals(ButtonTextAttributeName) &&
                    !attribute.Name.Equals(ButtonActionAttributeName) &&
                    !attribute.Name.Equals(ButtonIconAttributeName) &&
                    !attribute.Name.Equals(ButtonIconAlignAttributeName) &&
                    !attribute.Name.Equals(ButtonIdAttributeName) &&
                    !attribute.Name.Equals(GroupTextAttributeName) &&
                    !attribute.Name.Equals(PostfixAttributeName)
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
            //generate editor
            var tagName = For != null ? For.Name : Name;
            bool.TryParse(IsMultiple, out bool multiple);
            if (!string.IsNullOrEmpty(tagName))
            {
                IHtmlContent selectList;
                if (multiple)
                {
                    selectList = _htmlHelper.Editor(tagName, "MultiSelect", new { htmlAttributes, SelectList = Items });
                }
                else
                {
                    if (htmlAttributes.ContainsKey("class"))
                        htmlAttributes["class"] += " form-control";
                    else
                        htmlAttributes.Add("class", "form-control");

                    if (!htmlAttributes.ContainsKey("class2"))
                        htmlAttributes.Add("select2", null);

                    selectList = _htmlHelper.DropDownList(tagName, Items, htmlAttributes);
                    //selectList = _htmlHelper.Editor(tagName, "SingleSelect", new { htmlAttributes, SelectList = Items });
                }

                string html = "<div class=\"form-group\">" + labelcontent + "<div class=\"input-group\">" + iconleftcontent + selectList.RenderHtmlContent() + iconrightcontent + button + grouptext + labelErrorMessage + "</div></div>";
                output.Content.SetHtmlContent(html);
                
            }
        }
    }
}