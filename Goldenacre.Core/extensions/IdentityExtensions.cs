using System.Security.Principal;

namespace Goldenacre.Core.Extensions
{
    public static class IdentityExtensions
    {
        public static bool IsAuthenticated(this IPrincipal p)
        {
            return (p != null && p.Identity != null && p.Identity.IsAuthenticated);
        }

        public static bool IsAuthenticated(this  IIdentity ii)
        {
            return (ii != null && ii.IsAuthenticated);
        }

        public static string NameWithoutDomain(this IIdentity ii)
        {
            var name = string.Empty;

            if (ii != null)
            {
                name = ii.Name;

                if (name.Contains(@"\"))
                {
                    name = name.Substring(name.LastIndexOf('\\') + 1);
                }
            }

            return name.Trim();
        }
    }
}