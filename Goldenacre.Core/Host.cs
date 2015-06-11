using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Goldenacre.Core
{
    public static class Host
    {
        [DllImport("kernel32")]
        public static extern ulong GetTickCount64();

        public static TimeSpan GetUpTime()
        {
            return TimeSpan.FromMilliseconds(GetTickCount64());
        }

        public static bool IsWinXpOrHigher(this OperatingSystem os)
        {
            return (os.Platform == PlatformID.Win32NT)
                   && ((os.Version.Major > 5) || ((os.Version.Major == 5) && (os.Version.Minor >= 1)));
        }

        public static bool IsWinVistaOrHigher(this OperatingSystem os)
        {
            return (os.Platform == PlatformID.Win32NT)
                   && (os.Version.Major >= 6);
        }

        public static bool IsWin7OrHigher(this OperatingSystem os)
        {
            return (os.Platform == PlatformID.Win32NT)
                   && ((os.Version.Major > 6) || ((os.Version.Major == 6) && (os.Version.Minor >= 1)));
        }

        public static bool IsWin8OrHigher(this OperatingSystem os)
        {
            return (os.Platform == PlatformID.Win32NT)
                   && ((os.Version.Major > 6) || ((os.Version.Major == 6) && (os.Version.Minor >= 2)));
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static string FriendlyOsName()
        {
            var productName =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion")
                    .GetValue("ProductName") as string;
            var csdVersion =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("CSDVersion")
                    as string;
            if (productName != string.Empty)
            {
                return (productName.StartsWith("Microsoft") ? string.Empty : "Microsoft ") + productName +
                       (csdVersion != string.Empty ? " " + csdVersion : string.Empty);
            }
            return string.Empty;
        }
    }
}