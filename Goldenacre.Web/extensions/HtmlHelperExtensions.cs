using System.Web;
using System.Web.Mvc;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString ReplaceSpacesWithNbsp(this HtmlHelper @this, string value)
        {
            return @this.Raw(value.Replace(" ", "&nbsp;"));
        }

        public static IHtmlString RenderTextNoWrap(this HtmlHelper @this, string value)
        {
            return @this.Raw("<span style=\"white-space: nowrap;\">" + value + "</span>");
        }
    }
}