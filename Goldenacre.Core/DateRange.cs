using System;
using Goldenacre.Extensions;

namespace Goldenacre.Core
{
    public struct DateRange
    {
        public DateTime From;
        public DateTime To;

        public DateTime FromUtc => From.EnsureUtc();

        public DateTime ToUtc => To.EnsureUtc();

        public static DateRange New(DateTime f, DateTime t)
        {
            return new DateRange {From = f, To = t};
        }
    }
}