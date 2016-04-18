 // ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    using System;

    using Goldenacre.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExceptionExtensionsTest
    {
        [TestMethod]
        public void Test_ToLettersAndNumbersOnly()
        {
            try
            {
                var x = 0;

                var asd = 110 / x;
            }
            catch (Exception ex)
            {
                var t = ex.ToLogString("UhOh!");
            }
        }
    }
}