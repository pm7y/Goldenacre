using System.Diagnostics;
using Goldenacre.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class NetworkTest
    {
        [TestMethod]
        public void Test_ping()
        {
            var ms = Network.Ping("localhost", 80, 5);

            Assert.IsTrue(ms >= 0);
        }
    }
}