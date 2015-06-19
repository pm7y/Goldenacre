using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void TestToPascalCase()
        {
            Assert.AreEqual("     The Quick Brown Fox Jumped Over the Lazy Dog   ",
                "     THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG   ".ToPascalCase());

            Assert.AreEqual("     The Quick Brown Fox Jumped Over the (VERY) Lazy Dog   ",
                "     THE QUICK BROWN FOX JUMPED OVER THE (VERY) LAZY DOG   ".ToPascalCase("VERY"));

            Assert.AreEqual("     The SUPER Quick Brown Fox Jumped Over the EXTREMELY Lazy Dog   ",
                "     THE SUPER QUICK BROWN FOX JUMPED OVER THE EXTREMELY LAZY DOG   ".ToPascalCase("EXTREMELY", "SUPER"));

            Assert.AreEqual("The Quick Brown Fox Jumped Over the Lazy Dog",
                "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG".ToPascalCase());

            Assert.AreEqual("The Quick Brown Fox Jumped Over the Lazy Dog",
                "The Quick Brown Fox Jumped Over the Lazy DOG".ToPascalCase());

            Assert.AreEqual("The Quick Brown Fox Jumped Over the 2nd Lazy Dog",
                "The Quick Brown Fox Jumped Over the 2ND Lazy DOG".ToPascalCase());

            Assert.AreEqual("A", "A".ToPascalCase());
        }

        [TestMethod]
        public void TestToLettersAndNumbersOnly()
        {
            Assert.AreEqual("1", " 1 ".ToAlphaNumeric());
            Assert.AreEqual("OK", " ( OK ) ".ToAlphaNumeric());
            Assert.AreEqual("1OK2", " 1 ( OK ) 2 ".ToAlphaNumeric());
        }

        [TestMethod]
        public void TestMd5Hash()
        {
            Assert.AreEqual("ED076287532E86365E841E92BFC50D8C", "Hello World!".ToHexMd5Hash());

            Assert.AreEqual("7Qdih1MuhjZehB6Sv8UNjA==", "Hello World!".ToBase64Md5Hash());
        }
    }
}