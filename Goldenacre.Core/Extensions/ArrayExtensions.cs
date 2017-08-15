namespace Goldenacre.Extensions
{
    using System;
    using System.IO;

    public static class ArrayExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] @this)
        {
            return new MemoryStream(@this) { Position = 0 };
        }

        public static T[] Map<T>(this T[] @this, Func<int, T, T> func)
        {
            for (var i = 0; i < @this.Length; i++)
            {
                @this[i] = func(i, @this[i]);
            }

            return @this;
        }

        public static T[] Map<T>(this T[] @this, Func<T, T> func)
        {
            for (var i = 0; i < @this.Length; i++)
            {
                @this[i] = func(@this[i]);
            }

            return @this;
        }
    }
}