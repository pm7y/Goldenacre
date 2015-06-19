using System;
using System.Globalization;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class DateTimeExtensions
    {
        public static readonly DateTime EpochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// The years of age of a person given their DoB.
        /// </summary>
        public static int YearsOfAge(this DateTime @this)
        {
            if (DateTime.Today.Month < @this.Month ||
                DateTime.Today.Month == @this.Month && DateTime.Today.Day < @this.Day)
            {
                return DateTime.Today.Year - @this.Year - 1;
            }

            return DateTime.Today.Year - @this.Year;
        }

        /// <summary>
        /// The elapsed timespan since the given datetime.
        /// </summary>
        public static TimeSpan Elapsed(this DateTime @this)
        {
            return DateTime.UtcNow - @this.EnsureUtc();
        }

        /// <summary>
        /// Converts a DateTime into a unix timestamp.
        /// i.e. the number of seconds since 1970-01-01.
        /// </summary>
        public static long ToUnixTimestamp(this DateTime @this)
        {
            return (long)(@this.EnsureUtc() - EpochUtc).TotalSeconds;
        }

        /// <summary>
        /// Converts a unix timestamp into a datetime.
        /// i.e. the number of seconds since 1970-01-01.
        /// </summary>
        public static DateTime FromUnixTimestamp(this long @this)
        {
            return EpochUtc.AddSeconds(@this);
        }

        /// <summary>
        /// Converts a unix timestamp into a datetime.
        /// i.e. the number of seconds since 1970-01-01.
        /// </summary>
        public static DateTime FromUnixTimestamp(this int @this)
        {
            return EpochUtc.AddSeconds(@this);
        }

        /// <summary>
        ///     Converts the specified DateTime to Local if it isn't already.
        /// </summary>
        /// <param name="dt">The DateTime to convert.</param>
        /// <param name="targetTimeZone">The target time zone to convert to. If null then the machine time zone is used.</param>
        /// <returns>A local DateTime.</returns>
        public static DateTime EnsureLocal(this DateTime @this, TimeZoneInfo targetTimeZone = null)
        {
            if (@this.Kind == DateTimeKind.Unspecified)
            {
                @this = DateTime.SpecifyKind(@this, DateTimeKind.Utc);
            }
            if (targetTimeZone == null)
            {
                targetTimeZone = TimeZoneInfo.Local;
            }

            return TimeZoneInfo.ConvertTimeFromUtc(@this.ToUniversalTime(), targetTimeZone);
        }

        /// <summary>
        ///     Converts the specified DateTime to Local if it isn't already.
        /// </summary>
        /// <param name="dt">The DateTime to convert. If null then returns null.</param>
        /// <param name="targetTimeZone">The target time zone to convert to. If null then the machine time zone is used.</param>
        /// <returns>A local DateTime or null if specified DateTime is null.</returns>
        public static DateTime? EnsureLocal(this DateTime? @this, TimeZoneInfo targetTimeZone = null)
        {
            if (@this.HasValue)
            {
                return @this.Value.EnsureLocal();
            }
            return null;
        }

        /// <summary>
        ///     Converts the specified DateTime to UTC if it isn't already. If it is Unspecified then it is assumed to be UTC.
        /// </summary>
        /// <param name="dt">The DateTime to convert.</param>
        /// <returns>A UTC DateTime.</returns>
        public static DateTime EnsureUtc(this DateTime @this)
        {
            if (@this.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(@this, DateTimeKind.Utc);
            }

            return @this.ToUniversalTime();
        }

        /// <summary>
        ///     Converts the specified DateTime to UTC if it isn't already. If it is Unspecified then it is assumed to be UTC.
        /// </summary>
        /// <param name="dt">The DateTime to convert.</param>
        /// <returns>A UTC DateTime or null if specified DateTime is null.</returns>
        public static DateTime? EnsureUtc(this DateTime? @this)
        {
            if (@this.HasValue)
            {
                return @this.Value.EnsureUtc();
            }
            return null;
        }

        /// <summary>
        ///     Indicates whether or not the specified DateTime is a weekend or not.
        /// </summary>
        /// <param name="date">The DateTime to check.</param>
        /// <returns>True if DateTime is weekend.</returns>
        public static bool IsWeekend(this DateTime @this, TimeZoneInfo targetTimeZone = null)
        {
            return ((@this.EnsureLocal(targetTimeZone).DayOfWeek == DayOfWeek.Saturday) | (@this.EnsureLocal(targetTimeZone).DayOfWeek == DayOfWeek.Sunday));
        }

        /// <summary>
        ///     Indicates whether or not the specified DateTime is a weekend or not.
        /// </summary>
        /// <param name="date">The DateTime to check.</param>
        /// <returns>True if DateTime is weekend.</returns>
        public static bool IsWeekday(this DateTime @this, TimeZoneInfo targetTimeZone = null)
        {
            return ((@this.EnsureLocal(targetTimeZone).DayOfWeek != DayOfWeek.Saturday) && (@this.EnsureLocal(targetTimeZone).DayOfWeek != DayOfWeek.Sunday));
        }

        /// <summary>
        ///     Weeks the of year.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        /// <param name="weekrule">The weekrule.</param>
        /// <param name="firstDayOfWeek">The first day of week.</param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime @this, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday, CalendarWeekRule weekrule = CalendarWeekRule.FirstDay)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(@this, weekrule, firstDayOfWeek);
        }



        /// <summary>
        ///     Convert a DateTime to SQL Server formatted string with milliseconds: yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>A DateTime formatted as SQL Server string with milliseconds.</returns>
        public static string ToSqlString(this DateTime @this)
        {
            return @this.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        ///     Convert a DateTime to SQL Server formatted string without milliseconds: yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>A DateTime formatted as SQL Server string without milliseconds.</returns>
        public static string ToSqlDateTimeString(this DateTime @this)
        {
            return @this.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        ///     Convert a DateTime to SQL Server formatted date string: yyyy-MM-dd
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>A DateTime formatted as SQL Server date string.</returns>
        public static string ToSqlDateString(this DateTime @this)
        {
            return @this.ToString("yyyy-MM-dd");
        }

        /// <summary>
        ///     Convert a DateTime to a culture invariant date string: Thu 1st Jan 2015
        /// </summary>
        /// <param name="dateTime">The DateTime to convert.</param>
        /// <returns>A DateTime formatted as a culture invariant date string.</returns>
        public static string ToNiceDateString(this DateTime @this)
        {
            var suff = (@this.Day % 10 == 1 && @this.Day != 11)
                ? "st"
                : (@this.Day % 10 == 2 && @this.Day != 12)
                    ? "nd"
                    : (@this.Day % 10 == 3 && @this.Day != 13)
                        ? "rd"
                        : "th";

            return string.Format("{0:ddd d}{1} {0:MMM yyyy}", @this, suff);
        }
    }
}