using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class EnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> @this)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in @this)
            {
                c.Add(e);
            }
            return c;
        }

        public static bool IsLastElement<T>(this IEnumerable<T> @this, T element)
            where T : class
        {
            var last = @this.LastOrDefault();

            return last != null && last.Equals(element);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> @this,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return @this.Where(element => seenKeys.Add(keySelector(element)));
        }

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            if (@this == null) throw new ArgumentNullException();
            if (action == null) throw new ArgumentNullException();

            foreach (var element in @this)
            {
                action(element);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<int, T> action)
        {
            if (source == null) throw new ArgumentNullException();
            if (action == null) throw new ArgumentNullException();

            var index = 0;
            foreach (var element in source)
            {
                action(index, element);

                index++;
            }
        }

        public static T[] ForEachAssign<T>(this T[] source, Func<int, T, T> func)
        {
            if (source == null) throw new ArgumentNullException();
            if (func == null) throw new ArgumentNullException();

            for (var i = 0; i < source.Length; i++)
            {
                source[i] = func(i, source[i]);
            }
            return source;
        }

        public static T[] ForEachAssign<T>(this T[] source, Func<T, T> func)
        {
            if (source == null) throw new ArgumentNullException();
            if (func == null) throw new ArgumentNullException();

            for (var i = 0; i < source.Length; i++)
            {
                source[i] = func(source[i]);
            }

            return source;
        }
    }
}