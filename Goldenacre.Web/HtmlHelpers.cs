﻿using System.Web.Mvc;

namespace Goldenacre.Web
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString ReplaceSpacesWithNbsp(this HtmlHelper html, string value)
        {
            return MvcHtmlString.Create(value.Replace(" ", "&#160;"));
        }
    }
}