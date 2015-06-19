using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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


        public static string TrimIfNotNullAndToLowerInvariant(this string @this)
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

        public static bool IsStrongPassword(this string @this)
        {
            var isStrong = Regex.IsMatch(@this, @"[\d]");
            if (isStrong) isStrong = Regex.IsMatch(@this, @"[a-z]");
            if (isStrong) isStrong = Regex.IsMatch(@this, @"[A-Z]");
            if (isStrong) isStrong = Regex.IsMatch(@this, @"[\s~!@#\$%\^&\*\(\)\{\}\|\[\]\\:;'?,.`+=<>\/]");
            if (isStrong) isStrong = @this.Length > 7;
            return isStrong;
        }

        public static string ToPlural(this string @this, int count = 0)
        {
            return count == 1 ? @this : PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(@this);
        }

        public static bool IsDate(this string @this)
        {
            if (!string.IsNullOrEmpty(@this))
            {
                DateTime dt;
                return (DateTime.TryParse(@this, out dt));
            }
            return false;
        }

        public static bool IsGuid(this string @this)
        {
            if (@this == null)
                throw new ArgumentNullException("s");

            var format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
            var match = format.Match(@this);

            return match.Success;
        }

        public static string SubstringToIndexOf(this string @this, string value,
            StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            var idx = @this.IndexOf(value, StringComparison.InvariantCultureIgnoreCase);

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
        /// <param name="input">String value to parse</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string @this)
        {
            return ToEnum<T>(@this, false);
        }

        /// <summary>
        ///     Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="input">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string @this, bool ignorecase)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                throw new ArgumentNullException("@this");
            }

            @this = @this.Trim();

            var t = typeof (T);

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

        public static string Frmat(this string @this, params object[] args)
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

        /// <summary>
        ///     Converts the string to the specified type, using the default value configured for the type.
        /// </summary>
        /// <typeparam name="T">Type the string will be converted to. The type must implement IConvertable.</typeparam>
        /// <param name="original">The original string.</param>
        /// <returns>The converted value.</returns>
        public static T ConvertTo<T>(this string @this)
        {
            return ConvertTo(@this, CultureInfo.CurrentCulture, default(T));
        }

        /// <summary>
        ///     Converts the string to the specified type, using the default value configured for the type.
        /// </summary>
        /// <typeparam name="T">Type the string will be converted to.</typeparam>
        /// <param name="original">The original string.</param>
        /// <param name="defaultValue">
        ///     The default value to use in case the original string is null or empty, or can't be
        ///     converted.
        /// </param>
        /// <returns>The converted value.</returns>
        public static T ConvertTo<T>(this string @this, T defaultValue)
        {
            return ConvertTo(@this, CultureInfo.CurrentCulture, defaultValue);
        }

        /// <summary>
        ///     Converts the string to the specified type, using the default value configured for the type.
        /// </summary>
        /// <typeparam name="T">Type the string will be converted to.</typeparam>
        /// <param name="original">The original string.</param>
        /// <param name="provider">Format provider used during the type conversion.</param>
        /// <returns>The converted value.</returns>
        public static T ConvertTo<T>(this string @this, IFormatProvider provider)
        {
            return ConvertTo(@this, provider, default(T));
        }

        /// <summary>
        ///     Converts the string to the specified type.
        /// </summary>
        /// <typeparam name="T">Type the string will be converted to.</typeparam>
        /// <param name="original">The original string.</param>
        /// <param name="provider">Format provider used during the type conversion.</param>
        /// <param name="defaultValue">
        ///     The default value to use in case the original string is null or empty, or can't be
        ///     converted.
        /// </param>
        /// <returns>The converted value.</returns>
        /// <remarks>
        ///     If an error occurs while converting the specified value to the requested type, the exception is caught and the
        ///     default is returned. It is strongly recommended you
        ///     do NOT use this method if it is important that conversion failures are not swallowed up.
        ///     This method is intended to be used to convert string values to primatives, not for parsing, converting, or
        ///     deserializing complex types.
        /// </remarks>
        public static T ConvertTo<T>(this string @this, IFormatProvider provider,
            T defaultValue)
        {
            T result;
            var type = typeof (T);

            if (string.IsNullOrEmpty(@this)) result = defaultValue;
            else
            {
                // need to get the underlying type if T is Nullable<>.

                if (type.IsNullable())
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                try
                {
                    // ChangeType doesn't work properly on Enums
                    result = type.IsEnum
                        ? (T)Enum.Parse(type, @this, true)
                        : (T)Convert.ChangeType(@this, type, provider);
                }
                catch
                    // HACK: what can we do to minimize or avoid raising exceptions as part of normal operation? custom string parsing (regex?) for well-known types? it would be best to know if you can convert to the desired type before you attempt to do so.
                {
                    result = defaultValue;
                }
            }

            return result;
        }

        /// <summary>
        ///     Indicates whether the current string matches the supplied wildcard pattern.  Behaves the same
        ///     as VB's "Like" Operator.
        /// </summary>
        /// <param name="s">The string instance where the extension method is called</param>
        /// <param name="wildcardPattern">The wildcard pattern to match.  Syntax matches VB's Like operator.</param>
        /// <returns>true if the string matches the supplied pattern, false otherwise.</returns>
        /// <remarks>
        ///     "abc".IsLike("a*"); // true
        ///     "Abc".IsLike("[A-Z][a-z][a-z]"); // true
        ///     "abc123".IsLike("*###"); // true
        ///     "hat".IsLike("?at"); // true
        ///     "joe".IsLike("[!aeiou]*"); // true
        ///     "joe".IsLike("?at"); // false
        ///     "joe".IsLike("[A-Z][a-z][a-z]"); // false
        /// </remarks>
        public static bool IsLike(this string @this, string wildcardPattern)
        {
            if (@this == null || string.IsNullOrEmpty(wildcardPattern)) return false;
            // turn into regex pattern, and match the whole string with ^$
            var regexPattern = "^" + Regex.Escape(wildcardPattern) + "$";

            // add support for ?, #, *, [], and [!]
            regexPattern = regexPattern.Replace(@"\[!", "[^")
                .Replace(@"\[", "[")
                .Replace(@"\]", "]")
                .Replace(@"\?", ".")
                .Replace(@"\*", ".*")
                .Replace(@"\#", @"\d");

            bool result;
            try
            {
                result = Regex.IsMatch(@this, regexPattern);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(string.Format("Invalid pattern: {0}", wildcardPattern), ex);
            }
            return result;
        }

        public static string RemoveAllWhitespace(this string @this)
        {
            if (@this == null) throw new ArgumentNullException("@this");

            @this = @this.Replace("\t", string.Empty);

            while (@this.Contains(SingleSpace))
            {
                @this = @this.Replace(SingleSpace, string.Empty);
            }

            return @this;
        }

        public static string TrimAndRemoveConsecutiveWhitespace(this string @this)
        {
            if (@this == null) throw new ArgumentNullException("text");

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
        /// <param name="input">The sentence to convert.</param>
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
        /// <param name="input">The string to encode.</param>
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
        /// <param name="input">The string to encode.</param>
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
                throw new ArgumentNullException("@this");
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
    }
}