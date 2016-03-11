using System;
using Goldenacre.Extensions;

namespace Goldenacre.Core
{
    /// <summary>
    ///     A simple DateRange class representing a from and to date in UTC.
    /// </summary>
    public struct DateRange
    {
        /// <summary>
        ///     The start of the date range.
        /// </summary>
        public DateTime FromUtc { get; private set; }

        /// <summary>
        ///     The end of the date range
        /// </summary>
        public DateTime ToUtc { get; private set; }

        /// <summary>
        ///     Create a new instance of the DateRange object.
        /// </summary>
        /// <param name="fromUtc"></param>
        /// <param name="toUtc"></param>
        /// <returns></returns>
        public static DateRange New(DateTime fromUtc, DateTime toUtc)
        {
            if (fromUtc.Kind != DateTimeKind.Utc)
                throw new ArgumentException("The from date should be specified as UTC.");
            if (toUtc.Kind != DateTimeKind.Utc) throw new ArgumentException("The to date should be specified as UTC.");
            if (fromUtc.CompareTo(toUtc) == 1)
                throw new ArgumentException("The from date must not be before the to date.");

            return new DateRange {FromUtc = fromUtc, ToUtc = toUtc};
        }

        /// <summary>
        ///     Check to see if the ranges intersect each other.
        /// </summary>
        /// <param name="range">The range to check against.</param>
        /// <returns>True if they intersect.</returns>
        public bool Intersects(DateRange range)
        {
            // There are 3 ways ranges can intersect:
            // 1:     =====
            //     =====
            if (range.FromUtc.IsBetween(FromUtc, ToUtc)) return true;

            //
            // 2:  =====
            //        =====
            if (range.ToUtc.IsBetween(FromUtc, ToUtc)) return true;

            // 3:  =====    or   =====
            //     =====          ===
            //
            if (range.FromUtc >= FromUtc && range.ToUtc <= ToUtc) return true;

            // otherwise it doesn't intersect
            return false;
        }

        /// <summary>
        ///     Check to see if the range is entirely contained inside the other.
        /// </summary>
        /// <param name="range">The range to check against.</param>
        /// <returns>True if it is contained within the range.</returns>
        public bool Contains(DateRange range)
        {
            //     =====    or   =====
            //     =====          ===
            //
            if (range.FromUtc >= FromUtc && range.ToUtc <= ToUtc) return true;

            // otherwise it doesn't contain
            return false;
        }
    }
}