using System;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class AppDomainExtensionsTest
    {
        [TestMethod]
        public void Test_IsAssemblyLoaded_when_valid()
        {
            var result = AppDomain.CurrentDomain.IsAssemblyLoaded("Goldenacre.Test");

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Test_IsAssemblyLoaded_when_not_valid()
        {
            var result = AppDomain.CurrentDomain.IsAssemblyLoaded("ABC");

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Test_GetLoadedAssembly_when_valid()
        {
            var result = AppDomain.CurrentDomain.GetLoadedAssembly("Goldenacre.Test");

            Assert.IsNotNull(result);
        }
    }
}