using System;
using System.Globalization;
using System.Threading;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class DateTimeExtensionsTest
    {
        private static TestContext Context { get; set; }
        private const int TimeoutInMilliseconds = 100;

        [ClassInitialize]
        public static void ClassInitialize(TestContext ctx)
        {
            Context = ctx;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InstalledUICulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InstalledUICulture;
        }

        [TestMethod]
        public void Test_datetime_years_of_age()
        {
            var dob = new DateTime(1978, 2, 15);

            Assert.AreEqual(37, dob.YearsOfAge());
        }


        [TestMethod]
        public void Test_datetime_years_of_age_leap_year()
        {
            var dob = new DateTime(1952, 2, 29);

            Assert.AreEqual(63, dob.YearsOfAge());
        }


        [TestMethod]
        public void Test_elapsed_timespan_since_datetime()
        {
            var now = DateTime.UtcNow;
            var then = DateTime.UtcNow.AddDays(-1);

            var nowElapsed = now.Elapsed();
            var thenElapsed = then.Elapsed();

            Assert.IsTrue(nowElapsed.TotalSeconds < 1);
            Assert.IsTrue(thenElapsed.TotalHours % 24 == 0);
            Assert.IsTrue(((int)thenElapsed.TotalHours) == 24);
        }


        [TestMethod]
        public void Test_datetime_to_unix_timestamp()
        {
            var date = new DateTime(1978, 02, 15).EnsureUtc();
            var expected = 256348800;

            Assert.AreEqual(expected, date.ToUnixTimestamp());
        }


        [TestMethod]
        public void Test_datetime_from_unix_timestamp()
        {
            var expected = new DateTime(1978, 02, 15).EnsureUtc();
            int unixTimestampInt = 256348800;
            long unixTimestampLong = 256348800;

            Assert.AreEqual(expected, unixTimestampInt.FromUnixTimestamp());
            Assert.AreEqual(expected, unixTimestampLong.FromUnixTimestamp());
        }


        [TestMethod]
        public void Test_datetime_ensure_local()
        {
            //Context.WriteLine("jhjhj");

            //var localNow = DateTime.Now;
            //var utcNow = localNow.ToUniversalTime();
            //var unspecifiedNow = DateTime.SpecifyKind(localNow.ToUniversalTime(), DateTimeKind.Unspecified);

            //var localNowNull = new DateTime?(localNow);
            //var utcNowNull = new DateTime?(utcNow);
            //var unspecifiedNowNull = new DateTime?(unspecifiedNow);

            //var newYearUtc = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //var tz = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
            //var brisLocal = new DateTime(2000, 1, 1, 10, 0, 0, DateTimeKind.Local);

            //Assert.AreEqual(localNow, utcNow.EnsureLocal());
            //Assert.AreEqual(localNow, unspecifiedNow.EnsureLocal());

            //Assert.AreEqual(localNowNull, utcNowNull.EnsureLocal());
            //Assert.AreEqual(localNowNull, unspecifiedNowNull.EnsureLocal());

            //Assert.AreEqual(brisLocal, newYearUtc.EnsureLocal(tz));
        }

        [TestMethod]
        public void Test_datetime_ensure_utc()
        {
            var utcNow = DateTime.UtcNow;
            var localNow = utcNow.ToLocalTime();
            var unspecifiedNow = DateTime.SpecifyKind(utcNow, DateTimeKind.Unspecified);

            var localNowNull = new DateTime?(localNow);
            var utcNowNull = new DateTime?(utcNow);
            var unspecifiedNowNull = new DateTime?(unspecifiedNow);

            var newYearLocal = new DateTime(2000, 1, 1, 10, 0, 0, DateTimeKind.Local);
            var brisUtc = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            Assert.AreEqual(utcNow, localNow.EnsureUtc());
            Assert.AreEqual(utcNow, unspecifiedNow.EnsureUtc());

            Assert.AreEqual(utcNowNull, localNowNull.EnsureUtc());
            Assert.AreEqual(utcNowNull, unspecifiedNowNull.EnsureUtc());

            Assert.AreEqual(brisUtc, newYearLocal.EnsureUtc());
        }

        [TestMethod]
        public void Test_datetime_is_weekday()
        {
            Assert.IsTrue(new DateTime(2015, 1, 1).IsWeekday());
            Assert.IsFalse(new DateTime(2015, 1, 3).IsWeekday());
        }

        [TestMethod]
        public void Test_datetime_is_weekend()
        {
            Assert.IsFalse(new DateTime(2015, 1, 1).IsWeekend());
            Assert.IsTrue(new DateTime(2015, 1, 3).IsWeekend());
        }

        [TestMethod]
        public void Test_datetime_week_of_year()
        {
            var date = new DateTime(1978, 02, 15).EnsureUtc();
            var expected = 7;

            Assert.AreEqual(expected, date.WeekOfYear());
        }

        [TestMethod]
        public void Test_datetime_to_sql_string()
        {
            var dt = new DateTime(2015, 1, 11, 1, 1, 1);
            dt = dt.AddMilliseconds(123);

            Assert.AreEqual("2015-01-11", dt.ToSqlDateString());
            Assert.AreEqual("2015-01-11 01:01:01", dt.ToSqlDateTimeString());
            Assert.AreEqual("2015-01-11 01:01:01.123", dt.ToSqlString());
        }

        [TestMethod]
        public void Test_datetime_to_nice_string()
        {
            var dt = new DateTime(2015, 1, 11, 1, 1, 1);
            dt = dt.AddMilliseconds(123);

            Assert.AreEqual("Sun 11th Jan 2015", dt.ToNiceDateString());
        }
    }
}