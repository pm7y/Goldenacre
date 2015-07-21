using System;
using System.IO;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class ByteArrayExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] @this)
        {
            if (@this == null) throw new ArgumentNullException();

            return new MemoryStream(@this) {Position = 0};
        }
    }
}