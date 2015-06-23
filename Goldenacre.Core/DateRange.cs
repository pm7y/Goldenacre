using System;
using Goldenacre.Extensions;

namespace Goldenacre.Core
{
    public struct DateRange
    {
        public static DateRange New(DateTime f, DateTime t)
        {
            return new DateRange {From = f, To = t};
        }

        public DateTime From;
        public DateTime To;

        public DateTime FromUtc
        {
            get { return From.EnsureUtc(); }
        }

        public DateTime ToUtc
        {
            get { return To.EnsureUtc(); }
        }
    }
}