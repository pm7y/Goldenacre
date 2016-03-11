using System;
using System.Globalization;
using System.Threading;
using Goldenacre.Core;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class DateTimeExtensionsTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext ctx)
        {
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
        public void Test_elapsed_timespan_since_datetime()
        {
            var now = DateTime.UtcNow;
            var then = DateTime.UtcNow.AddDays(-1);

            var nowElapsed = now.Elapsed();
            var thenElapsed = then.Elapsed();

            Assert.IsTrue(nowElapsed.TotalSeconds < 1);
            Assert.IsTrue(thenElapsed.TotalHours%24 < 1);
            Assert.IsTrue((int) thenElapsed.TotalHours == 24);
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
            var unixTimestampInt = 256348800;
            long unixTimestampLong = 256348800;

            Assert.AreEqual(expected, unixTimestampInt.FromUnixTimestamp());
            Assert.AreEqual(expected, unixTimestampLong.FromUnixTimestamp());
        }

        //[TestMethod]
        //public void Test_datetime_ensure_local()
        //{
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
        //}

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
        public void Test_datetime_to_nice_string()
        {
            var dt = new DateTime(2015, 1, 11, 1, 1, 1);
            dt = dt.AddMilliseconds(123);

            Assert.AreEqual("Sun 11th Jan 2015", dt.ToNiceDateString());
        }

        [TestMethod]
        public void Test_daterange()
        {
            var fromUtc = DateTime.UtcNow.AddDays(-1);
            var toUtc = DateTime.UtcNow;

            var dr = DateRange.New(fromUtc, toUtc);
        }

        [TestMethod]
        public void Test_daterange_intersections()
        {
            var now = DateTime.UtcNow;

            var r = DateRange.New(now, now.AddDays(2));

            var doesNotIntersect1 = DateRange.New(now.AddDays(-5), now.AddDays(-4));
            var doesNotIntersect2 = DateRange.New(now.AddDays(5), now.AddDays(6));

            Assert.IsFalse(r.Intersects(doesNotIntersect1));
            Assert.IsFalse(r.Intersects(doesNotIntersect2));

            var doesIntersect1 = DateRange.New(now.AddDays(-1), now.AddDays(1));
            var doesIntersect2 = DateRange.New(now.AddDays(1), now.AddDays(3));
            var doesIntersect3 = DateRange.New(now, now.AddDays(2));
            var doesIntersect4 = DateRange.New(now.AddHours(5), now.AddDays(1));

            Assert.IsTrue(r.Intersects(doesIntersect1));
            Assert.IsTrue(r.Intersects(doesIntersect2));
            Assert.IsTrue(r.Intersects(doesIntersect3));
            Assert.IsTrue(r.Intersects(doesIntersect4));
        }

        [TestMethod]
        public void Test_daterange_contains()
        {
            var now = DateTime.UtcNow;

            var r = DateRange.New(now, now.AddDays(2));

            var doesNotContain1 = DateRange.New(now.AddDays(-5), now.AddDays(-4));
            var doesNotContain2 = DateRange.New(now.AddDays(5), now.AddDays(6));
            var doesNotContain3 = DateRange.New(now.AddDays(-1), now.AddDays(1));
            var doesNotContain4 = DateRange.New(now.AddDays(1), now.AddDays(3));

            Assert.IsFalse(r.Contains(doesNotContain1));
            Assert.IsFalse(r.Contains(doesNotContain2));
            Assert.IsFalse(r.Contains(doesNotContain3));
            Assert.IsFalse(r.Contains(doesNotContain4));

            var doesContain1 = DateRange.New(now, now.AddDays(2));
            var doesContain2 = DateRange.New(now.AddHours(5), now.AddDays(1));

            Assert.IsTrue(r.Contains(doesContain1));
            Assert.IsTrue(r.Contains(doesContain2));
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void Test_daterange_does_except_local_fromDate()
        {
            var fromLocal = DateTime.Now.AddDays(-1);

            var dr = DateRange.New(fromLocal, DateTime.UtcNow);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void Test_daterange_does_except_local_toDate()
        {
            var toLocal = DateTime.Now.AddDays(1);

            var dr = DateRange.New(DateTime.UtcNow, toLocal);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void Test_daterange_does_except_unspecified_fromDate()
        {
            var fromLocal = DateTime.SpecifyKind(DateTime.Now.AddDays(-1), DateTimeKind.Unspecified);

            var dr = DateRange.New(fromLocal, DateTime.UtcNow);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void Test_daterange_does_except_unspecified_toDate()
        {
            var toUnspecified = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(1), DateTimeKind.Unspecified);

            var dr = DateRange.New(DateTime.UtcNow, toUnspecified);
        }
    }
}