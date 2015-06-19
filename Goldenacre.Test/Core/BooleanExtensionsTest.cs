using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class BooleanExtensionsTest
    {
        [TestMethod]
        public void Test_when_true_text_is_expected()
        {
            var textExpected = "WooHoo!";
            var text = true.WhenTrue(textExpected);

            Assert.AreEqual(textExpected, text);

            text = true.WhenTrue(() => { return textExpected; });

            Assert.AreEqual(textExpected, text);

            text = false.WhenTrue(textExpected);

            Assert.AreEqual(null, text);

            text = false.WhenTrue(() => { return textExpected; });

            Assert.AreEqual(null, text);
        }

        [TestMethod]
        public void Test_when_true_object_is_expected()
        {
            var expected = new object();
            var actual = true.WhenTrue(expected);

            Assert.AreEqual(expected, actual);

            actual = true.WhenTrue(() => { return expected; });

            Assert.AreEqual(expected, actual);

            actual = false.WhenTrue(expected);

            Assert.AreEqual(null, actual);

            actual = false.WhenTrue(() => { return expected; });

            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void Test_when_false_text_is_expected()
        {
            var textExpected = "Doh!";
            var text = false.WhenFalse(textExpected);

            Assert.AreEqual(textExpected, text);

            text = false.WhenFalse(() => { return textExpected; });

            Assert.AreEqual(textExpected, text);

            text = true.WhenFalse(textExpected);

            Assert.AreEqual(null, text);

            text = true.WhenFalse(() => { return textExpected; });

            Assert.AreEqual(null, text);
        }

        [TestMethod]
        public void Test_when_false_object_is_expected()
        {
            var expected = new object();
            var actual = false.WhenFalse(expected);

            Assert.AreEqual(expected, actual);

            actual = false.WhenFalse(() => { return expected; });

            Assert.AreEqual(expected, actual);

            actual = true.WhenFalse(expected);

            Assert.AreEqual(null, actual);

            actual = true.WhenFalse(() => { return expected; });

            Assert.AreEqual(null, actual);
        }
    }
}