using System.Security.Principal;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class IdentityExtensions
    {
        public static bool IsAuthenticated(this IPrincipal @this)
        {
            return (@this != null ? @this.Identity : null) != null && @this.Identity.IsAuthenticated;
        }

        public static bool IsAuthenticated(this IIdentity @this)
        {
            return @this != null && @this.IsAuthenticated;
        }

        public static string NameWithoutDomain(this IIdentity @this)
        {
            var name = (string) null;

            if ((@this != null ? @this.Name : null) != null)
            {
                name = @this.Name;

                if (name.Contains(@"\"))
                {
                    name = name.Substring(name.LastIndexOf('\\') + 1);
                }

                name = name.Trim();
            }

            return name;
        }
    }
}