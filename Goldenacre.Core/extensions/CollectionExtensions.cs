using System.Collections.Generic;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> AddElement<T>(this ICollection<T> list, T item)
        {
            list.Add(item);
            return list;
        }

        public static ICollection<T> AddElementIf<T>(this ICollection<T> list, bool condition, T item)
        {
            if (condition)
            {
                list.Add(item);
            }

            return list;
        }
    }
}