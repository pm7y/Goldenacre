using System;
using System.Linq;

namespace Goldenacre.Extensions
{
    public static class AppDomainExtensions
    {
        public static bool AssemblyIsLoaded(this AppDomain @this, string assemblyName)
        {
            var loadedAssemblies = @this.GetAssemblies();

            return loadedAssemblies.Any(a => a.GetName().Name == assemblyName);
        }
    }
}