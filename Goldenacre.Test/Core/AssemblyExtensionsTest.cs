 // ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Reflection;

    using Goldenacre.Extensions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AssemblyExtensionsTest
    {
        [TestMethod]
        public void Test_Assembly_Embedded_resource_image()
        {
            var imgBytes = Assembly.GetExecutingAssembly().GetEmbeddedResourceBytes("tazmania.jpg");
            var img = Image.FromStream(new MemoryStream(imgBytes));
            var path = Path.GetTempFileName().Replace(".tmp", ".jpg");

            img.Save(path);

            File.Delete(path);
        }

        [TestMethod]
        public void Test_Assembly_Embedded_resource_text()
        {
            var text = Assembly.GetExecutingAssembly().GetEmbeddedResourceText("This is an embedded resource.txt");

            Assert.AreEqual(" This is an embedded resource! ", text);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_Assembly_InvalidOperationException_when_embedded_resource_filename_is_invalid()
        {
            var text = Assembly.GetExecutingAssembly().GetEmbeddedResourceText("Does not exist.txt", true);
        }

        [TestMethod]
        public void Test_Assembly_null_when_embedded_resource_filename_is_invalid()
        {
            var text = Assembly.GetExecutingAssembly().GetEmbeddedResourceText("Does not exist.txt", false);
        }

        [TestMethod]
        public void Test_Assembly_Compilation_DateTime_is_valid()
        {
            var dt = Assembly.GetExecutingAssembly().GetCompilationDateTimeUtc();

            Assert.AreNotEqual(DateTime.MinValue, dt);

            Assert.IsTrue(dt > DateTime.UtcNow.AddDays(-7));

            Assert.AreEqual(DateTimeKind.Utc, dt.Kind);
        }

        [TestMethod]
        public void Test_Assembly_Compilation_DateTime_of_System_dll_is_valid()
        {
            var dt = Assembly.GetAssembly(typeof(string)).GetCompilationDateTimeUtc();

            Assert.AreNotEqual(DateTime.MinValue, dt);

            Assert.IsTrue(dt > new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual(DateTimeKind.Utc, dt.Kind);
        }
    }
}