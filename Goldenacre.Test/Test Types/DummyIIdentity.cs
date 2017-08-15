namespace Goldenacre.Test.Test_Types
{
    using System;
    using System.Security.Principal;

    internal class DummyIIdentity : IIdentity
    {
        public string AuthenticationType => throw new NotImplementedException();

        public bool IsAuthenticated => true;

        public string Name => @"BIGCORP\joe.bloggs";
    }
}