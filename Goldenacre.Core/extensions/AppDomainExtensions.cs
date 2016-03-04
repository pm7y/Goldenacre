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
            var loadedAssemblies = @this.GetAssemblies()
                .Any(a => a.GetName().Name.EqualsCI(assemblyName));

            return loadedAssemblies;
        }

        public static Assembly GetLoadedAssembly(this AppDomain @this, string assemblyName)
        {
            var loadedAssembly = @this.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.EqualsCI(assemblyName));

            return loadedAssembly;
        }
    }

}