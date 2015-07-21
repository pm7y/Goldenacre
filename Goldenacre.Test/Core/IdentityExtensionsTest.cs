using System;
using System.Security.Principal;
using Goldenacre.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable UnusedVariable

namespace Goldenacre.Test.Core
{
    [TestClass]
    public class IdentityExtensionsTest
    {
        [TestMethod]
        public void Test_NameWithoutDomain_when_valid()
        {
            var iidentity = new DummyIIdentity();

            var name = iidentity.NameWithoutDomain();

            Assert.AreEqual("joe.bloggs", name);
        }

        [TestMethod]
        public void Test_NameWithoutDomain_when_null()
        {
            var iidentity = new DummyIIdentityNullName();

            var name = iidentity.NameWithoutDomain();

            Assert.AreEqual(null, name);
        }

        [TestMethod]
        public void Test_NameWithoutDomain_when_empty()
        {
            var iidentity = new DummyIIdentityEmptyName();

            var name = iidentity.NameWithoutDomain();

            Assert.AreEqual(string.Empty, name);
        }
    }

    internal class DummyIIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return @"BIGCORP\joe.bloggs"; }
        }
    }

    internal class DummyIIdentityNullName : IIdentity
    {
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return null; }
        }
    }

    internal class DummyIIdentityEmptyName : IIdentity
    {
        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return ""; }
        }
    }
}