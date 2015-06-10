using System.Security.Principal;

namespace mcilreavy.library.extensions
{
    public static class IdentityExtensions
    {
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