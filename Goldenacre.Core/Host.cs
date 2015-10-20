using Goldenacre.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Goldenacre.Core
{
    public static class Host
    {
        [DllImport("kernel32")]
        private static extern ulong GetTickCount64();

        public static long TickCount()
        {
            return (long)GetTickCount64();
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

        public static string[] Domains()
        {
            var domainList = new List<string>();
            var ctx = new DirectoryContext(DirectoryContextType.Domain);

            try
            {
                using (var currentDomain = Domain.GetDomain(ctx))
                using (var forest = currentDomain.Forest)
                {
                    var domains = forest.Domains;

                    foreach (Domain d in domains)
                    {
                        domainList.AddIfNotContains(d.Name.ToLowerInvariant());
                    }

                    return domainList.ToArray();
                }
            }
            catch (ActiveDirectoryOperationException)
            {
                return null;
            }
        }
    }
}