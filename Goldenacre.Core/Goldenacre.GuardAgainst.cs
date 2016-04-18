// ReSharper disable UseStringInterpolation

namespace Goldenacre.Core
{
    using System;

    public static class GuardAgainst
    {
        private static bool IsBetween<T>(T item, T from, T to) where T : IComparable<T>
        {
            return item.CompareTo(from) >= 0 && item.CompareTo(to) <= 0;
        }

        public static void ArgumentNull(object argument, string argumentName = null)
        {
            if (argument == null)
            {
                argumentName = string.IsNullOrWhiteSpace(argumentName) ? "The specified argument" : argumentName;

                throw new ArgumentNullException(string.Format("{0} must not be null.", argumentName));
            }
        }

        public static void ArgumentOutOfRange<T>(T argument, T min, T max, string argumentName = null) where T : IComparable<T>
        {
            if (!IsBetween(argument, min, max))
            {
                argumentName = string.IsNullOrWhiteSpace(argumentName) ? "The specified argument" : argumentName;

                throw new ArgumentOutOfRangeException(string.Format("{0} is not within the specified range.", argumentName));
            }
        }

        public static void ArgumentInvalid<T>(T argument, Func<T, bool> isValid, string argumentName = null) where T : IComparable<T>
        {
            if (!isValid(argument))
            {
                argumentName = string.IsNullOrWhiteSpace(argumentName) ? "The specified argument" : argumentName;

                throw new ArgumentException(string.Format("{0} is not valid.", argumentName));
            }
        }
    }
}