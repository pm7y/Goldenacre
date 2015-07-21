using System;
using System.Text;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class ByteArrayExtensionsTest
    {
        [TestMethod]
        public void Test_ByteArray_ToMemoryStream_when_valid()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello World!");
            using (var memoryStream = bytes.ToMemoryStream())
            {
                Assert.IsNotNull(memoryStream);
                Assert.AreEqual(12, memoryStream.Length);
            }
        }

        [TestMethod]
        public void Test_ByteArray_ToMemoryStream_when_empty()
        {
            byte[] bytes = new byte[0];
            using (var memoryStream = bytes.ToMemoryStream())
            {
                Assert.IsNotNull(memoryStream);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ByteArray_ToMemoryStream_when_null()
        {
            byte[] bytes = null;
            using (var memoryStream = bytes.ToMemoryStream())
            {
                //
            }
        }

    }
}