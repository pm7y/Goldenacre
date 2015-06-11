using System;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class BooleanExtensions
    {
        public static TResult WhenTrue<TResult>(this bool value, Func<TResult> expression)
        {
            return value ? expression() : default(TResult);
        }

        public static TResult WhenTrue<TResult>(this bool value, TResult content)
        {
            return value ? content : default(TResult);
        }

        public static TResult WhenFalse<TResult>(this bool value, Func<TResult> expression)
        {
            return !value ? expression() : default(TResult);
        }

        public static TResult WhenFalse<TResult>(this bool value, TResult content)
        {
            return !value ? content : default(TResult);
        }
    }
}