namespace Goldenacre.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Sockets;

    public static class Network
    {
        /// <summary>
        ///     Makes a TcpClient connection to www.msftconnecttest.com:80
        ///     and returns the roundtrip time in milliseconds.
        ///     If an exception occurs then it returns -1.
        /// </summary>
        /// <param name="timeoutInMilliseconds">Give up if no response after this many seconds.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static long Ping(int timeoutInMilliseconds)
        {
            return Ping("www.msftconnecttest.com", 80, timeoutInMilliseconds);
        }


        /// <summary>
        ///     Makes a TcpClient connection to the specified socket
        ///     and returns the roundtrip time in milliseconds.
        ///     If an exception occurs then it returns -1.
        /// </summary>
        /// <param name="host">The url or ip to check.</param>
        /// <param name="port">The port to connect to.</param>
        /// <param name="timeoutInMilliseconds">Give up if no response after this many seconds.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static long Ping(string host, int port, int timeoutInMilliseconds)
        {
            var start = DateTime.UtcNow;
            long duration = -1;

            try
            {
                using (var tcp = new TcpClient())
                {
                    var result = tcp.BeginConnect(host, port, null, null);

                    using (var wait = result.AsyncWaitHandle)
                    {
                        if (!wait.WaitOne((int)TimeSpan.FromMilliseconds(timeoutInMilliseconds).TotalMilliseconds, false))
                        {
                            tcp.Close();
                        }

                        tcp.EndConnect(result);
                    }
                }

                duration = (long)DateTime.UtcNow.Subtract(start).TotalMilliseconds;
            }
            catch
            {
                duration = -1;
            }

            return duration;
        }
    }
}