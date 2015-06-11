using System.IO;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] buffer)
        {
            var ms = new MemoryStream(buffer) {Position = 0};
            return ms;
        }
    }
}