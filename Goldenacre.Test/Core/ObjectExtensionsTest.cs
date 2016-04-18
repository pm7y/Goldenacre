// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    using System;

    using Goldenacre.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectExtensionsTest
    {
        [TestMethod]
        public void Test_Object_ToUpperInvariant_when_valid()
        {
            object x = "x";

            var xs = x.ToUpperInvariant();

            Assert.AreEqual("X", xs);
        }

        [TestMethod]
        public void Test_Object_ToLowerInvariant_when_valid()
        {
            object y = "Y";

            var ys = y.ToLowerInvariant();

            Assert.AreEqual("y", ys);
        }

        [TestMethod]
        public void Test_EnsureBetween_when_valid()
        {
            var min = 1;
            var max = 10;
            var val = 5;

            var result = val.EnsureBetween(min, max);

            Assert.AreEqual(val, result);
        }

        [TestMethod]
        public void Test_EnsureBetween_when_too_low()
        {
            var min = 1;
            var max = 10;
            var val = 0;

            var result = val.EnsureBetween(min, max);

            Assert.AreEqual(min, result);
        }

        [TestMethod]
        public void Test_EnsureBetween_when_too_high()
        {
            var min = 1;
            var max = 10;
            var val = 11;

            var result = val.EnsureBetween(min, max);

            Assert.AreEqual(max, result);
        }

        [TestMethod]
        public void Test_IsTruthy()
        {
            Assert.AreEqual(true, "1".IsTruthy());
            Assert.AreEqual(true, "y".IsTruthy());
            Assert.AreEqual(true, "yes".IsTruthy());
            Assert.AreEqual(true, "tRuE".IsTruthy());
            Assert.AreEqual(true, "+".IsTruthy());
            Assert.AreEqual(true, 123.IsTruthy());

            Assert.AreEqual(false, "".IsTruthy());
            Assert.AreEqual(false, "0".IsTruthy());
            Assert.AreEqual(false, "n".IsTruthy());
            Assert.AreEqual(false, "N".IsTruthy());
            Assert.AreEqual(false, "-1".IsTruthy());
            Assert.AreEqual(false, "-".IsTruthy());
            Assert.AreEqual(false, "false".IsTruthy());
            Assert.AreEqual(false, "fAlse".IsTruthy());
        }

        [TestMethod]
        public void Test_ConvertDateTimePropertiesToUtc_when_valid()
        {
            var o = new Child();

            o.PropA = DateTime.Now;
            o.PropB = null;
            o.PropC = DateTime.Now;

            o.Prop1 = DateTime.Now;
            o.Prop2 = null;
            o.Prop3 = DateTime.Now;

            o.ConvertDateTimePropertiesToUtc();

            Assert.AreEqual(DateTimeKind.Utc, o.PropA.Kind);
            Assert.AreEqual(false, o.PropB.HasValue);
            Assert.AreEqual(DateTimeKind.Utc, o.PropC.Value.Kind);

            Assert.AreEqual(DateTimeKind.Utc, o.Prop1.Kind);
            Assert.AreEqual(false, o.Prop2.HasValue);
            Assert.AreEqual(DateTimeKind.Utc, o.Prop3.Value.Kind);
        }

        private class Parent
        {
            public DateTime Prop1 { get; set; }

            public DateTime? Prop2 { get; set; }

            public DateTime? Prop3 { get; set; }
        }

        private class Child : Parent
        {
            public DateTime PropA { get; set; }

            public DateTime? PropB { get; set; }

            public DateTime? PropC { get; set; }
        }
    }
}