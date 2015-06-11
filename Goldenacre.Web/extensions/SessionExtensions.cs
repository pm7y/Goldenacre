using System;
using System.Web;
using System.Web.SessionState;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class SessionExtensions
    {
        public static void Kill(this HttpSessionState session)
        {
            if (session != null)
            {
                session.Clear();
                session.Abandon();
            }
        }

        public static void Kill(this HttpSessionStateBase session)
        {
            if (session != null)
            {
                session.Clear();
                session.Abandon();
            }
        }

        /// <summary>
        /// Type friendly way to get session value.
        /// </summary>
        public static T Get<T>(this HttpSessionState session, string key)
        {
            if (session != null && session[key] != null)
            {
                var o = session[key];
                var value = (T) Convert.ChangeType(o, typeof (T));

                return value;
            }
            return default(T);
        }

        /// <summary>
        /// Type friendly way to get session value.
        /// </summary>
        public static T Get<T>(this HttpSessionStateBase session, string key)
        {
            if (session != null && session[key] != null)
            {
                var o = session[key];
                var value = (T) Convert.ChangeType(o, typeof (T));

                return value;
            }
            return default(T);
        }

        /// <summary>
        /// Type friendly way to set session value.
        /// </summary>
        public static bool Set(this HttpSessionState session, string key, object value)
        {
            if (session != null)
            {
                session[key] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Type friendly way to set session value.
        /// </summary>
        public static bool Set(this HttpSessionStateBase session, string key, object value)
        {
            if (session != null)
            {
                session[key] = value;
                return true;
            }
            return false;
        }
    }
}