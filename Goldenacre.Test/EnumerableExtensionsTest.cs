using System.Globalization;
using Goldenacre.Web.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Goldenacre.Test
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