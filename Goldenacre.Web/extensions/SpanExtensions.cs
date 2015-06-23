using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class SpanExtensions
    {
        public static MvcHtmlString SpanFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression)
        {
            return SpanFor(html, expression, null, null, null);
        }

        public static MvcHtmlString SpanFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return SpanFor(html, expression, null, htmlAttributes, null);
        }

        internal static MvcHtmlString SpanFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string spanText, object htmlAttributes,
            ModelMetadataProvider metadataProvider)
        {
            return SpanFor(html,
                expression,
                spanText,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                metadataProvider);
        }

        internal static MvcHtmlString SpanFor<TModel, TValue>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression, string spanText, IDictionary<string, object> htmlAttributes,
            ModelMetadataProvider metadataProvider)
        {
            return SpanHelper(html,
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                ExpressionHelper.GetExpressionText(expression),
                spanText,
                htmlAttributes);
        }

        internal static MvcHtmlString SpanHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName,
            string spanText = null, IDictionary<string, object> htmlAttributes = null)
        {
            var resolvedSpanText = spanText ?? metadata.SimpleDisplayText;
            if (string.IsNullOrEmpty(resolvedSpanText))
            {
                return MvcHtmlString.Empty;
            }

            var tag = new TagBuilder("span");
            //tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tag.SetInnerText(resolvedSpanText);
            tag.MergeAttributes(htmlAttributes, true);

            return new MvcHtmlString(tag.ToString(TagRenderMode.Normal));
        }
    }
}