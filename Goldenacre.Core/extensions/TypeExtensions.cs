using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<string> GetPublicPropertyNames(this IReflect @this)
        {
            var pi = @this.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            return pi.Select(p => p.Name);
        }

        public static object[] GetPublicModelValues<TModel>(this TModel @this)
        {
            var t = typeof (TModel);
            var pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            return pi.Select(p => p.GetValue(@this, null)).ToArray();
        }

        public static Dictionary<string, object> GetPublicPropertyValues<TModel>(this TModel @this)
        {
            var d = new Dictionary<string, object>();

            var n = @this.GetType().GetPublicPropertyNames().ToArray();
            var v = @this.GetPublicModelValues();

            for (var i = 0; i < n.Length; i++)
            {
                d.Add(n[i], v[i]);
            }

            return d;
        }
    }
}