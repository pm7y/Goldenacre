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
            var ms = Network.Ping("google.com", 80, 5);

            Assert.IsTrue(ms >= 0);
        }
    }
}