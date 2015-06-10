using System;

namespace Goldenacre.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime EnsureLocal(this DateTime dt, TimeZoneInfo targetTimeZone = null)
        {
            if (dt.Kind == DateTimeKind.Unspecified)
            {
                dt = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }
            if (targetTimeZone == null)
            {
                targetTimeZone = TimeZoneInfo.Local;
            }

            return TimeZoneInfo.ConvertTimeFromUtc(dt.ToUniversalTime(), targetTimeZone);
        }

        public static DateTime? EnsureLocal(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.EnsureLocal();
            }
            return null;
        }

        public static DateTime EnsureUtc(this DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }

            return dt.ToUniversalTime();
        }

        public static DateTime? EnsureUtc(this DateTime? dt)
        {
            if (dt.HasValue)
            {
                return dt.Value.EnsureUtc();
            }
            return null;
        }

        /// <summary>
        ///     Indicates whether or not the specified DateTime is a weekend or not.
        /// </summary>
        /// <param name="date">The DateTime to check.</param>
        /// <returns>True if DateTime is weekend.</returns>
        public static bool IsWeekend(this DateTime date)
        {
            return ((date.DayOfWeek == DayOfWeek.Saturday) | (date.DayOfWeek == DayOfWeek.Sunday));
        }

        /// <summary>
        ///     Indicates whether or not the specified DateTime is a weekend or not.
        /// </summary>
        /// <param name="date">The DateTime to check.</param>
        /// <returns>True if DateTime is weekend.</returns>
        public static bool IsWeekday(this DateTime date)
        {
            return ((date.DayOfWeek != DayOfWeek.Saturday) && (date.DayOfWeek != DayOfWeek.Sunday));
        }

        public static string ToSqlString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static string ToSqlDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToSqlDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        public static string ToNiceDate(this DateTime d)
        {
            var suff = (d.Day%10 == 1 && d.Day != 11)
                ? "st"
                : (d.Day%10 == 2 && d.Day != 12)
                    ? "nd"
                    : (d.Day%10 == 3 && d.Day != 13)
                        ? "rd"
                        : "th";

            return string.Format("{0:ddd d}{1} {0:MMM yyyy}", d, suff);
        }
    }
}