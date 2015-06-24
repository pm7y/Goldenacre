using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test

{
    [TestClass]
    [Ignore]
    public class EmptyTestClass
    {
        private const int TimeoutInMilliseconds = 100;
        private static TestContext Context { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext ctx)
        {
            Context = ctx;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            //
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // runs before each test
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // runs after each test
        }

        [TestMethod]
        [Timeout(TimeoutInMilliseconds)]
        public void TestMethodExample()
        {
            //var ctx = TestContext;
            Assert.AreEqual(1, 1);
        }

        [TestMethod]
        [Timeout(TimeoutInMilliseconds)]
        [ExpectedException(typeof (Exception))]
        public void TestExceptionExample()
        {
            throw new Exception("doh!");
        }
    }
}