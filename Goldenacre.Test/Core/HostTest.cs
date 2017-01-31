// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    using Goldenacre.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HostTest
    {
        [TestMethod]
        public void Test_CurrentUserIsAdmin()
        {
            var result = Host.CurrentUserIsAdmin();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Test_FriendlyOsName()
        {
            var result = Host.FriendlyOsName();

            Assert.IsTrue(result.StartsWith("Microsoft Windows "));
        }

        [TestMethod]
        public void Test_TickCount()
        {
            var result = Host.TickCount();

            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void Test_UpTime()
        {
            var result = Host.Uptime();

            Assert.IsTrue(result.TotalMilliseconds > 0);
        }
    }
}