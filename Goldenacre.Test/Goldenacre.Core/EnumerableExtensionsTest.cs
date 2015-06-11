using System.Globalization;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Goldenacre.Core
{
    [TestClass]
    public class EnumerableExtensionsTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sl = CultureInfo.GetCultures(CultureTypes.AllCultures).ToSelectList(ci => ci.DisplayName, ci => ci.LCID);
        }
    }
}