using System;
using System.Security.Cryptography;
using System.Text;

namespace VSW.Lib.Global
{
    public static class Security
    {
        public static string Md5(string s)
        {
            var bytes = new UnicodeEncoding().GetBytes(s);
            var hasBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
            return BitConverter.ToString(hasBytes);
        }
    }
}