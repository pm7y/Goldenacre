using System.Collections.Generic;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class CollectionExtensionsTest
    {
        [TestMethod]
        public void Test_add_element()
        {
            var items = new List<string>().AddIfNotContains("xxx");

            items.AddIfNotContains("xxx");

            Assert.IsTrue(items.Count == 1);
        }

        [TestMethod]
        public void Test_add_element_if()
        {
            var items = new List<string>().AddIf(true, "xxx");

            Assert.IsTrue(items.Count == 1);

            items = new List<string>().AddIf(false, "xxx");

            Assert.IsTrue(items.Count == 0);
        }
    }
}