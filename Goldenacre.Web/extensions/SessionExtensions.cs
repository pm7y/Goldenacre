using System;
using System.Web;
using System.Web.SessionState;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class SessionExtensions
    {
        public static void Kill(this HttpSessionState @this)
        {
            if (@this != null)
            {
                @this.Clear();
                @this.Abandon();
            }
        }

        public static void Kill(this HttpSessionStateBase @this)
        {
            if (@this != null)
            {
                @this.Clear();
                @this.Abandon();
            }
        }

        /// <summary>
        ///     Type friendly way to get session value.
        /// </summary>
        public static T Get<T>(this HttpSessionState @this, string key)
        {
            if (@this != null && @this[key] != null)
            {
                var o = @this[key];
                var value = (T) Convert.ChangeType(o, typeof (T));

                return value;
            }
            return default(T);
        }

        /// <summary>
        ///     Type friendly way to get session value.
        /// </summary>
        public static T Get<T>(this HttpSessionStateBase @this, string key)
        {
            if (@this != null && @this[key] != null)
            {
                var o = @this[key];
                var value = (T) Convert.ChangeType(o, typeof (T));

                return value;
            }
            return default(T);
        }

        /// <summary>
        ///     Type friendly way to set session value.
        /// </summary>
        public static bool Set(this HttpSessionState @this, string key, object value)
        {
            if (@this != null)
            {
                @this[key] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Type friendly way to set session value.
        /// </summary>
        public static bool Set(this HttpSessionStateBase @this, string key, object value)
        {
            if (@this != null)
            {
                @this[key] = value;
                return true;
            }
            return false;
        }
    }
}