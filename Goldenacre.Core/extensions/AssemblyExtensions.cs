using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Gets the text from the specified assembly resource filename.
        /// </summary>
        /// <param name="this">The assembly to retrieve the resource from.</param>
        /// <param name="filename">The filename of the resource.</param>
        /// <param name="throwError"></param>
        /// <returns>The text contents of the resource file.</returns>
        public static string GetEmbeddedResourceText(this Assembly @this, string filename, bool throwError = false)
        {
            string resourceText = null;

            if (!string.IsNullOrWhiteSpace(filename))
            {
                filename = filename.Trim().ToLowerInvariant();

                var resourceName = @this.GetManifestResourceNames().FirstOrDefault(n => n.ToLowerInvariant().Contains(filename));

                if (resourceName == null)
                {
                    if (throwError)
                    {
                        throw new InvalidOperationException("The specified file resource was not found!");
                    }
                    return null;
                }

                using (var stream = @this.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            resourceText = reader.ReadToEnd();
                        }
                    }
                }
            }
            else if (throwError)
            {
                throw new ArgumentException("The specified filename was not valid!");
            }

            return resourceText;
        }

        public static Image GetEmbeddedResourceImage(this Assembly @this, string filename, bool throwError = false)
        {
            Image resourceImage = null;

            if (!string.IsNullOrWhiteSpace(filename))
            {
                filename = filename.Trim().ToLowerInvariant();

                var resourceName = @this.GetManifestResourceNames().FirstOrDefault(n => n.ToLowerInvariant().Contains(filename));

                if (resourceName == null)
                {
                    if (throwError)
                    {
                        throw new InvalidOperationException("The specified file resource was not found!");
                    }
                    return null;
                }

                using (var stream = @this.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        resourceImage = new Bitmap(Image.FromStream(stream).Clone() as Image);
                    }
                }
            }
            else if (throwError)
            {
                throw new ArgumentException("The specified filename was not valid!");
            }

            return resourceImage;
        }

        /// <summary>
        ///     Gets the date and time that the specified assembly was compiled.
        /// </summary>
        /// <param name="this">The assemebly to inspect.</param>
        /// <returns>The datetime the assembly was compiled.</returns>
        public static DateTime GetCompilationDateTimeUtc(this Assembly @this)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            const int bufferSize = 2048;
            var buffer = new byte[bufferSize];

            using (var fs = new FileStream(@this.Location, FileMode.Open, FileAccess.Read))
            {
                fs.Read(buffer, 0, bufferSize);
            }

            var i = BitConverter.ToInt32(buffer, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, i + linkerTimestampOffset);
            var dt = secondsSince1970.FromUnixTimestamp().EnsureUtc();

            return dt;
        }
    }
}