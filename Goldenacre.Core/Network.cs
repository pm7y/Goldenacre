using System;
using System.Net.Sockets;

namespace Goldenacre.Core
{
    public static class Network
    {
        /// <summary>
        ///     Makes a TcpClient connection to the specified socket
        ///     and returns the roundtrip time in milliseconds.
        ///     If an exception occurs then it returns -1.
        /// </summary>
        /// <param name="host">The url or ip to check.</param>
        /// <param name="port">The port to connect to.</param>
        /// <param name="timeoutInSeconds">Give up if no response after this many seconds.</param>
        public static long Ping(string host, int port = 80, int timeoutInSeconds = 15)
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
                        if (!wait.WaitOne(timeoutInSeconds*1000, false))
                        {
                            tcp.Close();
                        }

                        tcp.EndConnect(result);
                    }
                }

                duration = (long) DateTime.UtcNow.Subtract(start).TotalMilliseconds;
            }
            catch
            {
                duration = -1;
            }

            return duration;
        }
    }
}