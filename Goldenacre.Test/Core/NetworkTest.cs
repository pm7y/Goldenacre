// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    using Goldenacre.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NetworkTest
    {
        [TestMethod]
        public void Test_ping()
        {
            var ms = Network.Ping(5000);

            Assert.IsTrue(ms >= 0);
        }

        [TestMethod]
        public void Test_ping_with_host()
        {
            var ms = Network.Ping("www.msftconnecttest.com", 80, 5000);

            Assert.IsTrue(ms >= 0);
        }
    }
}