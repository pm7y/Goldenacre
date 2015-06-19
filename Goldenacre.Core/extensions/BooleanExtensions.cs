using System;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class BooleanExtensions
    {
        public static TResult WhenTrue<TResult>(this bool @this, Func<TResult> expression)
        {
            return @this ? expression() : default(TResult);
        }

        public static TResult WhenTrue<TResult>(this bool @this, TResult content)
        {
            return @this ? content : default(TResult);
        }

        public static TResult WhenFalse<TResult>(this bool @this, Func<TResult> expression)
        {
            return !@this ? expression() : default(TResult);
        }

        public static TResult WhenFalse<TResult>(this bool @this, TResult content)
        {
            return !@this ? content : default(TResult);
        }
    }
}