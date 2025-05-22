using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VSW.Core.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    public static class EncryptionHelper
    {
        private const int INTERATIONS_DEFAULT = 4096;
        private const int SALT_SIZE_DEFAULT = 16;

        public static string ComputeMD5(Stream stream)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] r = md5.ComputeHash(stream);
            return r.Aggregate("", (current, b) => current + b.ToString("x2"));
        }

        public static string Sha256(string data)
        {
            byte[] bytes;
            using (var sha256Hash = SHA256.Create())
            {
                bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
            // Convert byte array to a string   
            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Tính MD5 với việc áp dụng Encoding.UTF8 cho tham số đầu vào
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ComputeMD5(string value)
        {
            byte[] b = Encoding.UTF8.GetBytes(value);
            return ComputeMD5(b);
        }

        public static string ComputeMD5(byte[] value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] r = md5.ComputeHash(value);
            return r.Aggregate("", (current, b) => current + b.ToString("x2"));
        }

        public static byte[] GenerateRandomData(int size = 16)
        {
            var entropy = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(entropy);
            return entropy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="saltSize"></param>
        /// <param name="iterations">Khuyến nghị tối thiểu 1000</param>
        /// <returns></returns>
        public static (string, byte[]) GenerateCryptographicKey(string password, int saltSize = SALT_SIZE_DEFAULT, int iterations = INTERATIONS_DEFAULT)
        {
            byte[] salt;
            using (var derivedBytes = new Rfc2898DeriveBytes(password, saltSize, iterations))
            {
                salt = derivedBytes.Salt;
                byte[] key = derivedBytes.GetBytes(16); // 128 bits key
                return (Convert.ToBase64String(key), salt);
            }
        }

        public static string GenerateCryptographicKey(string password, CryptoKey salt, int iterations = INTERATIONS_DEFAULT)
        {
            return GenerateCryptographicKey(password, salt.GetRawKey(), iterations);
        }

        public static string GenerateCryptographicKey(string password, byte[] salt, int iterations = INTERATIONS_DEFAULT)
        {
            using (var derivedBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                byte[] key = derivedBytes.GetBytes(16); // 128 bits key
                return Convert.ToBase64String(key);
            }
        }

        public static byte[] ComputeHmac256(CryptoKey salt, byte[] data)
        {
            return ComputeHmac256(salt.GetRawKey(), data);
        }

        public static byte[] ComputeHmac256(byte[] key, byte[] data)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(data);
            }
        }

        public static byte[] ComputeHash(byte[] data)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(data);
            }
        }


        #region Base64
        public static byte[] FromBase64(string s)
        {
            byte[] binaryData = Convert.FromBase64String(s);
            return binaryData;
        }

        /// <summary>
        /// Convert the binary input into Base64 UUEncoded output.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Base64(byte[] data)
        {
            string base64String = Convert.ToBase64String(data, 0, data.Length);
            return base64String;
        }     
        #endregion

        #region HexToByte
        /// <summary>
        /// Converts a byte array to a hexadecimal string.
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string ByteToHex(byte[] byteArray)
        {
            return byteArray.Aggregate("", (current, b) => current + b.ToString("X2"));
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToByte(string hexString)
        {
            var returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        #endregion

    }

}
