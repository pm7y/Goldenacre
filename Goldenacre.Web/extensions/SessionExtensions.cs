using System;
using System.Web;
using System.Web.SessionState;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class SessionExtensions
    {
        public static T GetData<T>(this HttpSessionState session, string key)
        {
            if (session != null && session[key] != null)
            {
                var o = session[key];
                var value = (T) Convert.ChangeType(o, typeof (T));

                return value;
            }
            return default(T);
        }

        public static T GetData<T>(this HttpSessionStateBase session, string key)
        {
            if (session != null && session[key] != null)
            {
                var o = session[key];
                var value = (T) Convert.ChangeType(o, typeof (T));

                return value;
            }
            return default(T);
        }

        public static void SetData(this HttpSessionState session, string key, object value)
        {
            session[key] = value;
        }

        public static void SetData(this HttpSessionStateBase session, string key, object value)
        {
            session[key] = value;
        }
    }
}