using System;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class ObjectExtensionsTest
    {
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
        
        [TestMethod]
        public void TestConvertDateTimePropertiesToUtc()
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
            Assert.AreEqual(false, o.PropB .HasValue);
            Assert.AreEqual(DateTimeKind.Utc, o.PropC.Value.Kind);

            Assert.AreEqual(DateTimeKind.Utc, o.Prop1.Kind);
            Assert.AreEqual(false, o.Prop2.HasValue);
            Assert.AreEqual(DateTimeKind.Utc, o.Prop3.Value.Kind);
        }
    }
}