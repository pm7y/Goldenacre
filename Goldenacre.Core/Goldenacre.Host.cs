namespace Goldenacre.Core
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Principal;

    using Microsoft.Win32;

    public static class Host
    {
        public static long TickCount()
        {
            return Convert.ToInt64(NativeMethods.GetTickCount64());
        }

        public static TimeSpan UpTime()
        {
            return TimeSpan.FromMilliseconds(NativeMethods.GetTickCount64());
        }

        public static bool CurrentUserIsAdmin()
        {
            var currentIdentity = WindowsIdentity.GetCurrent();

            if (currentIdentity == null || currentIdentity.IsAnonymous || currentIdentity.IsGuest || !currentIdentity.IsAuthenticated)
            {
                return false;
            }

            return new WindowsPrincipal(currentIdentity).IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static string FriendlyOsName()
        {
            var currentVersionKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            if (currentVersionKey != null)
            {
                var productName = currentVersionKey.GetValue("ProductName") as string;
                var csdVersion = currentVersionKey.GetValue("CSDVersion") as string;

                if (!string.IsNullOrWhiteSpace(productName))
                {
                    return (productName.StartsWith("Microsoft") ? string.Empty : "Microsoft ") + productName + (csdVersion != string.Empty ? " " + csdVersion : string.Empty);
                }
            }
            return string.Empty;
        }

        private static class NativeMethods
        {
            [DllImport("kernel32")]
            internal static extern ulong GetTickCount64();
        }

        #region Windows version checks

        public static bool IsWinXpOrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT) && ((@this.Version.Major > 5) || ((@this.Version.Major == 5) && (@this.Version.Minor >= 1)));
        }

        public static bool IsWinVistaOrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT) && (@this.Version.Major >= 6);
        }

        public static bool IsWin7OrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT) && ((@this.Version.Major > 6) || ((@this.Version.Major == 6) && (@this.Version.Minor >= 1)));
        }

        public static bool IsWin8OrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT) && ((@this.Version.Major > 6) || ((@this.Version.Major == 6) && (@this.Version.Minor >= 2)));
        }

        public static bool IsWin81OrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT) && ((@this.Version.Major > 6) || ((@this.Version.Major == 6) && (@this.Version.Minor >= 3)));
        }

        public static bool IsWin10OrHigher(this OperatingSystem @this)
        {
            return (@this.Platform == PlatformID.Win32NT) && ((@this.Version.Major > 6) || ((@this.Version.Major == 10) && (@this.Version.Minor >= 0)));
        }

        #endregion Windows version checks
    }
}