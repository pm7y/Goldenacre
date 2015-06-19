using System;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class ExceptionExtensionsTest
    {
        [TestMethod]
        public void TestToLettersAndNumbersOnly()
        {
            try
            {
                int x = 0;

                int asd = 110/x;

            }
            catch (Exception ex )
            {
                var t = ex.ToLogString("It's fucked!");
            }
        }

    }
}