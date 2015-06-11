using System.Globalization;
using System.Linq;
using System.Web;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class RequestExtensions
    {

        /// <summary>
        /// Get the user preferred culture from the request.
        /// If not available then it returns the CurrentUICulture.
        /// </summary>
        public static CultureInfo PreferredCulture(this HttpRequest request)
        {
            var languages = request.UserLanguages;
            string culture = null;

            if (languages != null && languages.Length > 0)
            {
                culture = languages.FirstOrDefault(l => l.Contains("-"));

                culture = culture == null ? languages.FirstOrDefault(l => !l.Contains("-")) : culture.SubstringToIndexOf(";").Trim();
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return culture.IsNullOrWhiteSpace() ? CultureInfo.CurrentUICulture : new CultureInfo(culture);
        }
    }
}