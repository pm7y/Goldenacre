using System;

namespace Goldenacre.Core
{
    /// <summary>
    ///     A static wrapper around System.Random.
    /// </summary>
    public static class StaticRandom
    {
        private static readonly Random _random = new Random();
        private static readonly object _lock = new object();

        public static int Next()
        {
            lock (_lock)
            {
                return _random.Next();
            }
        }

        public static int Next(int max)
        {
            lock (_lock)
            {
                return _random.Next(max);
            }
        }

        public static int Next(int min, int max)
        {
            lock (_lock)
            {
                return _random.Next(min, max);
            }
        }

        public static double NextDouble()
        {
            lock (_lock)
            {
                return _random.NextDouble();
            }
        }

        public static void NextBytes(byte[] buffer)
        {
            lock (_lock)
            {
                _random.NextBytes(buffer);
            }
        }
    }
}