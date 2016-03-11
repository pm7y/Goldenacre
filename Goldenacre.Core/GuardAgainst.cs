using System;
using Goldenacre.Extensions;

namespace Goldenacre.Core
{
    public static class GuardAgainst
    {
        public static void ArgumentNull(object argument, string argumentName = null)
        {
            if (argument == null)
            {
                argumentName = argumentName.IsNullOrWhiteSpace() ? "The specified argument" : argumentName;

                throw new ArgumentNullException("{0} must not be null.".F(argumentName));
            }
        }

        public static void ArgumentOutOfRange<T>(T argument, T min, T max, string argumentName = null)
            where T : IComparable<T>
        {
            if (!argument.IsBetween(min, max))
            {
                argumentName = argumentName.IsNullOrWhiteSpace() ? "The specified argument" : argumentName;

                throw new ArgumentOutOfRangeException("{0} is not within the specified range.".F(argumentName));
            }
        }

        public static void ArgumentInvalid<T>(T argument, Func<T, bool> isValid, string argumentName = null)
            where T : IComparable<T>
        {
            if (!isValid(argument))
            {
                argumentName = argumentName.IsNullOrWhiteSpace() ? "The specified argument" : argumentName;

                throw new ArgumentException("{0} is not valid.".F(argumentName));
            }
        }
    }
}