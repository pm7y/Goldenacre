using System.IO;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] @this)
        {
            var ms = new MemoryStream(@this) { Position = 0 };
            return ms;
        }
    }
}