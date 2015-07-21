using System.Globalization;
using System.Linq;
using System.Web;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class RequestExtensions
    {
        /// <summary>
        ///     Get the users preferred culture from the request.
        ///     If not available then it returns the CurrentUICulture.
        /// </summary>
        public static CultureInfo PreferredCulture(this HttpRequest @this)
        {
            var languages = @this.UserLanguages;
            string culture = null;

            if (languages != null && languages.Length > 0)
            {
                culture = languages.FirstOrDefault(l => l.Contains("-"));

                if (culture == null)
                {
                    culture = languages.FirstOrDefault(l => !l.Contains("-"));
                }
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return string.IsNullOrWhiteSpace(culture)
                ? CultureInfo.CurrentUICulture
                : new CultureInfo(culture.Trim().TrimEnd(';'));
        }
    }
}