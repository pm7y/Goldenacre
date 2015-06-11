using System;
using System.Globalization;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0);

        public static int YearsOfAge(this DateTime dateOfBirth)
        {
            if (DateTime.Today.Month < dateOfBirth.Month ||
                DateTime.Today.Month == dateOfBirth.Month && DateTime.Today.Day < dateOfBirth.Day)
            {
                return DateTime.Today.Year - dateOfBirth.Year - 1;
            }

            return DateTime.Today.Year - dateOfBirth.Year;
        }

        /// <summary>
        ///     Elapsed time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan Elapsed(this DateTime datetime)
        {
            return DateTime.UtcNow - datetime.EnsureUtc();
        }

        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long) (dateTime - Epoch).TotalSeconds;
        }

        public static DateTime ToDateTime(this long unixDateTime)
        {
            return Epoch.AddSeconds(unixDateTime);
        }

        /// <summary>
        ///     Converts the specified DateTime to Local if it isn't already.
        /// </summary>
        /// <param name="dt">The DateTime to convert.</param>
        /// <param name="targetTimeZone">The target time zone to convert to. If null then the machine time zone is used.</param>
        /// <returns>A local DateTime.</returns>
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

        /// <summary>
        ///     Converts the specified DateTime to Local if it isn't already.
        /// </summary>
        /// <param name="dt">The DateTime to convert. If null then returns null.</param>
        /// <param name="targetTimeZone">The target time zone to convert to. If null then the machine time zone is used.</param>
        /// <returns>A local DateTime or null if specified DateTime is null.</returns>
        public static DateTime? EnsureLocal(this DateTime? dt, TimeZoneInfo targetTimeZone = null)
        {
            if (dt.HasValue)
            {
                return dt.Value.EnsureLocal();
            }
            return null;
        }

        /// <summary>
        ///     Converts the specified DateTime to UTC if it isn't already. If it is Unspecified then it is assumed to be UTC.
        /// </summary>
        /// <param name="dt">The DateTime to convert.</param>
        /// <returns>A UTC DateTime.</returns>
        public static DateTime EnsureUtc(this DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
            }

            return dt.ToUniversalTime();
        }

        /// <summary>
        ///     Converts the specified DateTime to UTC if it isn't already. If it is Unspecified then it is assumed to be UTC.
        /// </summary>
        /// <param name="dt">The DateTime to convert.</param>
        /// <returns>A UTC DateTime or null if specified DateTime is null.</returns>
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
        ///     Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="weekrule">The weekrule.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime datetime, CalendarWeekRule weekrule = CalendarWeekRule.FirstDay,
            DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(datetime, weekrule, firstDayOfWeek);
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

        /// <summary>
        ///     Convert a DateTime to SQL Server formatted string with milliseconds: yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>A DateTime formatted as SQL Server string with milliseconds.</returns>
        public static string ToSqlString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        ///     Convert a DateTime to SQL Server formatted string without milliseconds: yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>A DateTime formatted as SQL Server string without milliseconds.</returns>
        public static string ToSqlDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        ///     Convert a DateTime to SQL Server formatted date string: yyyy-MM-dd
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>A DateTime formatted as SQL Server date string.</returns>
        public static string ToSqlDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        ///     Convert a DateTime to a culture invariant date string: Thu 1st Jan 2015
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>A DateTime formatted as a culture invariant date string.</returns>
        public static string ToNiceDate(this DateTime dateTime)
        {
            var suff = (dateTime.Day%10 == 1 && dateTime.Day != 11)
                ? "st"
                : (dateTime.Day%10 == 2 && dateTime.Day != 12)
                    ? "nd"
                    : (dateTime.Day%10 == 3 && dateTime.Day != 13)
                        ? "rd"
                        : "th";

            return string.Format("{0:ddd dateTime}{1} {0:MMM yyyy}", dateTime, suff);
        }
    }
}