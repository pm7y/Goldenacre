using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Goldenacre.Core.Security.SymmetricEncryption
{
    public abstract class SymmetricEncryptionBase<T> where T : SymmetricAlgorithm, new()
    {

        public static string Encrypt(string text, string password, string salt)
        {
            using (var rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt)))
            {
                using (var algorithm = new T())
                {
                    var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
                    var rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);

                    using (var transform = algorithm.CreateEncryptor(rgbKey, rgbIv))
                    {
                        using (var buffer = new MemoryStream())
                        {
                            using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                            {
                                using (var writer = new StreamWriter(stream, Encoding.Unicode))
                                {
                                    writer.Write(text);
                                }
                            }

                            return Convert.ToBase64String(buffer.ToArray());
                        }
                    }
                }
            }
        }

        public static string Decrypt(string text, string password, string salt)
        {
            using (var rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt)))
            {
                using (var algorithm = new T())
                {
                    var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
                    var rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);

                    using (var transform = algorithm.CreateDecryptor(rgbKey, rgbIv))
                    {
                        using (var buffer = new MemoryStream(Convert.FromBase64String(text)))
                        {
                            using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                            {
                                using (var reader = new StreamReader(stream, Encoding.Unicode))
                                {
                                    return reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}