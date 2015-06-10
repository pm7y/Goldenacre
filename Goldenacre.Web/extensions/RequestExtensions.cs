using System.Linq;
using System.Web;

namespace Goldenacre.Web.Extensions
{
    public static class RequestExtensions
    {
        public static string DetermineLanguage(this HttpRequest request)
        {

            var languages = request.UserLanguages;
            string culture = null;

            if (languages != null && languages.Length > 0)
            {
                culture = languages.FirstOrDefault(l => l.Trim().Length >= 5);

                if (culture != null)
                {
                    if (culture.Contains(";"))
                    {
                        culture = culture.Substring(0, culture.IndexOf(';'));
                    }
                    culture = culture.Trim();
                }
            }
            return culture;
        }
    }
}