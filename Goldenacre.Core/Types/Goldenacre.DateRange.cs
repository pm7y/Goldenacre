namespace Goldenacre.Core
{
    using System;

    /// <summary>
    ///     A simple DateRange class representing a from and to date range in UTC.
    /// </summary>
    public struct DateRange : IEquatable<DateRange>
    {
        /// <summary>
        /// The start of the date range as a UTC DateTime.
        /// </summary>
        public DateTime FromUtc { get; private set; }

        /// <summary>
        /// The end of the date range as a UTC DateTime.
        /// </summary>
        public DateTime ToUtc { get; private set; }

        /// <summary>
        /// The start of the date range as a local DateTime.
        /// </summary>
        public DateTime FromLocal => this.FromUtc.ToLocalTime();

        /// <summary>
        /// The end of the date range as a local DateTime.
        /// </summary>
        public DateTime ToLocal => this.ToUtc.ToLocalTime();

        /// <summary>
        /// Create a new instance of the DateRange object.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentException">DateTimeKind.Unspecified is not supported.;from
        /// or
        /// DateTimeKind.Unspecified is not supported.;to
        /// or
        /// The from date must not be before the to date.</exception>
        public static DateRange New(DateTime from, DateTime to)
        {
            if (from.Kind == DateTimeKind.Unspecified || to.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException("DateTimeKind.Unspecified is not supported.");
            }

            if (from.CompareTo(to) == 1)
            {
                throw new ArgumentException("The from date must not be before the to date.");
            }

            return new DateRange { FromUtc = from.ToUniversalTime(), ToUtc = to.ToUniversalTime() };
        }

        /// <summary>
        /// Determines whether the specified date time is between start and end specified.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        private static bool IsBetween(DateTime dateTime, DateTime from, DateTime to)
        {
            return dateTime.CompareTo(from) >= 0 && dateTime.CompareTo(to) <= 0;
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
            if (IsBetween(range.FromUtc, this.FromUtc, this.ToUtc))
            {
                return true;
            }

            //
            // 2:  =====
            //        =====
            if (IsBetween(range.ToUtc, this.FromUtc, this.ToUtc))
            {
                return true;
            }

            // 3:  =====    or   =====
            //     =====          ===
            //
            if (range.FromUtc >= this.FromUtc && range.ToUtc <= this.ToUtc)
            {
                return true;
            }

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
            if (range.FromUtc >= this.FromUtc && range.ToUtc <= this.ToUtc)
            {
                return true;
            }

            // otherwise it doesn't contain
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.FromUtc.GetHashCode() + this.ToUtc.GetHashCode();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(DateRange other)
        {
            return this.FromUtc.CompareTo(other.FromUtc) == 0 && this.ToUtc.CompareTo(other.ToUtc) == 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is DateRange))
            {
                return false;
            }

            return this.Equals((DateRange)obj);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="dateRange1">The date range1.</param>
        /// <param name="dateRange2">The date range2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(DateRange dateRange1, DateRange dateRange2)
        {
            return dateRange1.Equals(dateRange2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="dateRange1">The date range1.</param>
        /// <param name="dateRange2">The date range2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(DateRange dateRange1, DateRange dateRange2)
        {
            return !dateRange1.Equals(dateRange2);
        }
    }
}