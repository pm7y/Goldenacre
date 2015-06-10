using System;
using System.Reflection;
using Goldenacre.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Goldenacre.Test
{
    [TestClass]
    public class AssemblyExtensionsTest
    {
        [TestMethod]
        public void TestGetEmbeddedResourceText()
        {
            var text = Assembly.GetExecutingAssembly().GetEmbeddedResourceText("This_is_an_embedded_resource.txt");

            Assert.AreEqual(" This is an embedded resource! ", text);
        }

        [TestMethod]
        public void TestCompilationDateTime()
        {
            var dt = Assembly.GetExecutingAssembly().GetCompilationDateTime();

            Assert.AreNotEqual(DateTime.MinValue, dt);
        }
    }
}