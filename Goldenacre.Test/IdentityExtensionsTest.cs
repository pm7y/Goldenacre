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
    public class IdentityExtensionsTest
    {
        [TestMethod]
        public void TestMethod3()
        {
            var s1 = Thread.CurrentPrincipal.Identity.NameWithoutDomain();

            WindowsIdentity windowsIdentity = new WindowsIdentity("");
            var s2 = windowsIdentity.NameWithoutDomain();

        }
    }
}
