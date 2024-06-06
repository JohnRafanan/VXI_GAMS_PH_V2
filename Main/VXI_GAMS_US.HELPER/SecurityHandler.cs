using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using VXI_GAMS_US.VIEWS;

namespace VXI_GAMS_US.HELPER
{
    /// <summary>
    /// Handles encryption and decryption
    /// </summary>
    public class SecurityHandler
    {
        private readonly TripleDESCryptoServiceProvider _des = new TripleDESCryptoServiceProvider();
        private readonly UTF8Encoding _utf8 = new UTF8Encoding();

        private byte[] Key { get; }

        private byte[] Vector { get; }

        /// <summary>
        /// Initialize the handler using the given key and vector
        /// </summary>
        /// <param name="key">The key to encrypt/decrypt</param>
        /// <param name="vector">The vector to be used in the algorithm</param>
        public SecurityHandler(string key, string vector)
        {
            Key = Convert.FromBase64String(key);
            Vector = Convert.FromBase64String(vector);
        }

        public SecurityHandler()
        {
            Key = Convert.FromBase64String("73tB175Zk4Mzk5SJACfBrwzJ73ATUIIO");
            Vector = Convert.FromBase64String("B5qYmjZ3zuc=");
        }
        
        /// <summary>
        /// Decrypt the given content
        /// </summary>
        /// <param name="bytes">The bytes of content to be decrypted</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] bytes)
        {
            return Transform(bytes, _des.CreateDecryptor(Key, Vector));
        }

        /// <summary>
        /// Encrypt the given content
        /// </summary>
        /// <param name="bytes">The bytes of content to be encrypted</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] bytes)
        {
            return Transform(bytes, _des.CreateEncryptor(Key, Vector));
        }

        /// <summary>
        /// Decrypt the given content
        /// </summary>
        /// <param name="text">The content to be decrypted</param>
        /// <returns></returns>
        public string Decrypt(string text)
        {
            try
            {
                return _utf8.GetString(Transform(HttpServerUtility.UrlTokenDecode(text), _des.CreateDecryptor(Key, Vector))).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
        

        /// <summary>
        /// Encrypt the given content
        /// </summary>
        /// <param name="text">The content to be encrypted</param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            return HttpServerUtility.UrlTokenEncode(Transform(_utf8.GetBytes(text), _des.CreateEncryptor(Key, Vector)));
        }

        private static byte[] Transform(byte[] input, ICryptoTransform cryptoTransform)
        {
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(input, 0, input.Length);
                cryptoStream.FlushFinalBlock();
                memoryStream.Position = 0L;
                var buffer = new byte[memoryStream.Length];
                memoryStream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// Create a new vector for the algorithm
        /// </summary>
        /// <returns></returns>
        public static string CreateNewVector()
        {
            using (var cryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                cryptoServiceProvider.GenerateIV();
                return Convert.ToBase64String(cryptoServiceProvider.IV);
            }
        }

        /// <summary>
        /// Create a new key for the algorithm
        /// </summary>
        /// <returns></returns>
        public static string CreateNewKey()
        {
            using (var cryptoServiceProvider = new TripleDESCryptoServiceProvider())
            {
                cryptoServiceProvider.GenerateKey();
                return Convert.ToBase64String(cryptoServiceProvider.Key);
            }
        }
    }
}
