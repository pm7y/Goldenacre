using System;
using Goldenacre.Core.Extensions;

namespace Goldenacre.Core
{
    public struct DateRange
    {
        #region Methods

        public static DateRange New(DateTime f, DateTime t)
        {
            return new DateRange { From = f, To = t };
        }

        #endregion Methods

        #region Fields

        public DateTime From;
        public DateTime To;

        #endregion Fields

        #region Properties

        public DateTime FromUtc
        {
            get { return From.EnsureUtc(); }
        }

        public DateTime ToUtc
        {
            get { return To.EnsureUtc(); }
        }

        #endregion Properties
    }
}