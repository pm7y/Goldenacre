// ReSharper disable UseStringInterpolation

namespace Goldenacre.Core
{
    using System;
    using System.Globalization;

    public static class GuardAgainst
    {
		/// <summary>
		/// Determines whether [is in range] [the specified item].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item">The item.</param>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <returns>
		///   <c>true</c> if [is in range] [the specified item]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsInRange<T>(T item, T from, T to) where T : IComparable<T>
        {
            return item.CompareTo(from) >= 0 && item.CompareTo(to) <= 0;
        }

		public static void ArgumentBeingNull<T>(T argument, string argumentName = null) where T : class
        {
            if (argument == null)
            {
                if (string.IsNullOrWhiteSpace(argumentName))
                {
                    throw new ArgumentNullException(Goldenacre_Resources.SpecifiedArgumentCannotBeNull);
                }

                throw new ArgumentNullException(string.Format(CultureInfo.CurrentUICulture, Goldenacre_Resources.ArgumentCannotBeNull, argumentName));
            }
        }

        public static void ArgumentBeingOutOfRange<T>(T argument, T min, T max, string argumentName = null) where T : IComparable<T>
        {
            if (!IsInRange(argument, min, max))
            {
                if (string.IsNullOrWhiteSpace(argumentName))
                {
                    throw new ArgumentOutOfRangeException(Goldenacre_Resources.SpecifiedArgumentIsNotWithinTheAllowedRange);
                }

                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentUICulture, Goldenacre_Resources.ArgumentIsNotWithinTheAllowedRange, argumentName));
            }
        }

        public static void ArgumentBeingInvalid<T>(T argument, Func<T, bool> isValid, string argumentName = null) where T : IComparable<T>
        {
            if (isValid == null)
            {
                throw new ArgumentNullException(Goldenacre_Resources.ArgumentCannotBeNull, nameof(isValid));
            }

            if (!isValid(argument))
            {
                if (string.IsNullOrWhiteSpace(argumentName))
                {
                    throw new ArgumentException(Goldenacre_Resources.SpecifiedArgumentIsNotValid);
                }

                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Goldenacre_Resources.ArgumentIsNotValid, argumentName));
            }
        }
    }
}