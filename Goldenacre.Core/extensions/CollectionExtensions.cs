using System.Collections.Generic;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> AddIfNotContains<T>(this ICollection<T> @this, T item)
        {
            if (!@this.Contains(item))
            {
                @this.Add(item);
            }
            return @this;
        }

        public static ICollection<T> AddIf<T>(this ICollection<T> @this, bool condition, T item)
        {
            if (condition)
            {
                @this.Add(item);
            }

            return @this;
        }
    }
}