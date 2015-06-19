using System.Collections.Generic;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> AddElement<T>(this ICollection<T> @this, T item)
        {
            @this.Add(item);
            return @this;
        }

        public static ICollection<T> AddElementIf<T>(this ICollection<T> @this, bool condition, T item)
        {
            if (condition)
            {
                @this.Add(item);
            }

            return @this;
        }
    }
}