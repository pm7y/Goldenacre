using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Goldenacre.Core
{
    public class Host
    {
        [DllImport("kernel32")]
        public static extern ulong GetTickCount64();

        public static TimeSpan GetUpTime()
        {
            return TimeSpan.FromMilliseconds(GetTickCount64());
        }

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