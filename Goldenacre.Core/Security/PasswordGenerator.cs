using System;
using System.Security.Cryptography;
using System.Text;

namespace Goldenacre.Core.Security
{

    public sealed class PasswordGenerator
    {
        private const int DefaultMaximum = 10;
        private const int DefaultMinimum = 6;
        private const int UBoundDigit = 61;

        private readonly char[] _pwdCharArray =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".
                ToCharArray();

        private readonly RNGCryptoServiceProvider _rng;
        private int _maxSize;
        private int _minSize;

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

            _rng = new RNGCryptoServiceProvider();
        }

        public string CharacterExclusions { get; set; }

        public int MinimumLength
        {
            get { return _minSize; }
            set
            {
                _minSize = value;

                if (DefaultMinimum > _minSize)
                {
                    _minSize = DefaultMinimum;
                }
            }
        }

        public int MaximumLength
        {
            get { return _maxSize; }
            set
            {
                _maxSize = value + 1;

                if (_minSize >= _maxSize)
                {
                    _maxSize = DefaultMaximum;
                }
            }
        }

        public bool ExcludeSymbols { get; set; }
        public bool AllowRepeatCharacters { get; set; }
        public bool AllowConsecutiveCharacters { get; set; }

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
                _rng.GetBytes(rndnum);
                urndnum = BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);

            return (int) (urndnum%(uBound - lBound)) + lBound;
        }

        private char GetRandomCharacter()
        {
            var upperBound = _pwdCharArray.GetUpperBound(0);

            if (ExcludeSymbols)
            {
                upperBound = UBoundDigit;
            }

            var randomCharPosition = GetCryptographicRandomNumber(_pwdCharArray.GetLowerBound(0), upperBound);

            var randomChar = _pwdCharArray[randomCharPosition];

            return randomChar;
        }

        public string Generate()
        {
            var pwdLength = GetCryptographicRandomNumber(MinimumLength, MaximumLength);

            var pwdBuffer = new StringBuilder {Capacity = MaximumLength};
            var lastCharacter = '\n';

            for (var i = 0; i < pwdLength; i++)
            {
                var nextCharacter = GetRandomCharacter();

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

            return pwdBuffer.ToString();
        }
    }
}