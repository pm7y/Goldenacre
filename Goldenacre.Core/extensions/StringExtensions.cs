using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Goldenacre.Core.Extensions
{
    public static class StringExtensions
    {
        const string SingleSpace = " ";
        const string DoubleSpace = "  "; 

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

        /// <summary>
        ///     Converts the first letter of each word in a
        ///     space delimited sentence into uppercase
        ///     and converts all others into lowercase.
        /// </summary>
        /// <param name="input">The sentence to convert.</param>
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
            return new string(input.Where(c => char.IsLetterOrDigit(c)).ToArray());
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
            for (var i = 0; i < hashedDataBytes.Length; i++)
            {
                sb.Append(hashedDataBytes[i].ToString("X2"));
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