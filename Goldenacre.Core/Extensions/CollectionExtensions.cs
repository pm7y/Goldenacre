namespace Goldenacre.Extensions
{
    using System;
    using System.Collections.Generic;

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

        public static ICollection<T> AddIf<T>(this ICollection<T> @this, Func<bool> condition, T item)
        {
            return AddIf(@this, condition(), item);
        }

        public static ICollection<T> AddIf<T>(this ICollection<T> @this, Predicate<T> condition, T item)
        {
            return AddIf(@this, condition(item), item);
        }
    }
}