using System;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Goldenacre.Core
{
    [TestClass]
    public class DateTimeExtensionsTest
    {
        [TestMethod]
        public void TestEnsureLocal()
        {
            var localNow = DateTime.Now;
            var utcNow = localNow.ToUniversalTime();
            var unspecifiedNow = DateTime.SpecifyKind(localNow.ToUniversalTime(), DateTimeKind.Unspecified);

            var localNowNull = new DateTime?(localNow);
            var utcNowNull = new DateTime?(utcNow);
            var unspecifiedNowNull = new DateTime?(unspecifiedNow);

            Assert.AreEqual(localNow, utcNow.EnsureLocal());
            Assert.AreEqual(localNow, unspecifiedNow.EnsureLocal());

            Assert.AreEqual(localNowNull, utcNowNull.EnsureLocal());
            Assert.AreEqual(localNowNull, unspecifiedNowNull.EnsureLocal());
        }

        [TestMethod]
        public void TestEnsureUtc()
        {
            var utcNow = DateTime.UtcNow;
            var localNow = utcNow.ToLocalTime();
            var unspecifiedNow = DateTime.SpecifyKind(utcNow, DateTimeKind.Unspecified);

            var localNowNull = new DateTime?(localNow);
            var utcNowNull = new DateTime?(utcNow);
            var unspecifiedNowNull = new DateTime?(unspecifiedNow);

            Assert.AreEqual(utcNow, localNow.EnsureUtc());
            Assert.AreEqual(utcNow, unspecifiedNow.EnsureUtc());

            Assert.AreEqual(utcNowNull, localNowNull.EnsureUtc());
            Assert.AreEqual(utcNowNull, unspecifiedNowNull.EnsureUtc());
        }

        [TestMethod]
        public void TestIsWeekendWeekDay()
        {
            Assert.AreEqual(true, new DateTime(2015, 1, 1).IsWeekday());
            Assert.AreEqual(false, new DateTime(2015, 1, 1).IsWeekend());
        }

        [TestMethod]
        public void TestToDateStrings()
        {
            var dt = new DateTime(2015, 1, 11, 1, 1, 1);
            dt = dt.AddMilliseconds(123);

            Assert.AreEqual("2015-01-11", dt.ToSqlDateString());
            Assert.AreEqual("2015-01-11 01:01:01", dt.ToSqlDateTimeString());
            Assert.AreEqual("2015-01-11 01:01:01.123", dt.ToSqlString());
            Assert.AreEqual("Sun 11th Jan 2015", dt.ToNiceDate());
        }
    }
}