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
            var items = new List<string>();

            items.AddIfNotContains("xxx");
            items.AddIfNotContains("yyy");
            items.AddIfNotContains("xxx");
            items.AddIfNotContains("zzz");

            Assert.IsTrue(items.Count == 3);
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