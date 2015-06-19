using System;
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
            var items = new List<string>().AddElement("xxx");

            Assert.IsTrue(items.Count == 1);
        }

        [TestMethod]
        public void Test_add_element_if()
        {
            var items = new List<string>().AddElementIf(true,"xxx");

            Assert.IsTrue(items.Count == 1);

            items = new List<string>().AddElementIf(false, "xxx");

            Assert.IsTrue(items.Count == 0);
        }

    }

}