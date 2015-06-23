using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString ReplaceSpacesWithNbsp(this HtmlHelper @this, string value)
        {
            return @this.Raw(value.Replace(" ", "&#160;"));
        }

        public static IHtmlString RenderTextNoWrap(this HtmlHelper @this, string value)
        {
            return @this.Raw("<span style=\"white-space: nowrap;\">" + value + "</span>");
        }

        public static IHtmlString RenderEmailLink(this HtmlHelper @this, string email, string text = "e-mail",
            bool showIcon = true, string iconClass = "fa fa-envelope-o")
        {
            var icon = "";
            if (showIcon)
            {
                icon = string.Format("<i class=\"{0}\" style=\"padding-right: 5px;\"></i>",
                    iconClass ?? "fa fa-envelope-o");
            }
            var html = string.Format("<a href=\"mailto:{0}\">{1}{2}</a>", email, icon, text ?? "e-mail");

            return @this.Raw(html);
        }

        public static IHtmlString RenderExternalLink(this HtmlHelper @this, string url, string text,
            bool blankTarget = false, bool showIcon = true, bool iconAfterLink = false,
            string iconClass = "fa fa-external-link")
        {
            var icon = "";
            if (showIcon)
            {
                icon = string.Format("<i class=\"{0}\" style=\"padding-right: 5px;\"></i>",
                    iconClass ?? "fa fa-envelope-o");
            }
            var target = (blankTarget ? "_blank" : null);
            string html;
            if (showIcon && !iconAfterLink)
            {
                html = string.Format("<a href=\"{0}\" target=\"{1}\">{2}&nbsp;{3}</a>", url, target, icon, text);
            }
            else
            {
                html = string.Format("<a href=\"{0}\" target=\"{1}\">{3}&nbsp;{2}</a>", url, target, icon, text);
            }

            return @this.Raw(html);
        }

        public static IHtmlString UlFor(this HtmlHelper @this, IEnumerable<string> input)
        {
            var html = input.Aggregate("<ul class=\"list-unstyled\">",
                (current, item) => current + string.Format("<li>{0}</li>", item), s => s + "</ul>");

            return @this.Raw(html);
        }
    }
}