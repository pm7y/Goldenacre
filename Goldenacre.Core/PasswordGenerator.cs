using System;
using System.Security.Cryptography;
using System.Text;

namespace Goldenacre.Core
{
    /// <summary>
    /// </summary>
    public sealed class PasswordGenerator
    {
        private const int DefaultMaximum = 10;
        private const int DefaultMinimum = 6;
        private const int UBoundDigit = 61;

        private readonly char[] pwdCharArray =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".
                ToCharArray();

        private readonly RNGCryptoServiceProvider rng;
        private int maxSize;
        private int minSize;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public PasswordGenerator()
        {
            MinimumLength = DefaultMinimum;
            MaximumLength = DefaultMaximum;
            AllowConsecutiveCharacters = false;
            AllowRepeatCharacters = true;
            ExcludeSymbols = false;
            CharacterExclusions = null;

            rng = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// </summary>
        public string CharacterExclusions { get; set; }

        /// <summary>
        /// </summary>
        public int MinimumLength
        {
            get { return minSize; }
            set
            {
                minSize = value;

                if (DefaultMinimum > minSize)
                {
                    minSize = DefaultMinimum;
                }
            }
        }

        /// <summary>
        /// </summary>
        public int MaximumLength
        {
            get { return maxSize; }
            set
            {
                maxSize = value + 1;

                if (minSize >= maxSize)
                {
                    maxSize = DefaultMaximum;
                }
            }
        }

        /// <summary>
        /// </summary>
        public bool ExcludeSymbols { get; set; }

        /// <summary>
        /// </summary>
        public bool AllowRepeatCharacters { get; set; }

        /// <summary>
        /// </summary>
        public bool AllowConsecutiveCharacters { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="lBound"></param>
        /// <param name="uBound"></param>
        /// <returns></returns>
        private int GetCryptographicRandomNumber(int lBound, int uBound)
        {
            uint urndnum;
            var rndnum = new byte[4];

            if (lBound == uBound - 1)
            {
                return lBound;
            }

            var xcludeRndBase = (uint.MaxValue - (uint.MaxValue%(uint) (uBound - lBound)));

            do
            {
                rng.GetBytes(rndnum);
                urndnum = BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);

            return (int) (urndnum%(uBound - lBound)) + lBound;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private char GetRandomCharacter()
        {
            var upperBound = pwdCharArray.GetUpperBound(0);

            if (ExcludeSymbols)
            {
                upperBound = UBoundDigit;
            }

            var randomCharPosition = GetCryptographicRandomNumber(pwdCharArray.GetLowerBound(0), upperBound);

            var randomChar = pwdCharArray[randomCharPosition];

            return randomChar;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            var pwdLength = GetCryptographicRandomNumber(MinimumLength, MaximumLength);

            var pwdBuffer = new StringBuilder();
            pwdBuffer.Capacity = MaximumLength;
            char lastCharacter, nextCharacter;

            lastCharacter = nextCharacter = '\n';

            for (var i = 0; i < pwdLength; i++)
            {
                nextCharacter = GetRandomCharacter();

                if (false == AllowConsecutiveCharacters)
                {
                    while (lastCharacter == nextCharacter)
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                if (false == AllowRepeatCharacters)
                {
                    var temp = pwdBuffer.ToString();
                    var duplicateIndex = temp.IndexOf(nextCharacter);
                    while (-1 != duplicateIndex)
                    {
                        nextCharacter = GetRandomCharacter();
                        duplicateIndex = temp.IndexOf(nextCharacter);
                    }
                }

                if ((null != CharacterExclusions))
                {
                    while (-1 != CharacterExclusions.IndexOf(nextCharacter))
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }

            if (null != pwdBuffer)
            {
                return pwdBuffer.ToString();
            }
            return string.Empty;
        }
    }
}