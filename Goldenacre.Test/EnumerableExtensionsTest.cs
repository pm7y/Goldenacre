using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;
using System.Threading;
using mcilreavy.library.extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mcilreavy.test
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