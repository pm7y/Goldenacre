using System;
using System.IO;
using System.Linq;
using System.Reflection;

public static class AssemblyExtensions
{
    public static string GetEmbeddedResourceText(this Assembly assembly, string filename)
    {
        string resourceText = null;

        if (filename != null && filename.Contains(".") && filename.Length >= 3)
        {
            filename = filename.Trim().ToLowerInvariant();

            var resourceName =
                assembly.GetManifestResourceNames().FirstOrDefault(n => n.ToLowerInvariant().Contains(filename));

            if (resourceName == null)
            {
                throw new InvalidOperationException("The specified file resource was not found!");
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
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
        else
        {
            throw new ArgumentException("The specified filename was not valid!");
        }

        return resourceText;
    }

    public static DateTime GetCompilationDateTime(this Assembly assembly)
    {
        var filePath = assembly.Location;
        const int peHeaderOffset = 60;
        const int linkerTimestampOffset = 8;
        var b = new byte[2048];
        Stream s = null;

        try
        {
            s = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            s.Read(b, 0, 2048);
        }
        finally
        {
            if (s != null)
            {
                s.Close();
            }
        }

        var i = BitConverter.ToInt32(b, peHeaderOffset);
        var secondsSince1970 = BitConverter.ToInt32(b, i + linkerTimestampOffset);
        var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        dt = dt.AddSeconds(secondsSince1970);
        dt = dt.ToLocalTime();
        return dt;
    }
}