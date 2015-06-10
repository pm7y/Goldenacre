using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using mcilreavy.library.extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mcilreavy.test
{
    [TestClass]
    public class AssemblyExtensionsTest
    {

        [TestMethod]
        public void TestGetEmbeddedResourceText()
        {
            var text = Assembly.GetExecutingAssembly().GetEmbeddedResourceText("This_is_an_embedded_resource.txt");

            Assert.AreEqual("This is an embedded resource!", text);
        }

        [TestMethod]
        public void TestCompilationDateTime()
        {
            DateTime dt = Assembly.GetExecutingAssembly().GetCompilationDateTime();

            Assert.AreNotEqual(DateTime.MinValue, dt);
        }

    }
}
