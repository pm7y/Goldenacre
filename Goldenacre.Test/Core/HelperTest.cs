using System.Diagnostics;
using Goldenacre.Core;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.DirectoryServices.ActiveDirectory;
using System;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        public void Test_IsAssemblyLoaded_when_valid()
        {
            try
            {
                var list = Host.Domains();

                if (list != null)
                {
                    list.ForEach(s => Debug.WriteLine(s));
                }
            }
            catch (ActiveDirectoryOperationException ex)
            {

            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }
    }
}