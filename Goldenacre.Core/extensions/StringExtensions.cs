using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

// ReSharper disable CheckNamespace

namespace Goldenacre.Extensions
{
    public static class StringExtensions
    {
        private const string SingleSpace = " ";
        private const string DoubleSpace = "  ";

        public static bool IsNullOrWhiteSpace(this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsAnyCS(this string @this, params string[] input)
        {
            return input.Any(s => s.Equals(@this, StringComparison.CurrentCulture));
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsAnyCS(this string @this, IEnumerable<string> input)
        {
            return input.Any(s => s.Equals(@this, StringComparison.CurrentCulture));
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsAnyCI(this string @this, params string[] input)
        {
            return input.Any(s => s.Equals(@this, StringComparison.InvariantCultureIgnoreCase));
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsCI(this string @this, IEnumerable<string> input)
        {
            return input.Any(s => s.Equals(@this, StringComparison.InvariantCultureIgnoreCase));
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsCI(this string @this, string value2)
        {
            return @this.Equals(value2, StringComparison.InvariantCultureIgnoreCase);
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsCS(this string @this, string value2)
        {
            return @this.Equals(value2, StringComparison.CurrentCulture);
        }

        public static string TrimToLowerInvariantIfNotNull(this string @this)
        {
            if (!string.IsNullOrEmpty(@this))
            {
                return @this.Trim().ToLowerInvariant();
            }
            return @this;
        }

        public static string TrimIfNotNull(this string @this)
        {
            if (!string.IsNullOrEmpty(@this))
            {
                return @this.Trim();
            }
            return @this;
        }

        public static bool ContainsAll(this string @this, params string[] values)
        {
            return values.All(one => @this.ToLowerInvariant().Contains(one.ToLowerInvariant()));
        }

        public static string ToPlural(this string @this, int count = 0)
        {
            return count == 1 ? @this : PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(@this);
        }

        public static bool IsNumeric(this string @this)
        {
            if (!string.IsNullOrEmpty(@this))
            {
                decimal num;
                return decimal.TryParse(@this, NumberStyles.Any, CultureInfo.CurrentCulture, out num);
            }
            return false;
        }

        public static bool IsDate(this string @this)
        {
            if (!string.IsNullOrEmpty(@this))
            {
                DateTime dt;
                return DateTime.TryParse(@this, out dt);
            }
            return false;
        }

        public static bool IsGuid(this string @this)
        {
            if (@this == null)
                throw new ArgumentNullException();

            var format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
            var match = format.Match(@this);

            return match.Success;
        }

        public static string SubstringToIndexOf(this string @this, string value,
            StringComparison comparison = StringComparison.CurrentCulture)
        {
            var idx = @this.IndexOf(value, comparison);

            if (idx > 0)
            {
                return @this.Substring(0, idx);
            }

            if (idx == 0)
            {
                return string.Empty;
            }

            return @this;
        }

        /// <summary>
        ///     Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="this">String value to parse</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string @this)
        {
            return ToEnum<T>(@this, false);
        }

        /// <summary>
        ///     Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="this">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string @this, bool ignorecase)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                throw new ArgumentNullException();
            }

            @this = @this.Trim();

            var t = typeof(T);

            if (!t.IsEnum)
            {
                throw new ArgumentException("Not an enum value!");
            }

            return (T)Enum.Parse(t, @this, ignorecase);
        }

        public static string DapiEncrypt(this string @this)
        {
            return
                Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(@this), null,
                    DataProtectionScope.LocalMachine));
        }

        public static string DapiDecrypt(this string @this)
        {
            return
                Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(@this), null,
                    DataProtectionScope.LocalMachine));
        }

        public static string F(this string @this, params object[] args)
        {
            return string.Format(@this, args);
        }

        public static int NthIndexOf(this string @this, string match, int occurrence)
        {
            var i = 1;
            var index = 0;

            while (i <= occurrence &&
                   (index = @this.IndexOf(match, index + 1, StringComparison.InvariantCultureIgnoreCase)) != -1)
            {
                if (i == occurrence)
                {
                    return index;
                }

                i++;
            }

            return -1;
        }

        public static string RemoveAllWhitespace(this string @this)
        {
            if (@this == null) throw new ArgumentNullException();

            @this = @this.Replace("\t", string.Empty);

            while (@this.Contains(SingleSpace))
            {
                @this = @this.Replace(SingleSpace, string.Empty);
            }

            return @this;
        }

        public static string TrimAndRemoveConsecutiveWhitespace(this string @this)
        {
            if (@this == null) throw new ArgumentNullException();

            @this = @this.Replace("\t", SingleSpace).Trim();

            while (@this.Contains(DoubleSpace))
            {
                @this = @this.Replace(DoubleSpace, SingleSpace);
            }

            return @this;
        }

