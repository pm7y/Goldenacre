using System.Diagnostics;
using Goldenacre.Core;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        public void Test_IsAssemblyLoaded_when_valid()
        {
            var list = Host.Domains();

            list.ForEach(s => Debug.WriteLine(s));
        }
    }
}