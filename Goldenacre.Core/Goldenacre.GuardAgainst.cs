// ReSharper disable UseStringInterpolation

namespace Goldenacre.Core
{
    using System;

    public static class GuardAgainst
    {
        public static void ArgumentNull<T>(T argument, string argumentName) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentLessThanMinimum<T>(T argument, T minimum, string argumentName = null)
            where T : IComparable<T>
        {
            if (argument.IsLessThan(minimum))
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void ArgumentMoreThanMaximum<T>(T argument, T maximum, string argumentName = null)
            where T : IComparable<T>
        {
            if (argument.IsMoreThan(maximum))
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void ArgumentOutOfRange<T>(T argument, T minimum, T maximum, string argumentName = null)
            where T : IComparable<T>
        {
            if (!argument.IsInRange(minimum, maximum))
            {
                throw new ArgumentOutOfRangeException(argumentName);
            }
        }

        public static void ArgumentInvalid(bool invalid, string argumentName = null)
        {
            if (invalid)
            {
                throw new ArgumentException(default(string), argumentName);
            }
        }

        public static void ArgumentInvalid<T>(Func<bool> invalid, string argumentName = null)
        {
            if (invalid())
            {
                throw new ArgumentException(default(string), argumentName);
            }
        }

        private static bool IsInRange<T>(this T item, T from, T to) where T : IComparable<T>
        {
            return item.CompareTo(from) >= 0 && item.CompareTo(to) <= 0;
        }

        private static bool IsLessThan<T>(this T item, T value) where T : IComparable<T>
        {
            return item.CompareTo(value) < 0;
        }

        private static bool IsMoreThan<T>(this T item, T value) where T : IComparable<T>
        {
            return item.CompareTo(value) > 0;
        }
    }
}