        public static string SplitOnCapitals(string @this)
        {
            var result = new StringBuilder(@this.Length);
            var countSinceLastSpace = 0;
            for (var i = 0; i < @this.Length - 1; i++)
            {
                result.Append(@this[i]);
                if (countSinceLastSpace > 2 && @this[i] != ' ' && @this[i] != '-' &&
                    (char.IsUpper(@this[i + 1]) || !char.IsDigit(@this[i]) && char.IsDigit(@this[i + 1])))
                {
                    result.Append(' ');
                    countSinceLastSpace = 0;
                }
                countSinceLastSpace++;
            }
            result.Append(@this[@this.Length - 1]);

            return result.ToString();
        }

        public static string ToTitleCase(this string @this)
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(@this);
        }

        /// <summary>
        ///     Converts the first letter of each word in a
        ///     space delimited sentence into uppercase
        ///     and converts all others into lowercase.
        /// </summary>
        /// <param name="this">The sentence to convert.</param>
        /// <param name="ignore">A list of words to ignore. Useful for abbreviations etc.</param>
        /// <returns>Pascal cased sentence.</returns>
        public static string ToPascalCase(this string @this, params string[] ignore)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            var strReturn = string.Empty;

            if (@this.Trim().Length > 0)
            {
                var objSb = new StringBuilder();
                var leftSpace = new string(@this.TakeWhile(c => c == ' ').ToArray());
                var rightSpace = new string(@this.Reverse().TakeWhile(c => c == ' ').ToArray());
                var arrWords = @this.Trim().Split(new[] { ' ' }, StringSplitOptions.None);

                foreach (var word in arrWords)
                {
                    if (word != string.Empty && !ignore.Contains(word.ToAlphaNumeric()))
                    {
                        if (word.Length > 1 && word[0] == '\'' || word[0] == '"' || word[0] == '(' || word[0] == '[')
                        {
                            objSb.Append(char.ToUpperInvariant(word[0]));
                            objSb.Append(char.ToUpperInvariant(word[1]));
                            objSb.Append(word.Substring(2).ToLowerInvariant()); // Convert rest of word to LOWERCASE
                        }
                        else
                        {
                            objSb.Append(char.ToUpperInvariant(word[0])); // Convert 1st char to UPPERCASE
                            objSb.Append(word.Substring(1).ToLowerInvariant()); // Convert rest of word to LOWERCASE
                        }
                        objSb.Append(" ");
                    }
                    else
                    {
                        objSb.Append(word).Append(" ");
                    }
                }

                strReturn = leftSpace +
                            objSb.ToString()
                                .Replace(" The ", " the ")
                                .Replace(" And ", " and ")
                                .Replace(" A ", " a ")
                                .Replace(" To ", " to ")
                                .Trim() + rightSpace;
            }

            return strReturn;
        }

        public static string ToAlphaNumeric(this string @this)
        {
            return new string(@this.Where(char.IsLetterOrDigit).ToArray());
        }

        /// <summary>
        ///     Encodes the specified string to a HEX encoded MD5Hash.
        /// </summary>
        /// <param name="this">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string ToHexMd5Hash(this string @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            var md5Hasher = new MD5CryptoServiceProvider();
            var hashedDataBytes = md5Hasher.ComputeHash(Encoding.Default.GetBytes(@this));

            // step 2, convert byte array to hex string
            var sb = new StringBuilder();
            foreach (var t in hashedDataBytes)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        ///     Encodes the specified string to a Base64 encoded MD5Hash.
        /// </summary>
        /// <param name="this">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string ToBase64Md5Hash(this string @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            var md5Hasher = new MD5CryptoServiceProvider();
            var hashedDataBytes = md5Hasher.ComputeHash(Encoding.Default.GetBytes(@this));

            return Convert.ToBase64String(hashedDataBytes);
        }

        public static string ToAspNetPasswordHash(string @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            byte[] salt;
            byte[] bytes;

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(@this, 16, 1000))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(32);
            }

            var inArray = new byte[49];
            Buffer.BlockCopy(salt, 0, inArray, 1, 16);
            Buffer.BlockCopy(bytes, 0, inArray, 17, 32);

            return Convert.ToBase64String(inArray);
        }

        public static string Truncate(this string @this, int maxLength, bool appendEllipsis)
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }

            if (appendEllipsis)
            {
                maxLength = Math.Max(0, maxLength - 3);
                return string.Concat(@this.Substring(0, maxLength), "...");
            }

            return @this.Substring(0, maxLength);
        }
    }
}