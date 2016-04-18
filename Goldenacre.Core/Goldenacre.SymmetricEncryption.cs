// ReSharper disable CheckNamespace

namespace Goldenacre.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public abstract class SymmetricEncryptionBase<T>
        where T : SymmetricAlgorithm, new()
    {
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Encrypt(byte[] text, byte[] password, byte[] salt)
        {
            using (var rgb = new Rfc2898DeriveBytes(password, salt, 1000))
            {
                using (var algorithm = new T())
                {
                    var rgbKey = rgb.GetBytes(algorithm.KeySize); // >> 3);
                    var rgbIv = rgb.GetBytes(algorithm.BlockSize); // >> 3);

                    using (var transform = algorithm.CreateEncryptor(rgbKey, rgbIv))
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

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string Decrypt(byte[] text, byte[] password, byte[] salt)
        {
            using (var rgb = new Rfc2898DeriveBytes(password, salt, 1000))
            {
                using (var algorithm = new T())
                {
                    var rgbKey = rgb.GetBytes(algorithm.KeySize); // >> 3);
                    var rgbIv = rgb.GetBytes(algorithm.BlockSize); // >> 3);

                    using (var transform = algorithm.CreateDecryptor(rgbKey, rgbIv))
                    {
                        using (var buffer = new MemoryStream(text))
                        using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                        using (var reader = new StreamReader(stream, Encoding.Unicode))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    public sealed class Aes : SymmetricEncryptionBase<AesManaged>
    {
        //
    }

    public sealed class Rijndael : SymmetricEncryptionBase<RijndaelManaged>
    {
        //
    }
}