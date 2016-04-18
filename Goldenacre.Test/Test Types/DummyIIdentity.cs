namespace Goldenacre.Test.Test_Types
{
    using System;
    using System.Security.Principal;

    internal class DummyIIdentity : IIdentity
    {
        public string AuthenticationType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get
            {
                return @"BIGCORP\joe.bloggs";
            }
        }
    }
}