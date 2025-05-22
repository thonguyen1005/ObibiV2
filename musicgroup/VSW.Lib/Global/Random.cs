using System.Security.Cryptography;
using System.Text;

namespace VSW.Lib.Global
{
    public static class Random
    {
        private static char[] _letter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        public static string GetRandom(int size)
        {
            var data = new byte[1];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(data);
                data = new byte[size];
                rng.GetNonZeroBytes(data);
            }

            var result = new StringBuilder(size);
            foreach (var o in data)
                result.Append(_letter[o % (_letter.Length)]);

            return result.ToString();
        }

        public static string GetRandom(int size, bool letterOrNumber)
        {
            _letter = letterOrNumber ? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray() : "1234567890".ToCharArray();

            return GetRandom(size);
        }
    }
}