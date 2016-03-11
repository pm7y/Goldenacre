using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;

namespace Goldenacre.Core
{
    public static class Host
    {
        [DllImport("kernel32")]
        private static extern ulong GetTickCount64();

        public static long TickCount()
        {
            return (long) GetTickCount64();
        }

        public static TimeSpan UpTime()
        {
            return TimeSpan.FromMilliseconds(GetTickCount64());
        }

        public static bool CurrentUserIsAdmin()
        {
            var currentIdentity = WindowsIdentity.GetCurrent();
            return currentIdentity != null &&
                   new WindowsPrincipal(currentIdentity).IsInRole(WindowsBuiltInRole.Administrator);
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

        #region Windows version checks

        public static bool IsWinXpOrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT)
                   && ((@this.Version.Major > 5) || ((@this.Version.Major == 5) && (@this.Version.Minor >= 1)));
        }

        public static bool IsWinVistaOrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT)
                   && (@this.Version.Major >= 6);
        }

        public static bool IsWin7OrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT)
                   && ((@this.Version.Major > 6) || ((@this.Version.Major == 6) && (@this.Version.Minor >= 1)));
        }

        public static bool IsWin8OrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT)
                   && ((@this.Version.Major > 6) || ((@this.Version.Major == 6) && (@this.Version.Minor >= 2)));
        }

        public static bool IsWin81OrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT)
                   && ((@this.Version.Major > 6) || ((@this.Version.Major == 6) && (@this.Version.Minor >= 3)));
        }

        public static bool IsWin10OrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT)
                   && ((@this.Version.Major > 6) || ((@this.Version.Major == 10) && (@this.Version.Minor >= 0)));
        }

        #endregion Windows version checks
    }
}