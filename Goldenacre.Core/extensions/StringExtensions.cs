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

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsAnyCS(this string value, params string[] input)
        {
            return input.Any(s => s.Equals(value, StringComparison.CurrentCulture));
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsAnyCS(this string value, IEnumerable<string> input)
        {
            return input.Any(s => s.Equals(value, StringComparison.CurrentCulture));
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsAnyCI(this string value, params string[] input)
        {
            return input.Any(s => s.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsCI(this string value, IEnumerable<string> input)
        {
            return input.Any(s => s.Equals(value, StringComparison.InvariantCultureIgnoreCase));
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsCI(this string value, string value2)
        {
            return value.Equals(value2, StringComparison.InvariantCultureIgnoreCase);
        }

        // ReSharper disable once InconsistentNaming
        public static bool EqualsCS(this string value, string value2)
        {
            return value.Equals(value2, StringComparison.CurrentCulture);
        }


        public static string TrimIfNotNullAndToLowerInvariant(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return s.Trim().ToLowerInvariant();
            }
            return s;
        }

        public static string TrimIfNotNull(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return s.Trim();
            }
            return s;
        }

        public static bool ContainsAll(this string value, params string[] values)
        {
            return values.All(one => value.ToLowerInvariant().Contains(one.ToLowerInvariant()));
        }

        public static bool IsStrongPassword(this string s)
        {
            var isStrong = Regex.IsMatch(s, @"[\d]");
            if (isStrong) isStrong = Regex.IsMatch(s, @"[a-z]");
            if (isStrong) isStrong = Regex.IsMatch(s, @"[A-Z]");
            if (isStrong) isStrong = Regex.IsMatch(s, @"[\s~!@#\$%\^&\*\(\)\{\}\|\[\]\\:;'?,.`+=<>\/]");
            if (isStrong) isStrong = s.Length > 7;
            return isStrong;
        }

        public static string ToPlural(this string input, int count = 0)
        {
            return count == 1 ? input : PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(input);
        }

        public static bool IsDate(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                DateTime dt;
                return (DateTime.TryParse(input, out dt));
            }
            return false;
        }

        public static bool IsGuid(this string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            var format = new Regex(
                "^[A-Fa-f0-9]{32}$|" +
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
            var match = format.Match(s);

            return match.Success;
        }

        public static string SubstringToIndexOf(this string input, string value,
            StringComparison comparison = StringComparison.InvariantCultureIgnoreCase)
        {
            var idx = input.IndexOf(value, StringComparison.InvariantCultureIgnoreCase);

            if (idx > 0)
            {
                return input.Substring(0, idx);
            }

            if (idx == 0)
            {
                return string.Empty;
            }

            return input;
        }

        /// <summary>
        ///     Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="input">String value to parse</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string input)
        {
            return ToEnum<T>(input, false);
        }

        /// <summary>
        ///     Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="input">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string input, bool ignorecase)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException("input");
            }

            input = input.Trim();

            var t = typeof (T);

            if (!t.IsEnum)
            {
                throw new ArgumentException("Not an enum value!");
            }

            return (T) Enum.Parse(t, input, ignorecase);
        }

        public static string DapiEncrypt(this string text)
        {
            return
                Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(text), null,
                    DataProtectionScope.LocalMachine));
        }

        public static string DapiDecrypt(this string text)
        {
            return
                Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(text), null,
                    DataProtectionScope.LocalMachine));
        }

        public static string Format(this string intput, params object[] args)
        {
            return string.Format(intput, args);
        }

        public static int NthIndexOf(this string input, string match, int occurrence)
        {
            var i = 1;
            var index = 0;

            while (i <= occurrence &&
                   (index = input.IndexOf(match, index + 1, StringComparison.InvariantCultureIgnoreCase)) != -1)
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
        public static T ConvertTo<T>(this string original)
        {
            return ConvertTo(original, CultureInfo.CurrentCulture, default(T));
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
        public static T ConvertTo<T>(this string original, T defaultValue)
        {
            return ConvertTo(original, CultureInfo.CurrentCulture, defaultValue);
        }

        /// <summary>
        ///     Converts the string to the specified type, using the default value configured for the type.
        /// </summary>
        /// <typeparam name="T">Type the string will be converted to.</typeparam>
        /// <param name="original">The original string.</param>
        /// <param name="provider">Format provider used during the type conversion.</param>
        /// <returns>The converted value.</returns>
        public static T ConvertTo<T>(this string original, IFormatProvider provider)
        {
            return ConvertTo(original, provider, default(T));
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
        public static T ConvertTo<T>(this string original, IFormatProvider provider,
            T defaultValue)
        {
            T result;
            var type = typeof (T);

            if (string.IsNullOrEmpty(original)) result = defaultValue;
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
                        ? (T) Enum.Parse(type, original, true)
                        : (T) Convert.ChangeType(original, type, provider);
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
        public static bool IsLike(this string s, string wildcardPattern)
        {
            if (s == null || string.IsNullOrEmpty(wildcardPattern)) return false;
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
                result = Regex.IsMatch(s, regexPattern);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(string.Format("Invalid pattern: {0}", wildcardPattern), ex);
            }
            return result;
        }

        public static string RemoveAllWhitespace(this string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            text = text.Replace("\t", string.Empty);

            while (text.Contains(SingleSpace))
            {
                text = text.Replace(SingleSpace, string.Empty);
            }

            return text;
        }

        public static string TrimAndRemoveConsecutiveWhitespace(this string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            text = text.Replace("\t", SingleSpace).Trim();

            while (text.Contains(DoubleSpace))
            {
                text = text.Replace(DoubleSpace, SingleSpace);
            }

            return text;
        }

        public static string SplitOnCapitals(string text)
        {
            var result = new StringBuilder(text.Length);
            var countSinceLastSpace = 0;
            for (var i = 0; i < text.Length - 1; i++)
            {
                result.Append(text[i]);
                if (countSinceLastSpace > 2 && text[i] != ' ' && text[i] != '-' &&
                    (char.IsUpper(text[i + 1]) || !char.IsDigit(text[i]) && char.IsDigit(text[i + 1])))
                {
                    result.Append(' ');
                    countSinceLastSpace = 0;
                }
                countSinceLastSpace++;
            }
            result.Append(text[text.Length - 1]);

            return result.ToString();
        }

        public static string ToTitleCase(this string input)
        {
            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input);
        }

        /// <summary>
        ///     Converts the first letter of each word in a
        ///     space delimited sentence into uppercase
        ///     and converts all others into lowercase.
        /// </summary>
        /// <param name="input">The sentence to convert.</param>
        /// <param name="ignore">A list of words to ignore. Useful for abbreviations etc.</param>
        /// <returns>Pascal cased sentence.</returns>
        public static string ToPascalCase(this string input, params string[] ignore)
        {
            if (input == null)
            {
                throw new ArgumentNullException();
            }

            var strReturn = string.Empty;

            if (input.Trim().Length > 0)
            {
                var objSb = new StringBuilder();
                var leftSpace = new string(input.TakeWhile(c => c == ' ').ToArray());
                var rightSpace = new string(input.Reverse().TakeWhile(c => c == ' ').ToArray());
                var arrWords = input.Trim().Split(new[] {' '}, StringSplitOptions.None);

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

        public static string ToAlphaNumeric(this string input)
        {
            return new string(input.Where(char.IsLetterOrDigit).ToArray());
        }

        /// <summary>
        ///     Encodes the specified string to a HEX encoded MD5Hash.
        /// </summary>
        /// <param name="input">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        public static string ToHexMd5Hash(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException();
            }

            var md5Hasher = new MD5CryptoServiceProvider();
            var hashedDataBytes = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

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
        public static string ToBase64Md5Hash(this string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException();
            }

            var md5Hasher = new MD5CryptoServiceProvider();
            var hashedDataBytes = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            return Convert.ToBase64String(hashedDataBytes);
        }

        public static string ToAspNetPasswordHash(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] salt;
            byte[] bytes;

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 1000))
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