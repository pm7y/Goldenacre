using System;
using System.Web;

namespace mcilreavy.library
{
    /// <summary>
    ///     http://stackoverflow.com/questions/22568065/how-do-i-get-hold-of-the-current-httpcontextbase-in-an-authorizeattribute
    /// </summary>
    public class HttpContextFactory
    {
        private static HttpContextBase _context;

        public static HttpContextBase Current
        {
            get
            {
                if (_context != null)
                {
                    return _context;
                }

                if (HttpContext.Current == null)
                {
                    throw new InvalidOperationException("HttpContext not available");
                }

                return (_context = new HttpContextWrapper(HttpContext.Current));
            }
        }
    }
}