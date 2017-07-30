namespace Goldenacre.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Goldenacre.Core;

    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Get the full resource name path for a given resourceFileName.
        /// </summary>
        public static string GetFullResourcePathForFileName(this Assembly @this, string resourceFileName)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            resourceFileName = resourceFileName.Trim().ToUpperInvariant();

            var resourceNames = @this.GetManifestResourceNames();
            
            var resourceName = resourceNames.FirstOrDefault(n => n.ToUpperInvariant().EndsWith(string.Concat(".", resourceFileName.ToUpperInvariant()), StringComparison.OrdinalIgnoreCase));

            return resourceName;
        }

        /// <summary>
        /// Gets the text from the specified assembly resource resourceFileName.
        /// </summary>
        /// <param name="this">The assembly to retrieve the resource from.</param>
        /// <param name="fileName">The resourceFileName of the resource.</param>
        /// <returns>
        /// The text contents of the resource file.
        /// </returns>
        public static string GetEmbeddedResourceText(this Assembly @this, string fileName)
        {
            return GetEmbeddedResourceText(@this, fileName, false);
        }

        /// <summary>
        /// Gets the text from the specified assembly resource resourceFileName.
        /// </summary>
        /// <param name="this">The assembly to retrieve the resource from.</param>
        /// <param name="fileName">The resourceFileName of the resource.</param>
        /// <param name="throwErrorIfNotFound">if set to <c>true</c> [throw error if not found].</param>
        /// <returns>
        /// The text contents of the resource file.
        /// </returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ArgumentException">fileName</exception>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string GetEmbeddedResourceText(this Assembly @this, string fileName, bool throwErrorIfNotFound)
        {
            string resourceText = null;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var resourceName = GetFullResourcePathForFileName(@this, fileName);

                if (resourceName == null)
                {
                    if (throwErrorIfNotFound)
                    {
                        throw new InvalidOperationException("The specified resource was not found.");
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
            else if (throwErrorIfNotFound)
            {
                throw new ArgumentException(default(string), "fileName");
            }

            return resourceText;
        }

        /// <summary>
        /// Gets the embedded resource bytes.
        /// </summary>
        /// <param name="this">The assembly.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ArgumentException">fileName</exception>
        public static byte[] GetEmbeddedResourceBytes(this Assembly @this, string fileName)
        {
            return GetEmbeddedResourceBytes(@this, fileName, false);
        }

        /// <summary>
        /// Gets the embedded resource bytes.
        /// </summary>
        /// <param name="this">The assembly.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="throwErrorIfNotFound">if set to <c>true</c> [throw error if not found].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.ArgumentException">fileName</exception>
        public static byte[] GetEmbeddedResourceBytes(this Assembly @this, string fileName, bool throwErrorIfNotFound)
        {
            byte[] resourceBytes = null;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var resourceName = GetFullResourcePathForFileName(@this, fileName);

                if (resourceName == null)
                {
                    if (throwErrorIfNotFound)
                    {
                        throw new InvalidOperationException("The specified resource was not found.");
                    }
                    return null;
                }

                using (var stream = @this.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        var mstream = stream as MemoryStream;
                        if (mstream != null)
                        {
                            resourceBytes = mstream.ToArray();
                        }
                        else
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                resourceBytes = memoryStream.ToArray();
                            }
                        }
                    }
                }
            }
            else if (throwErrorIfNotFound)
            {
                throw new ArgumentException(default(string), "fileName");
            }

            return resourceBytes;
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