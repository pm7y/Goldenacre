using System;
using System.Linq;
using System.Reflection;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class AppDomainExtensions
    {
        public static bool IsAssemblyLoaded(this AppDomain @this, string assemblyName)
        {
            var loadedAssemblies = @this.GetAssemblies();

            return loadedAssemblies.Any(a => a.GetName().Name == assemblyName);
        }

        public static Assembly GetLoadedAssembly(this AppDomain @this, string assemblyName)
        {
            var loadedAssemblies = @this.GetAssemblies();

            return loadedAssemblies.FirstOrDefault(a => a.GetName().Name == assemblyName);
        }
    }
}