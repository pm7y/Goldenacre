using System.IO;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class ByteArrayExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] @this)
        {
            return new MemoryStream(@this) { Position = 0 };
        }
    }
}